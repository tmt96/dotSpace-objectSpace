using System;
using System.Collections.Generic;
using System.Collections;
using dotSpace.Interfaces;
using System.Threading;
using System.Linq;
using System.Collections.Concurrent;

namespace dotSpace.BaseClasses.Space
{
    public abstract class ObjectSpaceBase : IObjectSpace
    {
        private readonly ConcurrentDictionary<Type, IList> buckets;
        private readonly ConcurrentDictionary<Type, ReaderWriterLockSlim> bucketLocks;

        public ObjectSpaceBase()
        {
            buckets = new ConcurrentDictionary<Type, IList>();
            bucketLocks = new ConcurrentDictionary<Type, ReaderWriterLockSlim>();
        }

        public T Get<T>()
        {
            var bucket = GetBucket<T>();
            var result = GetP<T>();
            while (result == null)
            {
                Wait(bucket);
                result = GetP<T>();
            }
            return result;
        }

        public T Get<T>(Predicate<T> condition)
        {
            var bucket = GetBucket<T>();
            var result = GetP<T>(condition);
            while (result == null)
            {
                Wait(bucket);
                result = GetP<T>(condition);
            }
            return result;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return RemoveAll<T>(o => true);
        }

        public IEnumerable<T> GetAll<T>(Predicate<T> condition)
        {
            return RemoveAll(condition);
        }

        public T GetP<T>()
        {
            return RemoveFirst<T>(o => true);
        }

        public T GetP<T>(Predicate<T> condition)
        {
            return RemoveFirst<T>(condition);
        }

        public void Put<T>(T element)
        {
            var bucket = GetBucket<T>();
            var rwLock = GetReaderWriterLock<T>();

            rwLock.EnterWriteLock();
            bucket.Insert(this.GetIndex(bucket.Count), element);
            rwLock.ExitWriteLock();
            this.Awake(bucket);
        }

        public T Query<T>()
        {
            var bucket = GetBucket<T>();
            var result = QueryP<T>();
            while (result == null)
            {
                Wait(bucket);
                result = QueryP<T>();
            }
            return result;
        }

        public T Query<T>(Predicate<T> condition)
        {
            var bucket = GetBucket<T>();
            var result = QueryP<T>(condition);
            while (result == null)
            {
                Wait(bucket);
                result = QueryP<T>(condition);
            }
            return result;
        }

        public IEnumerable<T> QueryAll<T>()
        {
            return GetBucket<T>();
        }

        public IEnumerable<T> QueryAll<T>(Predicate<T> condition)
        {
            return FindAll<T>(condition);
        }

        public T QueryP<T>()
        {
            return Find<T>(t => true);
        }

        public T QueryP<T>(Predicate<T> condition)
        {
            return Find<T>(condition);
        }


        protected abstract int GetIndex(int count);

        private T RemoveFirst<T>(Predicate<T> condition)
        {
            var (bucket, rwLock) = GetBucketAndRWLock<T>();

            rwLock.EnterWriteLock();
            var element = bucket.Find(condition);
            bucket.Remove(element);
            rwLock.ExitWriteLock();
            return element;
        }

        private List<T> RemoveAll<T>(Predicate<T> condition)
        {
            var (bucket, rwLock) = GetBucketAndRWLock<T>();

            rwLock.EnterWriteLock();
            var elements = bucket.FindAll(condition);
            bucket.RemoveAll(condition);
            rwLock.ExitWriteLock();
            return elements;
        }


        private T Find<T>(Predicate<T> condition)
        {
            var bucket = GetBucket<T>();
            var rwLock = GetReaderWriterLock<T>();
            rwLock.EnterReadLock();
            var t = bucket.Find(condition);
            rwLock.ExitReadLock();
            return t;
        }

        private List<T> FindAll<T>(Predicate<T> condition)
        {
            var bucket = GetBucket<T>();
            var rwLock = GetReaderWriterLock<T>();
            rwLock.EnterReadLock();
            var list = bucket.FindAll(condition);
            rwLock.ExitReadLock();
            return list;
        }

        private (List<T> bucket, ReaderWriterLockSlim rwLock) GetBucketAndRWLock<T>() {
            return (GetBucket<T>(), GetReaderWriterLock<T>());
        }

        private ReaderWriterLockSlim GetReaderWriterLock<T>()
        {
            return bucketLocks.GetOrAdd(typeof(T), new ReaderWriterLockSlim());
        }

        private List<T> GetBucket<T>()
        {
            return buckets.GetOrAdd(typeof(T), new List<T>()) as List<T>;
        }

        private void Wait(object _lock)
        {
            Monitor.Enter(_lock);
            Monitor.Wait(_lock);
            Monitor.Exit(_lock);
        }
        private void Awake(object _lock)
        {
            Monitor.Enter(_lock);
            Monitor.PulseAll(_lock);
            Monitor.Exit(_lock);
        }
    }
}