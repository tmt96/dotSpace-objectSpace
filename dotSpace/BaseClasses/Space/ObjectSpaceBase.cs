using System;
using System.Collections.Generic;
using System.Collections;
using dotSpace.Interfaces;
using System.Threading;
using System.Linq;
using System.Collections.Concurrent;

namespace dotSpace.BaseClasses.Space
{
    class OSBObjectCollection<T>
    {
        internal ReaderWriterLockSlim ObjectLock;
        internal List<T> ObjectList {get; }

        internal OSBObjectCollection(List<T> objectList = null, ReaderWriterLockSlim objectLock = null)
        {
            this.ObjectList = objectList ?? new List<T>();
            this.ObjectLock = objectLock ?? new ReaderWriterLockSlim();
        }
    }

    class OSBTypeCollection
    {
        internal ReaderWriterLockSlim TypeLock {get; }
        internal List<Type> TypeList { get; }

        internal OSBTypeCollection(List<Type> typeList = null, ReaderWriterLockSlim typeLock = null)
        {
            this.TypeList = typeList ?? new List<Type>();
            this.TypeLock = typeLock ?? new ReaderWriterLockSlim();
        }

        internal void AddType(Type t)
        {
            TypeLock.EnterWriteLock();
            TypeList.Add(t);
            TypeLock.ExitWriteLock();
        }
    }

    abstract class OSBEntry
    {
        protected internal OSBTypeCollection SubtypeCollection { get; protected set; }

        protected internal OSBTypeCollection SupertypeCollection { get; protected set; }
    }

    class OSBEntry<T> : OSBEntry
    {   
        internal OSBObjectCollection<T> ObjectCollection {get;}

        internal OSBEntry(OSBObjectCollection<T> objectCollection = null, OSBTypeCollection subtypeCollection = null, OSBTypeCollection supertypeCollection = null)
        {
            this.ObjectCollection = objectCollection ?? new OSBObjectCollection<T>();
            SubtypeCollection = subtypeCollection ?? new OSBTypeCollection();
            SupertypeCollection = supertypeCollection ?? new OSBTypeCollection();
        }
    }

    public abstract class ObjectSpaceBase : IObjectSpace
    {
        // private readonly ConcurrentDictionary<Type, IList> buckets;
        // private readonly ConcurrentDictionary<Type, (ReaderWriterLockSlim objectLock, ReaderWriterLockSlim subtypeLock)> bucketLocks;
        // private readonly ConcurrentDictionary<Type, IEnumerable<Type>> subtypesDict;

        private readonly ConcurrentDictionary<Type, OSBEntry> typeEntryDict;

        public ObjectSpaceBase()
        {
            // buckets = new ConcurrentDictionary<Type, IList>();
            // bucketLocks = new ConcurrentDictionary<Type, (ReaderWriterLockSlim, ReaderWriterLockSlim)>();
            // subtypesDict = new ConcurrentDictionary<Type, IEnumerable<Type>>();
            typeEntryDict = new ConcurrentDictionary<Type, OSBEntry>();
        }

