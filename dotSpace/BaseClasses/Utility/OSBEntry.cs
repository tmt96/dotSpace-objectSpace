using System;
using System.Collections.Generic;

namespace dotSpace.BaseClasses.Utility
{
    abstract class OSBEntry
    {
        protected internal OSBTypeCollection SubtypeCollection { get; protected set; }

        protected internal OSBTypeCollection SupertypeCollection { get; protected set; }

        internal void AddSubtype(Type t) => SubtypeCollection.AddType(t);

        internal void AddSupertype(Type t) => SupertypeCollection.AddType(t);

        internal abstract void Add(object item);

        internal abstract void Add(object item, int index);

        internal abstract T Get<T>(Func<T, bool> condition);

        internal abstract IEnumerable<T> GetAll<T>(Func<T, bool> condition);

        internal abstract T Remove<T>(Func<T, bool> condition);

        internal abstract IEnumerable<T> RemoveAll<T>(Func<T, bool> condition);
    }


    class OSBEntry<T> : OSBEntry
    {
        internal OSBObjectCollection<T> ObjectCollection { get; }
        internal int Size  => ObjectCollection.Size;

        internal OSBEntry(OSBObjectCollection<T> objectCollection = null, OSBTypeCollection subtypeCollection = null, OSBTypeCollection supertypeCollection = null)
        {
            this.ObjectCollection = objectCollection ?? new OSBObjectCollection<T>();
            SubtypeCollection = subtypeCollection ?? new OSBTypeCollection();
            SupertypeCollection = supertypeCollection ?? new OSBTypeCollection();
        }

        internal void Add(T item)
        {
            ObjectCollection.Add(item);
        }

        internal void Add(T item, int index)
        {
            ObjectCollection.Add(item, index);
        }

        internal T Get(Func<T, bool> condition)
        {
            return ObjectCollection.Get(condition);
        }

        internal IEnumerable<T> GetAll(Func<T, bool> condition)
        {
            return ObjectCollection.GetAll(condition);
        }

        internal T Remove(Func<T, bool> condition)
        {
            return ObjectCollection.Remove(condition);
        }

        internal IEnumerable<T> RemoveAll(Func<T, bool> condition)
        {
            return ObjectCollection.RemoveAll(condition);
        }

        internal override void Add(object item)
        {
            if (item is T)
            {
                Add((T) item);
            }
            else
            {
                throw new ArgumentException("The value \"" + item + "\" is not of type \"" + typeof(T) + "\" and cannot be used.");
            }
        }

        internal override void Add(object item, int index)
        {
            if (item is T)
            {
                Add((T)item, index);
            }
            else
            {
                throw new ArgumentException("The value \"" + item + "\" is not of type \"" + typeof(T) + "\" and cannot be used.");
            }
        }

        internal override T1 Get<T1>(Func<T1, bool> condition)
        {
            if (typeof(T1).IsAssignableFrom(typeof(T)))
            {
                return (T1) (object) Get(condition as Func<T, bool>);
            }
            else
            {
                throw new ArgumentException("The value \"" + condition + "\" is not of type \"" + typeof(Func<T, bool>) + "\" and cannot be used.");
            }
        }

        internal override IEnumerable<T1> GetAll<T1>(Func<T1, bool> condition)
        {
            if (typeof(T1).IsAssignableFrom(typeof(T)))
            {
                return (IEnumerable<T1>) GetAll(condition as Func<T, bool>);
            }
            else
            {
                throw new ArgumentException("The value \"" + condition + "\" is not of type \"" + typeof(Func<T, bool>) + "\" and cannot be used.");
            }
        }

        internal override T1 Remove<T1>(Func<T1, bool> condition)
        {
            if (typeof(T1).IsAssignableFrom(typeof(T)))
            {
                return (T1)(object)Remove(condition as Func<T, bool>);
            }
            else
            {
                Console.WriteLine("Oops");
                throw new ArgumentException("The value \"" + condition + "\" is not of type \"" + typeof(Func<T, bool>) + "\" and cannot be used.");
            }
        }

        internal override IEnumerable<T1> RemoveAll<T1>(Func<T1, bool> condition)
        {
            if (typeof(T1).IsAssignableFrom(typeof(T)))
            {
                return (IEnumerable<T1>)RemoveAll(condition as Func<T, bool>);
            }
            else
            {
                throw new ArgumentException("The value \"" + condition + "\" is not of type \"" + typeof(Func<T, bool>) + "\" and cannot be used.");
            }
        }
    }
}