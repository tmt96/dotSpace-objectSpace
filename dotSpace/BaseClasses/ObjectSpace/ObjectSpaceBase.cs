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

    public abstract class ObjectSpaceBase : IObjectSpace
    {
        private readonly ConcurrentDictionary<Type, OSBEntry> typeEntryDict;

        /// <summary>
        /// Create new instance of ObjectSpaceBase
        /// </summary>
        public ObjectSpaceBase()
        {
            typeEntryDict = new ConcurrentDictionary<Type, OSBEntry>();
        }

        /// <summary>
        /// Retrieves and removes the first object from the space of type T. The operation will block if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
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

        /// <summary>
        /// Retrieves and removes the first object from the space of type T matching condition. The operation will block if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition</returns>
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

        /// <summary>
        /// Retrieves and removes all objects from the space of type T. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <returns>A collection of all object of type T from the space</returns>
        public IEnumerable<T> GetAll<T>()
        {
            return RemoveAll<T>(o => true);
        }

        /// <summary>
        /// Retrieves and removes all objects from the space of type T matching the condition. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <param name="condition">he condition the object needs to match</param>
        /// <returns>A collection of all object of type T from the space matching the condition</returns>
        public IEnumerable<T> GetAll<T>(Func<T, bool> condition)
        {
            return RemoveAll(condition);
        }

        /// <summary>
        /// Retrieves and removes the first object from the space of type T. The operation returns null if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        public T GetP<T>()
        {
            return RemoveFirst<T>(o => true);
        }

        /// <summary>
        /// Retrieves and removes the first object from the space of type T matching condition. The operation returns null if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition, or null if no element satisfies</returns>
        public T GetP<T>(Func<T, bool> condition)
        {
            return RemoveFirst<T>(condition);
        }

        /// <summary>
        /// Inserts an object of type T to the object space.
        /// </summary>
        /// <param name="element">The object ot be inserted to the space</param>
        public void Put<T>(T element)
        {
            // find the entry corresponds to type T
            var type = typeof(T);
            var wasAdded = typeEntryDict.ContainsKey(type);

            var entry = GetTypeEntry<T>();
            var supertypeCollection = entry.SupertypeCollection;

            // add element to entry
            Monitor.Enter(type);
            entry.Add(element, GetIndex(entry.Size));
            Monitor.PulseAll(type);
            Monitor.Exit(type);

            // add subtypes & supertyeps appropriately
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

            // notify waiting threads
            supertypeCollection.TypeLock.EnterReadLock();
            foreach (var t in supertypeCollection.TypeList)
            {
                Monitor.Enter(t);
                Monitor.PulseAll(t);
                Monitor.Exit(t);
            }
            supertypeCollection.TypeLock.ExitReadLock();
        }

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T. The operation will block if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
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

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T matching condition. The operation will block if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition</returns>
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

        /// <summary>
        /// Retrieves clones of all objects from the space of type T. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <returns>A collection of all object of type T from the space</returns>
        public IEnumerable<T> QueryAll<T>()
        {
            return GetTypeEntry<T>().ObjectCollection.ObjectList;
        }

        /// <summary>
        /// Retrieves clones of all objects from the space of type T matching the condition. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <param name="condition">he condition the object needs to match</param>
        /// <returns>A collection of all object of type T from the space matching the condition</returns>
        public IEnumerable<T> QueryAll<T>(Func<T, bool> condition)
        {
            return FindAll<T>(condition);
        }

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T. The operation returns null if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        public T QueryP<T>()
        {
            return Find<T>(t => true);
        }

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T matching condition. The operation returns null if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition, or null if no element satisfies</returns>
        public T QueryP<T>(Func<T, bool> condition)
        {
            return Find<T>(condition);
        }


        protected abstract int GetIndex(int count);

        /// <summary>
        /// Remove first element of type T matching condition
        /// </summary>
        /// <returns>The entry to be removed<returns>
        private T RemoveFirst<T>(Func<T, bool> condition)
        {
            // remove in the main entry
            var typeEntry = GetTypeEntry<T>();
            var result = typeEntry.Remove(condition);
            if (result != null)
            {
                return result;
            }

            // if cannot remove in main entry, remove from subtype entris
            var subtypeCollection = typeEntry.SubtypeCollection;
            var subtypeLock = subtypeCollection.TypeLock;
            subtypeLock.EnterReadLock();
            var subtypeEntryList = subtypeCollection.TypeList.Select(type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            var allEntry = subtypeEntryList.Select(l => l.Remove(condition));
            var satisfied = allEntry.Where(element => element != null);
            return satisfied.FirstOrDefault();
        }

        /// <summary>
        /// remove all elements of type T matching the condition
        /// </summary>
        /// <returns>the collection of all removed elements/returns>
        private IEnumerable<T> RemoveAll<T>(Func<T, bool> condition)
        {
            // remove elements from main entry
            var entry = GetTypeEntry<T>();
            var list = entry.RemoveAll(condition);

            // remove elements from subtype entry
            var subtypeCollection = entry.SubtypeCollection;
            var subtypeLock = subtypeCollection.TypeLock;
            subtypeLock.EnterReadLock();
            var subtypeEntryList = subtypeCollection.TypeList.Select(type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            list.Concat(subtypeEntryList.SelectMany(
                            l => l.RemoveAll(condition)));
            return list;
        }

        /// <summary>
        /// Find the first element of type T matching condition
        /// </summary>
        private T Find<T>(Func<T, bool> condition)
        {
            // find in main entry
            var typeEntry = GetTypeEntry<T>();
            T result;
            if ((result = typeEntry.Get(condition)) != null)
            {
                return result;
            }

            // find in subtype entries
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

        /// <summary>
        /// Find the all elements of type T matching condition
        /// </summary>
        private IEnumerable<T> FindAll<T>(Func<T, bool> condition)
        {
            // find in main entry
            var entry = GetTypeEntry<T>();
            var list = entry.GetAll(condition);

            // find in subtype entries
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
