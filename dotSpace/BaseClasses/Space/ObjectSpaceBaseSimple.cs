using System;
using System.Collections.Generic;
using System.Collections;
using dotSpace.Interfaces;
using System.Threading;
using System.Linq;
using System.Collections.Concurrent;
using dotSpace.BaseClasses.Utility;

namespace dotSpace.BaseClasses.Space
{

    public abstract class ObjectSpaceBaseSimple : IObjectSpaceSimple
    {
        private readonly ConcurrentDictionary<Type, OSBEntry> typeEntryDict;

        public ObjectSpaceBaseSimple()
        {
            typeEntryDict = new ConcurrentDictionary<Type, OSBEntry>();
        }

        public T Get<T>()
        {
            Monitor.Enter(typeof(T));
            var result = GetP<T>();
            while (result == null)
            {
                Monitor.Wait(typeof(T));
                result = GetP<T>();
            }
            Monitor.Exit(typeof(T));
            return result;
        }

        public T Get<T>(Func<T, bool> condition)
        {
            Monitor.Enter(typeof(T));
            var result = GetP<T>(condition);
            while (result == null)
            {
                Monitor.Wait(typeof(T));
                result = GetP<T>(condition);
            }
            Monitor.Exit(typeof(T));
            return result;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return RemoveAll<T>(o => true);
        }

        public IEnumerable<T> GetAll<T>(Func<T, bool> condition)
        {
            return RemoveAll(condition);
        }

        public T GetP<T>()
        {
            return RemoveFirst<T>(o => true);
        }

        public T GetP<T>(Func<T, bool> condition)
        {
            return RemoveFirst<T>(condition);
        }

        public void Put<T>(T element)
        {
            var type = typeof(T);
            var wasAdded = typeEntryDict.ContainsKey(type);

            var entry = GetTypeEntry<T>();
            var supertypeCollection = entry.SupertypeCollection;

            Monitor.Enter(type);
            entry.Add(element, GetIndex(entry.Size));
            Monitor.PulseAll(type);
            Monitor.Exit(type);

            if(!wasAdded)
            {
                foreach (var key in typeEntryDict.Keys)
                {
                    if (type.IsSubclassOf(key))
                    {
                        SetSubtype(key, type);
                    }
                    else if (key.IsSubclassOf(type))
                    {
                        SetSubtype(type, key);
                    }
                }
            }

            supertypeCollection.TypeLock.EnterReadLock();
            foreach (var t in supertypeCollection.TypeList)
            {
                Monitor.Enter(t);
                Monitor.PulseAll(t);
                Monitor.Exit(t);
            }
            supertypeCollection.TypeLock.ExitReadLock();
        }

        public T Query<T>()
        {
            Monitor.Enter(typeof(T));
            var result = QueryP<T>();
            while (result == null)
            {
                Monitor.Wait(typeof(T));
                result = QueryP<T>();
            }
            Monitor.Exit(typeof(T));
            return result;
        }

        public T Query<T>(Func<T, bool> condition)
        {
            Monitor.Enter(typeof(T));
            var result = QueryP<T>(condition);
            while (result == null)
            {
                Monitor.Wait(typeof(T));
                result = QueryP<T>(condition);
            }
            Monitor.Exit(typeof(T));
            return result;
        }

        public IEnumerable<T> QueryAll<T>()
        {
            return GetTypeEntry<T>().ObjectCollection.ObjectList;
        }

        public IEnumerable<T> QueryAll<T>(Func<T, bool> condition)
        {
            return FindAll<T>(condition);
        }

        public T QueryP<T>()
        {
            return Find<T>(t => true);
        }

        public T QueryP<T>(Func<T, bool> condition)
        {
            return Find<T>(condition);
        }


        protected abstract int GetIndex(int count);

        private T RemoveFirst<T>(Func<T, bool> condition)
        {
            var typeEntry = GetTypeEntry<T>();
            var result = typeEntry.Remove(condition);
            if (result != null)
            {
                return result;
            }

            var subtypeCollection = typeEntry.SubtypeCollection;
            var subtypeLock = subtypeCollection.TypeLock;
            subtypeLock.EnterReadLock();
            var subtypeEntryList = subtypeCollection.TypeList.Select(type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            var allEntry = subtypeEntryList.Select(l => l.Remove(condition));
            var satisfied = allEntry.Where(element => element != null);
            return satisfied.FirstOrDefault();
        }

        private IEnumerable<T> RemoveAll<T>(Func<T, bool> condition)
        {
            var entry = GetTypeEntry<T>();
            var list = entry.RemoveAll(condition);

            var subtypeCollection = entry.SubtypeCollection;
            var subtypeLock = subtypeCollection.TypeLock;
            subtypeLock.EnterReadLock();
            var subtypeEntryList = subtypeCollection.TypeList.Select(type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            list.Concat(subtypeEntryList.SelectMany(
                            l => l.RemoveAll(condition)));
            return list;
        }

        private T Find<T>(Func<T, bool> condition)
        {
            var typeEntry = GetTypeEntry<T>();
            T result;
            if ((result = typeEntry.Get(condition)) != null)
            {
                return result;
            }

            var subtypeCollection = typeEntry.SubtypeCollection;
            var subtypeLock = subtypeCollection.TypeLock;
            subtypeLock.EnterReadLock();
            var subtypeEntryList = subtypeCollection.TypeList.Select(type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            return subtypeEntryList
                .Select(l => l.Get(condition))
                .Where(element => element != null)
                .FirstOrDefault();
        }

        private IEnumerable<T> FindAll<T>(Func<T, bool> condition)
        {
            var entry = GetTypeEntry<T>();
            var list = entry.GetAll(condition);

            var subtypeCollection = entry.SubtypeCollection;
            var subtypeLock = subtypeCollection.TypeLock;
            subtypeLock.EnterReadLock();
            var subtypeEntryList = subtypeCollection.TypeList.Select(
                type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            list.Concat(subtypeEntryList.SelectMany(
                                l => l.GetAll(condition)));
            return list;
        }

        private OSBEntry<T> GetTypeEntry<T>()
        {
            return typeEntryDict.GetOrAdd(typeof(T), new OSBEntry<T>()) as OSBEntry<T>;
        }

        private void SetSubtype(Type supertype, Type subtype)
        {
            typeEntryDict[supertype].AddSubtype(subtype);
            typeEntryDict[subtype].AddSupertype(supertype);
        }
    }
}
