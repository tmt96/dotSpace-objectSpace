using System;
using System.Collections.Generic;
using System.Linq;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;

namespace dotSpace.Objects.Utility
{
    public class TupleTree
    {
        private enum TupleHandleOption { KEEP, REMOVE };
        private int Count { get; set; }
        private Dictionary<object, TupleTree> lookupTable;

        public TupleTree()
        {
            this.Count = 0;
            this.lookupTable = new Dictionary<object, TupleTree>();
        }

        public void Add(params object[] tuple)
        {
            var curTree = this;

            for (int i = 0; i < tuple.Length; i++)
            {
                if (!curTree.lookupTable.ContainsKey(tuple[i]))
                {
                    curTree.lookupTable[tuple[i]] = new TupleTree();
                }
                curTree = curTree[tuple[i]];
            }

            curTree.Count++;
        }

        public ITuple Query(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.Query(TupleHandleOption.KEEP, 0, res, pattern);
        }

        public IEnumerable<ITuple> QueryAll(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.QueryAll(TupleHandleOption.KEEP, 0, res, pattern);
        }

        public ITuple Get(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.Query(TupleHandleOption.REMOVE, 0, res, pattern);
        }

        public IEnumerable<ITuple> GetAll(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.QueryAll(TupleHandleOption.REMOVE, 0, res, pattern);
        }

        private ITuple Query(TupleHandleOption option, int cur, List<object> res, object[] pattern)
        {
            if (cur >= pattern.Length)
            {
                if (Count > 0)
                {
                    if (option == TupleHandleOption.REMOVE)
                    {
                        Count--;
                    }
                    return new Space.Tuple(res.ToArray());
                }
                return null;
            }

            ITuple tuple = null;

            if (pattern[cur] is Type)
            {
                foreach (var item in lookupTable)
                {
                    var key = item.Key;
                    if (MatchedType((Type)pattern[cur], key))
                    {
                        res.Add(key);
                        tuple = this[key].Query(option, cur + 1, res, pattern);
                        if (tuple!= null)
                        {
                            break;
                        }
                        res.RemoveAt(res.Count - 1);
                    }
                }
            }
            else if (lookupTable.ContainsKey(pattern[cur]))
            {
                res.Add(pattern[cur]);
                tuple = this[pattern[cur]].Query(option, cur + 1, res, pattern);
            }
            if (option == TupleHandleOption.REMOVE && tuple != null)
            {
                RemoveEmptyChild(tuple[cur]);
            }
            return tuple;
        }

        private List<Space.Tuple> QueryAll(TupleHandleOption option, int cur, List<object> res, object[] pattern)
        {
            if (cur >= pattern.Length)
            {
                var result = Enumerable.Repeat(new dotSpace.Objects.Space.Tuple(res.ToArray()), Count).ToList();
                if (option == TupleHandleOption.REMOVE)
                {
                    Count = 0;
                }
                return result;
            }

            var resList = new List<Space.Tuple>();
            var matchedKeysList = new List<object>();
            if (pattern[cur] is Type)
            {
                foreach (var item in lookupTable)
                {
                    var key = item.Key;
                    if (MatchedType((Type)pattern[cur], key))
                    {
                        res.Add(key);
                        var subResult = this[key].QueryAll(option, cur + 1, res, pattern);
                        if (subResult.Count() != 0)
                        {
                            matchedKeysList.Add(key);
                        }
                        resList.AddRange(subResult);
                        res.RemoveAt(res.Count - 1);
                    }
                }
            }
            else if (lookupTable.ContainsKey(pattern[cur]))
            {
                res.Add(pattern[cur]);
                resList = this[pattern[cur]].QueryAll(option, cur + 1, res, pattern);
                matchedKeysList.Add(pattern[cur]);
            }

            if (option == TupleHandleOption.REMOVE)
            {
                foreach (var key in matchedKeysList)
                {
                    RemoveEmptyChild(key);
                }
            }
            return resList;
        }

        private bool MatchedType(Type type, object obj)
        {
            Type elementType = obj is Type ? (Type)obj : obj.GetType();
            return type.IsAssignableFrom(elementType);
        }

        private void RemoveEmptyChild(object key)
        {
            if (this[key].lookupTable.Count == 0 && this[key].Count == 0)
            {
                this.lookupTable.Remove(key);
            }
        }

        public TupleTree this[object key] => this.lookupTable[key];
    }
}