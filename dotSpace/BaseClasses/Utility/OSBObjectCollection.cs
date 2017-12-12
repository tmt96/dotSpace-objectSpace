using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace dotSpace.BaseClasses.Utility
{
    class OSBObjectCollection<T>
    {
        internal ReaderWriterLockSlim ObjectLock;
        internal List<T> ObjectList { get; }

        internal OSBObjectCollection(List<T> objectList = null, ReaderWriterLockSlim objectLock = null)
        {
            this.ObjectList = objectList ?? new List<T>();
            this.ObjectLock = objectLock ?? new ReaderWriterLockSlim();
        }

        internal void Add(T item)
        {
            if (item == null)
            {
                return;
            }
            ObjectLock.EnterWriteLock();
            ObjectList.Add(item);
            ObjectLock.ExitWriteLock();
        }

        internal void Add(T item, int pos)
        {
            if (item == null)
            {
                return;
            }
            ObjectLock.EnterWriteLock();
            ObjectList.Insert(pos, item);
            ObjectLock.ExitWriteLock();
        }

        internal T Get(Func<T, bool> condition)
        {
            ObjectLock.EnterReadLock();
            var result = ObjectList.FirstOrDefault(condition);
            ObjectLock.ExitReadLock();
            return result;
        }

        internal IEnumerable<T> GetAll(Func<T, bool> condition)
        {
            ObjectLock.EnterReadLock();
            var result = ObjectList.Where(condition);
            ObjectLock.ExitReadLock();
            return result;
        }

        internal T Remove(Func<T, bool> condition>)
        {
            ObjectLock.EnterWriteLock();
            var result = ObjectList.FirstOrDefault(condition);
            ObjectList.Remove(result);
            ObjectLock.ExitWriteLock();
            return result;
        }

        internal IEnumerable<T> RemoveAll(Func<T, bool> condition>)
        {
            ObjectLock.EnterWriteLock();
            var result = ObjectList.Where(condition);
            ObjectList.RemoveAll(new Predicate<T>(condition));
            ObjectLock.ExitWriteLock();
            return result;
        }

    }
}