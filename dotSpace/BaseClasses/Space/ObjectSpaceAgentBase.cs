using System;
using System.Collections.Generic;
using System.Threading;
using dotSpace.Interfaces;

namespace dotSpace.BaseClasses.Space
{
    public abstract class ObjectSpaceAgentBase : ObjectSpaceAgentBaseSimple, IObjectSpace
    {
        protected new IObjectSpace space;

        public ObjectSpaceAgentBase(string name, IObjectSpace space) : base(name, space)
        {
            this.space = space;   
        }

        public T Get<T>(Func<T, bool> condition)
        {
            return space.Get<T>(condition);
        }

        public IEnumerable<T> GetAll<T>(Func<T, bool> condition)
        {
            return space.GetAll<T>(condition);
        }

        public T GetP<T>(Func<T, bool> condition)
        {
            return space.GetP<T>(condition);
        }

        public T Query<T>(Func<T, bool> condition)
        {
            return space.Query<T>(condition);
        }

        public IEnumerable<T> QueryAll<T>(Func<T, bool> condition)
        {
            return space.QueryAll<T>(condition);
        }

        public T QueryP<T>(Func<T, bool> condition)
        {
            return space.QueryP<T>(condition);
        }
    }
}