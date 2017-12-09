using System;
using System.Collections.Generic;
using System.Threading;
using dotSpace.Interfaces;

namespace dotSpace.BaseClasses.Space
{
    public abstract class ObjectSpaceAgentBase : IObjectSpace
    {
        protected string name;
        protected IObjectSpace space;

        public ObjectSpaceAgentBase(string name, IObjectSpace space)
        {
            this.name = name;
            this.space = space;   
        }

        /// <summary>
        /// Starts the underlying thread, executing the 'DoWork' method.
        /// </summary>
        public void Start()
        {
            var thread = new Thread(new ThreadStart(this.DoWork));
            thread.Start();
        }

        public T Get<T>()
        {
            return space.Get<T>();
        }

        public T Get<T>(Predicate<T> condition)
        {
            return space.Get<T>(condition);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return space.GetAll<T>();
        }

        public IEnumerable<T> GetAll<T>(Predicate<T> condition)
        {
            return space.GetAll<T>(condition);
        }

        public T GetP<T>()
        {
            return space.GetP<T>();
        }

        public T GetP<T>(Predicate<T> condition)
        {
            return space.GetP<T>(condition);
        }

        public void Put<T>(T element)
        {
            space.Put<T>(element);
        }

        public T Query<T>()
        {
            return space.Query<T>();
        }

        public T Query<T>(Predicate<T> condition)
        {
            return space.Query<T>(condition);
        }

        public IEnumerable<T> QueryAll<T>()
        {
            return space.QueryAll<T>();
        }

        public IEnumerable<T> QueryAll<T>(Predicate<T> condition)
        {
            return space.QueryAll<T>(condition);
        }

        public T QueryP<T>()
        {
            return space.QueryP<T>();
        }

        public T QueryP<T>(Predicate<T> condition)
        {
            return space.QueryP<T>(condition);
        }

        /// <summary>
        /// Method executed when starting the agent.
        /// </summary>
        protected abstract void DoWork();
    }
}