using System;
using System.Collections.Generic;
using System.Threading;

namespace dotSpace.BaseClasses.Utility
{
    class OSBTypeCollection
    {
        internal ReaderWriterLockSlim TypeLock { get; }
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
}