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
            var bucket = GetBucket<T>();
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
            var bucket = GetBucket<T>();
            var rwLock = GetReaderWriterLock<T>();

            Monitor.Enter(bucket);
            rwLock.EnterWriteLock();
            bucket.Insert(this.GetIndex(bucket.Count), element);
            rwLock.ExitWriteLock();
            Monitor.PulseAll(bucket);
            Monitor.Exit(bucket);
        }

        public T Query<T>()
        {
            var bucket = GetBucket<T>();
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
            var bucket = GetBucket<T>();
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
            return GetBucket<T>();
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
            var (bucket, rwLock) = GetBucketAndRWLock<T>();

            rwLock.EnterWriteLock();
            var element = bucket.FirstOrDefault(condition);
            bucket.Remove(element);
            rwLock.ExitWriteLock();
            return element;
        }

        private IEnumerable<T> RemoveAll<T>(Func<T, bool> condition)
        {
            var (bucket, rwLock) = GetBucketAndRWLock<T>();

            rwLock.EnterWriteLock();
            var elements = bucket.Where(condition);
            var pred = new Predicate<T>(condition);
            bucket.RemoveAll(pred);
            rwLock.ExitWriteLock();
            return elements;
        }


        private T Find<T>(Func<T, bool> condition)
        {
            var bucket = GetBucket<T>();
            var rwLock = GetReaderWriterLock<T>();
            rwLock.EnterReadLock();
            var t = bucket.FirstOrDefault(condition);
            rwLock.ExitReadLock();
            return t;
        }

        private IEnumerable<T> FindAll<T>(Func<T, bool> condition)
        {
            var bucket = GetBucket<T>();
            var rwLock = GetReaderWriterLock<T>();
            rwLock.EnterReadLock();
            var list = bucket.Where(condition);
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
    }
}