        public T Get<T>()
        {
            var result = GetP<T>();
            Monitor.Enter(typeof(T));
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
            var result = GetP<T>(condition);
            Monitor.Enter(typeof(T));
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
            var objectCollection = entry.ObjectCollection;
            var supertypeCollection = entry.SupertypeCollection;
            var bucket = objectCollection.ObjectList;
            var rwLock = objectCollection.ObjectLock;

            Monitor.Enter(type);
            rwLock.EnterWriteLock();
            bucket.Insert(this.GetIndex(bucket.Count), element);
            rwLock.ExitWriteLock();
            Monitor.PulseAll(type);
            Monitor.Exit(type);

            foreach (var key in typeEntryDict.Keys)
            {
                if (type.IsSubclassOf(key))
                {
                    SetSubtype(type, key);
                }
                else if (key.IsSubclassOf(type))
                {
                    SetSubtype(key, type);
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
            var result = QueryP<T>();
            Monitor.Enter(typeof(T));
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
            var result = QueryP<T>(condition);
            Monitor.Enter(typeof(T));
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
            return GetObjectCollection<T>().ObjectList;
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
            var objectCollection = GetObjectCollection<T>();

            objectCollection.ObjectLock.EnterWriteLock();
            var element = objectCollection.ObjectList.FirstOrDefault(condition);
            objectCollection.ObjectList.Remove(element);
            objectCollection.ObjectLock.ExitWriteLock();
            return element;
        }

        private IEnumerable<T> RemoveAll<T>(Func<T, bool> condition)
        {
            var objectCollection = GetObjectCollection<T>();

            objectCollection.ObjectLock.EnterWriteLock();
            var elements = objectCollection.ObjectList.Where(condition);
            var pred = new Predicate<T>(condition);
            objectCollection.ObjectList.RemoveAll(pred);
            objectCollection.ObjectLock.ExitWriteLock();
            return elements;
        }


        private T Find<T>(Func<T, bool> condition)
        {
            var typeEntry = GetTypeEntry<T>();
            T result;
            if ((result = FindNoSubtype<T>(condition, typeEntry.ObjectCollection)) != null)
            {
                return result;
            }

            var subtypeCollection = typeEntry.SubtypeCollection;
            var subtypeLock = subtypeCollection.TypeLock;
            subtypeLock.EnterReadLock();
            var subtypeEntryList = subtypeCollection.TypeList.Select(type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            return subtypeEntryList
                .Select(l => FindNoSubtype<T>(
                    condition, (l as OSBEntry<T>).ObjectCollection))
                .Where(element => element != null)
                .FirstOrDefault();
        }

        private T FindNoSubtype<T>(Func<T, bool> condition, OSBObjectCollection<T> collection)
        {
            var rwLock = collection.ObjectLock;
            var bucket = collection.ObjectList;
            rwLock.EnterReadLock();
            var element = bucket.FirstOrDefault(condition);
            rwLock.ExitReadLock();
            return element;
        }

        private IEnumerable<T> FindAll<T>(Func<T, bool> condition)
        {
            var entry = GetTypeEntry<T>();
            var objectCollection = entry.ObjectCollection;
            objectCollection.ObjectLock.EnterReadLock();
            var list = objectCollection.ObjectList.Where(condition);
            objectCollection.ObjectLock.ExitReadLock();

            var subtypeCollection = entry.SubtypeCollection;
            var subtypeLock = subtypeCollection.TypeLock;
            subtypeLock.EnterReadLock();
            var subtypeEntryList = subtypeCollection.TypeList.Select(type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            list.Concat(
                subtypeEntryList.SelectMany(
                    l => FindAllNoSubtype<T>(
                        condition, (l as OSBEntry<T>).ObjectCollection)));
            return list;
        }

        private IEnumerable<T> FindAllNoSubtype<T>(Func<T, bool> condition, OSBObjectCollection<T> collection)
        {
            var rwLock = collection.ObjectLock;
            var bucket = collection.ObjectList;
            rwLock.EnterReadLock();
            var element = bucket.Where(condition);
            rwLock.ExitReadLock();
            return element;
        }


        private OSBEntry<T> GetTypeEntry<T>()
        {
            return typeEntryDict.GetOrAdd(typeof(T), new OSBEntry<T>()) as OSBEntry<T>;
        }

        private OSBObjectCollection<T> GetObjectCollection<T>() {
            return GetTypeEntry<T>().ObjectCollection;
        }

        private void SetSubtype(Type supertype, Type subtype)
        {
            var supertypeSubtypeCollection = typeEntryDict[supertype].SubtypeCollection;
            supertypeSubtypeCollection.AddType(subtype);

            var subtypeSupertypeCollection = typeEntryDict[subtype].SupertypeCollection;
            subtypeSupertypeCollection.AddType(supertype);
        }
    }
}