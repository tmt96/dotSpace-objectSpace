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

    class OSBSubtypeCollection
    {
        internal ReaderWriterLockSlim SubtypeLock {get; }
        internal List<Type> SubtypeList { get; }

        internal OSBSubtypeCollection(List<Type> subtypeList = null, ReaderWriterLockSlim subtypeLock = null)
        {
            this.SubtypeList = subtypeList ?? new List<Type>();
            this.SubtypeLock = subtypeLock ?? new ReaderWriterLockSlim();
        }
    }

    interface IOSBEntry
    {
    }

    class OSBEntry<T> : IOSBEntry
    {   
        internal OSBObjectCollection<T> ObjectCollection {get;}
        internal OSBSubtypeCollection SubtypeCollection {get;}

        internal OSBEntry(OSBObjectCollection<T> objectCollection = null, OSBSubtypeCollection subtypeCollection = null)
        {
            this.ObjectCollection = objectCollection ?? new OSBObjectCollection<T>();
            this.SubtypeCollection = subtypeCollection ?? new OSBSubtypeCollection();
        }
    }

    public abstract class ObjectSpaceBase : IObjectSpace
    {
        // private readonly ConcurrentDictionary<Type, IList> buckets;
        // private readonly ConcurrentDictionary<Type, (ReaderWriterLockSlim objectLock, ReaderWriterLockSlim subtypeLock)> bucketLocks;
        // private readonly ConcurrentDictionary<Type, IEnumerable<Type>> subtypesDict;

        private readonly ConcurrentDictionary<Type, IOSBEntry> typeEntryDict;

        public ObjectSpaceBase()
        {
            // buckets = new ConcurrentDictionary<Type, IList>();
            // bucketLocks = new ConcurrentDictionary<Type, (ReaderWriterLockSlim, ReaderWriterLockSlim)>();
            // subtypesDict = new ConcurrentDictionary<Type, IEnumerable<Type>>();
            typeEntryDict = new ConcurrentDictionary<Type, IOSBEntry>();
        }

        public T Get<T>()
        {
            var bucket = GetObjectBucket<T>();
            var result = GetP<T>();
            Monitor.Enter(bucket);
            while (result == null)
            {
                Monitor.Wait(bucket);
                result = GetP<T>();
            }
            Monitor.Exit(bucket);
            return result;
        }

        public T Get<T>(Func<T, bool> condition)
        {
            var bucket = GetObjectBucket<T>();
            var result = GetP<T>(condition);
            Monitor.Enter(bucket);
            while (result == null)
            {
                Monitor.Wait(bucket);
                result = GetP<T>(condition);
            }
            Monitor.Exit(bucket);
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
            var objectCollection = GetObjectCollection<T>();
            var bucket = objectCollection.ObjectList;
            var rwLock = objectCollection.ObjectLock;

            Monitor.Enter(bucket);
            rwLock.EnterWriteLock();
            bucket.Insert(this.GetIndex(bucket.Count), element);
            rwLock.ExitWriteLock();
            Monitor.PulseAll(bucket);
            Monitor.Exit(bucket);
        }

        public T Query<T>()
        {
            var bucket = GetObjectBucket<T>();
            var result = QueryP<T>();
            Monitor.Enter(bucket);
            while (result == null)
            {
                Monitor.Wait(bucket);
                result = QueryP<T>();
            }
            Monitor.Exit(bucket);
            return result;
        }

        public T Query<T>(Func<T, bool> condition)
        {
            var bucket = GetObjectBucket<T>();
            var result = QueryP<T>(condition);
            Monitor.Enter(bucket);
            while (result == null)
            {
                Monitor.Wait(bucket);
                result = QueryP<T>(condition);
            }
            Monitor.Exit(bucket);
            return result;
        }

        public IEnumerable<T> QueryAll<T>()
        {
            return GetObjectBucket<T>();
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
            var subtypeLock = subtypeCollection.SubtypeLock;
            subtypeLock.EnterReadLock();
            var subtypeList = subtypeCollection.SubtypeList.Select(type => typeEntryDict[type]).ToList();
            subtypeLock.ExitReadLock();

            return subtypeList
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
            var objectCollection = GetObjectCollection<T>();
            objectCollection.ObjectLock.EnterReadLock();
            var list = objectCollection.ObjectList.Where(condition);
            objectCollection.ObjectLock.ExitReadLock();
            return list;
        }

        private OSBObjectCollection<T> GetObjectCollection<T>() {
            var entry = typeEntryDict.GetOrAdd(typeof(T), new OSBEntry<T>()) as OSBEntry<T>;
            return entry.ObjectCollection;
        }

        private ReaderWriterLockSlim GetReaderWriterLock<T>()
        {
            var entry = typeEntryDict.GetOrAdd(typeof(T), new OSBEntry<T>()) as OSBEntry<T>;
            return entry.ObjectCollection.ObjectLock;
        }

        private List<T> GetObjectBucket<T>()
        {
            // return buckets.GetOrAdd(typeof(T), new List<T>()) as List<T>;
            var entry = typeEntryDict.GetOrAdd(typeof(T), new OSBEntry<T>()) as OSBEntry<T>;
            return entry.ObjectCollection.ObjectList;
        }

        private OSBEntry<T> GetTypeEntry<T>()
        {
            return typeEntryDict.GetOrAdd(typeof(T), new OSBEntry<T>()) as OSBEntry<T>;
        }
    }
}