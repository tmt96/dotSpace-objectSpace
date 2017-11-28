using System;
using System.Collections.Generic;
using System.Linq;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;

namespace dotSpace.Objects.Utility
{
    public class TupleTree
    {
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

            lock (curTree)
            {
                curTree.Count++;
            }
        }

        public ITuple Query(params object[] pattern)
        {
            var res = new object[pattern.Length];
            return this.Query(0, res, pattern);
        }

        public IEnumerable<ITuple> QueryAll(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.QueryAll(0, res, pattern);
        }

        public ITuple Get(params object[] pattern)
        {
            var res = new object[pattern.Length];
            return this.Get(0, res, pattern);
        }

        public IEnumerable<ITuple> GetAll(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.GetAll(0, res, pattern);
        }


        private ITuple Query(int cur, object[] res, object[] pattern)
        {
            if (cur >= pattern.Length)
            {
                lock (this)
                {
                    return Count > 0 ? new dotSpace.Objects.Space.Tuple(res) : null;
                }
            }           
            if (pattern[cur] is Type)
            {
                foreach (var item in lookupTable)
                {
                    var key = item.Key;
                    if (MatchedType((Type)pattern[cur], key))
                    {
                        res[cur] = key;
                        var subResult = this[key].Query(cur + 1, res, pattern);
                        if (subResult != null)
                        {
                            return subResult;
                        }
                        res[cur] = null;
                    }
                }
                return null;
            } 
            else if (lookupTable.ContainsKey(pattern[cur]))
            {
                res[cur] = pattern[cur];
                return this[pattern[cur]].Query(cur + 1, res, pattern);
            }

            return null;
        }

        private IEnumerable<ITuple> QueryAll( int cur, List<object> res, object[] pattern)
        {
            IEnumerable<ITuple> resList = new List<ITuple>();
            if (cur >= pattern.Length)
            {
                lock (this)
                {
                    resList = Enumerable.Repeat(new dotSpace.Objects.Space.Tuple(res.ToArray()), Count).ToList();
                }
            }
            if (pattern[cur] is Type)
            {
                foreach (var item in lookupTable)
                {
                    var key = item.Key;
                    if (MatchedType((Type)pattern[cur], key))
                    {
                        res.Add(key);
                        var subResult = this[key].QueryAll(cur + 1, res, pattern);
                        if (subResult != null)
                        {
                            resList = resList.Concat(subResult).ToList();
                        }
                        res.RemoveAt(res.Count - 1);
                    }
                }
            }
            else if (lookupTable.ContainsKey(pattern[cur]))
            {
                res.Add(pattern[cur]);
                resList = this[pattern[cur]].QueryAll(cur + 1, res, pattern);
            }
            return resList;
        }

        private ITuple Get(int cur, object[] res, object[] pattern)
        {
            ITuple tuple = null;
            object removedKey = null;
            if (cur >= pattern.Length)
            {
                lock (this)
                {
                    if (Count > 0)
                    {
                        Count--;
                        tuple = new dotSpace.Objects.Space.Tuple(res);
                    }
                }
            }
            else if (pattern[cur] is Type)
            {
                foreach (var item in lookupTable)
                {
                    // var item = lookupTable.ElementAt(0);
                    var key = item.Key;
                    if (MatchedType((Type)pattern[cur], key))
                    {
                        res[cur] = key;
                        tuple = this[key].Get(cur + 1, res, pattern);
                        if (tuple != null)
                        {
                            removedKey = key;
                            break;
                        }
                        // res[cur] = null;
                    }
                }
            }
            else if (lookupTable.ContainsKey(pattern[cur]))
            {
                res[cur] = pattern[cur];
                tuple =  this[pattern[cur]].Get( cur + 1, res, pattern);
                removedKey = pattern[cur];
            }
            if (removedKey != null)
            {
                RemoveEmptyChild(removedKey);
            }
            return tuple;
        }

        private IEnumerable<ITuple> GetAll(int cur, List<object> res, object[] pattern)
        {
            IEnumerable<ITuple> resList = new List<ITuple>();
            var removedKeys = new List<object>();
            if (cur >= pattern.Length)
            {
                lock (this)
                {
                    resList = Enumerable.Repeat(new dotSpace.Objects.Space.Tuple(res.ToArray()), Count);
                    Count = 0;
                }
            }
            else if (pattern[cur] is Type)
            {
                foreach (var item in lookupTable)
                {
                    var key = item.Key;
                    if (MatchedType((Type) pattern[cur], key))
                    {
                        res.Add(key);
                        var subResult = this[key].GetAll(cur + 1, res, pattern);
                        if (subResult.Count() != 0)
                        {
                            removedKeys.Add(key);
                        }
                        resList = resList.Concat(subResult);
                        res.RemoveAt(res.Count - 1);
                    }
                }
            }
            else if (lookupTable.ContainsKey(pattern[cur]))
            {
                res.Add(pattern[cur]);
                resList = this[pattern[cur]].GetAll(cur + 1, res, pattern);
                removedKeys.Add(pattern[cur]); 
            }
            foreach (var key in removedKeys)
            {
                RemoveEmptyChild(key);
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