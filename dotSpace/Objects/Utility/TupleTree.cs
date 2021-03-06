using System;
using System.Collections.Generic;
using System.Linq;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Space;

namespace dotSpace.Objects.Utility
{
    /// <summary>
    /// A hierarchy structure storing tuples
    /// Each tuple is represented as a branch
    /// the end of a tuple is denoted by a counter of
    /// identical tuples in the space
    /// </summary>
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

        /// <summary>
        /// Add a tuple to the tree
        /// </summary>
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

        /// <summary>
        /// Get a tuple of the specify pattern from the tree
        /// </summary>
        public ITuple Query(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.Query(TupleHandleOption.KEEP, 0, res, pattern);
        }

        /// <summary>
        /// Get all tuples of the specify pattern from the tree
        /// </summary>
        public IEnumerable<ITuple> QueryAll(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.QueryAll(TupleHandleOption.KEEP, 0, res, pattern);
        }

        /// <summary>
        /// Remove a tuple of the specify pattern from the tree
        /// </summary>
        /// <returns>The tuple removed from the tree</returns>
        public ITuple Get(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.Query(TupleHandleOption.REMOVE, 0, res, pattern);
        }

        /// <summary>
        /// Remove all tuples of the specify pattern from the tree
        /// </summary>
        /// <returns>The collection of tuples removed from the tree</returns>
        public IEnumerable<ITuple> GetAll(params object[] pattern)
        {
            var res = new List<object>(pattern.Length);
            return this.QueryAll(TupleHandleOption.REMOVE, 0, res, pattern);
        }

        /// <summary>
        /// Recursively query for tuple of the specify pattern
        /// </summary>
        /// <param name="option">Whether to keep the tuple or not</param>
        /// <param name="cur">current index in the pattern</param>
        /// <param name="res">current result tuple</param>
        /// <param name="pattern">the pattern we need to match</param>
        /// <returns>The tuple matching the pattern</returns>
        private ITuple Query(TupleHandleOption option, int cur, List<object> res, object[] pattern)
        {
            #region LocalFunctions
            // supports recursive querying
            ITuple RecursiveQuery(object element)
            {
                res.Add(element);
                var resultTuple = this[element].Query(option, cur + 1, res, pattern);
                res.RemoveAt(res.Count - 1);
                return resultTuple;
            }
            #endregion

            // return result if done
            if (cur >= pattern.Length)
            {
                if (Count <= 0)
                {
                    return null;
                }
                if (option == TupleHandleOption.REMOVE)
                {
                    Count--;
                }
                return new Space.Tuple(res.ToArray());
            }

            ITuple tuple = null;

            if (pattern[cur] is Type)
            {
                // recursively search all children matching the specified type
                tuple = lookupTable.Select(item => item.Key)
                                   .Where(key => MatchedType((Type)pattern[cur], key))
                                   .Select(RecursiveQuery)
                                   .Where(t => t != null)
                                   .FirstOrDefault();
            }
            else if (lookupTable.ContainsKey(pattern[cur]))
            {
                tuple = RecursiveQuery(pattern[cur]);
            }

            if (option == TupleHandleOption.REMOVE && tuple != null)
            {
                RemoveEmptyChild(tuple[cur]);
            }
            return tuple;
        }

        private List<Space.Tuple> QueryAll(TupleHandleOption option, int cur, List<object> res, object[] pattern)
        {
            #region LocalFunctions
            // supports recursive querying
            List<Space.Tuple> RecursiveQueryAll(object element)
            {
                res.Add(element);
                var allTuples = this[element].QueryAll(option, cur + 1, res, pattern);
                res.RemoveAt(res.Count - 1);
                return allTuples;
            }
            #endregion
            
            // return result if done
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
                // recursively search all children matching the specified type
                foreach (var key in 
                            lookupTable.Select(item => item.Key)
                                       .Where(key => MatchedType((Type)pattern[cur], key)))
                {
                    var subResult = RecursiveQueryAll(key);
                    if (subResult.Count != 0)
                    {
                        matchedKeysList.Add(key);
                        resList.AddRange(subResult);
                    }
                }
            }
            else if (lookupTable.ContainsKey(pattern[cur]))
            {
                resList = RecursiveQueryAll(pattern[cur]);
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