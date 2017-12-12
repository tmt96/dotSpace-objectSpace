using System;
using System.Collections.Generic;

namespace dotSpace.Interfaces
{
    public interface IObjectSpaceSimple
    {
        /// <summary>
        /// Retrieves and removes the first object from the space of type T. The operation will block if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        T Get<T>();

        /// <summary>
        /// Retrieves and removes the first object from the space of type T. The operation returns null if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        T GetP<T>();

        /// <summary>
        /// Retrieves and removes all objects from the space of type T. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <returns>A collection of all object of type T from the space</returns>
        IEnumerable<T> GetAll<T>();

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T. The operation will block if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        T Query<T>();

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T. The operation returns null if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        T QueryP<T>();

        /// <summary>
        /// Retrieves clones of all objects from the space of type T. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <returns>A collection of all object of type T from the space</returns>
        IEnumerable<T> QueryAll<T>();

        /// <summary>
        /// Inserts an object of type T to the object space.
        /// </summary>
        /// <param name="element">The object ot be inserted to the space</param>
        void Put<T>(T element);
    }
}
