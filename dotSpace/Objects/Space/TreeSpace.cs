using System.Collections.Generic;
using System.Threading;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Utility;

namespace dotSpace.Objects.Space
{
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
        public ITuple Get(IPattern pattern)
        {
            return Get(pattern.Fields);
        }

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

        public IEnumerable<ITuple> GetAll(IPattern pattern)
        {
            return GetAll(pattern.Fields);
        }

        public IEnumerable<ITuple> GetAll(params object[] pattern)
        {
            readerWriterLock.EnterWriteLock();
            var resList = spaceTree.GetAll(pattern);
            readerWriterLock.ExitWriteLock();
            return resList;
        }

        public ITuple GetP(IPattern pattern)
        {
            return GetP(pattern.Fields);
        }

        public ITuple GetP(params object[] pattern)
        {
            readerWriterLock.EnterWriteLock();
            var tuple = spaceTree.Get(pattern);
            readerWriterLock.ExitWriteLock();
            return tuple;
        }

        public void Put(ITuple tuple)
        {
            Put(tuple.Fields);
        }

        public void Put(params object[] tuple)
        {
            Monitor.Enter(tLock);
            readerWriterLock.EnterWriteLock();
            spaceTree.Add(tuple);
            readerWriterLock.ExitWriteLock();
            Monitor.PulseAll(tLock);
            Monitor.Exit(tLock);
        }

        public ITuple Query(IPattern pattern)
        {
            return Query(pattern.Fields);
        }

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

        public IEnumerable<ITuple> QueryAll(IPattern pattern)
        {
            return QueryAll(pattern.Fields);
        }

        public IEnumerable<ITuple> QueryAll(params object[] pattern)
        {
            readerWriterLock.EnterReadLock();
            var resList = spaceTree.QueryAll(pattern);
            readerWriterLock.ExitReadLock();
            return resList;
        }

        public ITuple QueryP(IPattern pattern)
        {
            return QueryP(pattern.Fields);
        }

        public ITuple QueryP(params object[] pattern)
        {
            readerWriterLock.EnterReadLock();
            var tuple = spaceTree.Query(pattern);
            readerWriterLock.ExitReadLock();
            return tuple;
        }
    }
}