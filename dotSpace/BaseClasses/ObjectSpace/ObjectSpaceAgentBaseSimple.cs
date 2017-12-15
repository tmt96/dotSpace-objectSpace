using System;
using System.Collections.Generic;
using System.Threading;
using dotSpace.Interfaces;

namespace dotSpace.BaseClasses.Space
{
    public abstract class ObjectSpaceAgentBaseSimple : IObjectSpaceSimple
    {
        protected string name;
        protected IObjectSpaceSimple space;

        public ObjectSpaceAgentBaseSimple(string name, IObjectSpaceSimple space)
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

        public IEnumerable<T> GetAll<T>()
        {
            return space.GetAll<T>();
        }

        public T GetP<T>()
        {
            return space.GetP<T>();
        }

        public void Put<T>(T element)
        {
            space.Put<T>(element);
        }

        public T Query<T>()
        {
            return space.Query<T>();
        }

        public IEnumerable<T> QueryAll<T>()
        {
            return space.QueryAll<T>();
        }

        public T QueryP<T>()
        {
            return space.QueryP<T>();
        }

        /// <summary>
        /// Method executed when starting the agent.
        /// </summary>
        protected abstract void DoWork();
    }
}
