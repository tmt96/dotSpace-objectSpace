using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;

namespace TreeSpace
{
    public class TupleTree
    {
        private int Count { get; set; }

        private ConcurrentDictionary<object, TupleTree> lookupTable;
        public TupleTree()
        {
            this.Count = 0;
            this.lookupTable = new ConcurrentDictionary<object, TupleTree>();
        }

        public void Add(params object[] tuple)
        {
            var curTree = this;

            for (int i = 0; i < tuple.Length; i++)
            {
                curTree = curTree.lookupTable.GetOrAdd(tuple[i], new TupleTree());
            }

            lock (curTree)
            {
                curTree.Count += 1;
            }
        }

        public ITuple Query(params object[] pattern)
        {
            var res = new object[pattern.Length];
            return this.Query(this, 0, res, pattern);
        }

        public IEnumerable<ITuple> QueryAll(params object[] pattern)
        {
            var res = new object[pattern.Length];
            return this.QueryAll(this, 0, res, pattern);
        }

        public ITuple Get(params object[] pattern)
        {
            var res = new object[pattern.Length];
            return this.Get(this, 0, res, pattern);
        }

        public IEnumerable<ITuple> GetAll(params object[] pattern)
        {
            var res = new object[pattern.Length];
            return this.GetAll(this, 0, res, pattern);
        }


        private ITuple Query(TupleTree tree, int cur, object[] res, object[] pattern)
        {
            if (cur >= pattern.Length)
            {
                lock (tree)
                {
                    return tree.Count > 0 ? new dotSpace.Objects.Space.Tuple(res) : null;
                }
            }           
            if (pattern[cur] is Type)
            {
                foreach (var key in tree.lookupTable.Keys)
                {
                    Type elementType = key is Type ? (Type)key : key.GetType();
                    if (((Type)pattern[cur]).IsAssignableFrom(elementType))
                    {
                        res[cur] = key;
                        var subResult = this.Query(tree[key], ++cur, res, pattern);
                        if (subResult != null)
                        {
                            return subResult;
                        }
                        res[cur] = null;
                    }
                }
                return null;
            } 
            else if (tree.lookupTable.ContainsKey(pattern[cur]))
            {
                res[cur] = pattern[cur];
                return this.Query(tree[pattern[cur]], ++cur, res, pattern);
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<ITuple> QueryAll(TupleTree tree, int cur, object[] res, object[] pattern)
        {
            if (cur >= pattern.Length)
            {
                lock (tree)
                {
                    var resList = Enumerable.Repeat(new dotSpace.Objects.Space.Tuple(res), tree.Count).ToList();
                    return resList;
                }
            }
            if (pattern[cur] is Type)
            {
                foreach (var key in tree.lookupTable.Keys)
                {
                    var resultList = new List<ITuple>();
                    Type elementType = key is Type ? (Type)key : key.GetType();
                    if (((Type)pattern[cur]).IsAssignableFrom(elementType))
                    {
                        res[cur] = key;
                        var subResult = this.QueryAll(tree[key], ++cur, res, pattern);
                        if (subResult != null)
                        {
                            resultList = resultList.Concat(subResult).ToList();
                        }
                        res[cur] = null;
                    }
                }
                return null;
            }
            else if (tree.lookupTable.ContainsKey(pattern[cur]))
            {
                res[cur] = pattern[cur];
                return this.QueryAll(tree[pattern[cur]], ++cur, res, pattern);
            }
            else
            {
                return new List<ITuple>();
            }
        }

        private ITuple Get(TupleTree tree, int cur, object[] res, object[] pattern)
        {
            if (cur >= pattern.Length)
            {
                lock (tree)
                {
                    if (tree.Count > 0)
                    {
                        tree.Count--;
                        return new dotSpace.Objects.Space.Tuple(res);
                    }
                }
                return null;
            }
            if (pattern[cur] is Type)
            {
                foreach (var key in tree.lookupTable.Keys)
                {
                    Type elementType = key is Type ? (Type)key : key.GetType();
                    if (((Type)pattern[cur]).IsAssignableFrom(elementType))
                    {
                        res[cur] = key;
                        var subResult = this.Get(tree[key], ++cur, res, pattern);
                        if (subResult != null)
                        {
                            return subResult;
                        }
                        res[cur] = null;
                    }
                }
                return null;
            }
            else if (tree.lookupTable.ContainsKey(pattern[cur]))
            {
                res[cur] = pattern[cur];
                return this.Get(tree[pattern[cur]], ++cur, res, pattern);
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<ITuple> GetAll(TupleTree tree, int cur, object[] res, object[] pattern)
        {
            if (cur >= pattern.Length)
            {
                lock (tree)
                {
                    var resList = Enumerable.Repeat(new dotSpace.Objects.Space.Tuple(res), tree.Count).ToList();
                    tree.Count = 0;
                    return resList;
                }
            }
            if (pattern[cur] is Type)
            {
                foreach (var key in tree.lookupTable.Keys)
                {
                    var resultList = new List<ITuple>();
                    Type elementType = key is Type ? (Type)key : key.GetType();
                    if (((Type)pattern[cur]).IsAssignableFrom(elementType))
                    {
                        res[cur] = key;
                        var subResult = this.GetAll(tree[key], ++cur, res, pattern);
                        if (subResult != null)
                        {
                            resultList = resultList.Concat(subResult).ToList();
                        }
                        res[cur] = null;
                    }
                }
                return null;
            }
            else if (tree.lookupTable.ContainsKey(pattern[cur]))
            {
                res[cur] = pattern[cur];
                return this.GetAll(tree[pattern[cur]], ++cur, res, pattern);
            }
            else
            {
                return new List<ITuple>();
            }
        }


        public TupleTree this[object key] => this.lookupTable[key];
    }
}