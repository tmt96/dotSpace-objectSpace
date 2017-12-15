using System.Collections.Generic;
using System.Threading;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Utility;

namespace dotSpace.Objects.Space
{
    /// <summary>
    /// A tuplespace implementation based on a hierarchy structure
    /// </summary>
    public class TreeSpace : ISpace
    {
        private TupleTree spaceTree;
        private object tLock;
        private ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        public TreeSpace()
        {
            spaceTree = new TupleTree();
            tLock = new object();
        }
        /// <summary>
        /// Retrieves and removes the first tuple from the space, matching the specified pattern. The operation will block if no elements match.
        /// </summary>
        public ITuple Get(IPattern pattern)
        {
            return Get(pattern.Fields);
        }

        /// <summary>
        /// Retrieves and removes the first tuple from the space, matching the specified pattern. The operation will block if no elements match.
        /// </summary>
        public ITuple Get(params object[] pattern)
        {
            Monitor.Enter(tLock);
            var tuple = GetP(pattern);
            while (tuple == null)
            {
                Monitor.Wait(tLock);
                tuple = GetP(pattern);
            }
            Monitor.Exit(tLock);
            return tuple;
        }

        /// <summary>
        /// Retrieves and removes all tuples from the space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        public IEnumerable<ITuple> GetAll(IPattern pattern)
        {
            return GetAll(pattern.Fields);
        }

        /// <summary>
        /// Retrieves and removes all tuples from the space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        public IEnumerable<ITuple> GetAll(params object[] pattern)
        {
            readerWriterLock.EnterWriteLock();
            var resList = spaceTree.GetAll(pattern);
            readerWriterLock.ExitWriteLock();
            return resList;
        }

        /// <summary>
        /// Retrieves and removes the first tuple from the space, matching the specified pattern. The operation is non-blocking. The operation will return null if no elements match.
        /// </summary>
        public ITuple GetP(IPattern pattern)
        {
            return GetP(pattern.Fields);
        }

        /// <summary>
        /// Retrieves and removes the first tuple from the space, matching the specified pattern. The operation is non-blocking. The operation will return null if no elements match.
        public ITuple GetP(params object[] pattern)
        {
            readerWriterLock.EnterWriteLock();
            var tuple = spaceTree.Get(pattern);
            readerWriterLock.ExitWriteLock();
            return tuple;
        }

        /// <summary>
        /// Inserts the tuple passed as argument into the space.
        /// </summary>
        public void Put(ITuple tuple)
        {
            Put(tuple.Fields);
        }

        /// <summary>
        /// Inserts the tuple passed as argument into the space.
        /// </summary>
        public void Put(params object[] tuple)
        {
            Monitor.Enter(tLock);
            readerWriterLock.EnterWriteLock();
            spaceTree.Add(tuple);
            readerWriterLock.ExitWriteLock();
            Monitor.PulseAll(tLock);
            Monitor.Exit(tLock);
        }

        /// <summary>
        /// Retrieves a clone of the first tuple from the space matching the specified pattern. The operation will block if no elements match.
        /// </summary>
        public ITuple Query(IPattern pattern)
        {
            return Query(pattern.Fields);
        }

        /// <summary>
        /// Retrieves a clone of the first tuple from the space matching the specified pattern. The operation will block if no elements match.
        /// </summary>
        public ITuple Query(params object[] pattern)
        {
            Monitor.Enter(tLock);
            var tuple = QueryP(pattern);
            while (tuple == null)
            {
                Monitor.Wait(tLock);
                tuple = QueryP(pattern);
            }
            Monitor.Exit(tLock);
            return tuple;
        }

        /// <summary>
        /// Retrieves clones of all tuples from the space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        public IEnumerable<ITuple> QueryAll(IPattern pattern)
        {
            return QueryAll(pattern.Fields);
        }

        /// <summary>
        /// Retrieves clones of all tuples from the space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        public IEnumerable<ITuple> QueryAll(params object[] pattern)
        {
            readerWriterLock.EnterReadLock();
            var resList = spaceTree.QueryAll(pattern);
            readerWriterLock.ExitReadLock();
            return resList;
        }

        /// <summary>
        /// Retrieves a clone of the first tuple from the space matching the specified pattern. The operation is non-blocking. The operation will return null if no elements match.
        /// </summary>
        public ITuple QueryP(IPattern pattern)
        {
            return QueryP(pattern.Fields);
        }

        /// <summary>
        /// Retrieves a clone of the first tuple from the space matching the specified pattern.The operation is non-blocking.The operation will return null if no elements match.
        /// </summary>
        public ITuple QueryP(params object[] pattern)
        {
            readerWriterLock.EnterReadLock();
            var tuple = spaceTree.Query(pattern);
            readerWriterLock.ExitReadLock();
            return tuple;
        }
    }
}