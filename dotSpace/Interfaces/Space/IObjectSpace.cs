using System;
using System.Collections.Generic;

namespace dotSpace.Interfaces
{
    public interface IObjectSpace
    {
        /// <summary>
        /// Retrieves and removes the first object from the space of type T. The operation will block if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        T Get<T>();

        /// <summary>
        /// Retrieves and removes the first object from the space of type T matching condition. The operation will block if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition</returns>
        T Get<T>(Predicate<T> condition);

        /// <summary>
        /// Retrieves and removes the first object from the space of type T. The operation returns null if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        T GetP<T>();

        /// <summary>
        /// Retrieves and removes the first object from the space of type T matching condition. The operation returns null if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition, or null if no element satisfies</returns>
        T GetP<T>(Predicate<T> condition);

        /// <summary>
        /// Retrieves and removes all objects from the space of type T. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <returns>A collection of all object of type T from the space</returns>
        IEnumerable<T> GetAll<T>();

        /// <summary>
        /// Retrieves and removes all objects from the space of type T matching the condition. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <param name="condition">he condition the object needs to match</param>
        /// <returns>A collection of all object of type T from the space matching the condition</returns>
        IEnumerable<T> GetAll<T>(Predicate<T> condition);


        /// <summary>
        /// Retrieves a clone of the first object from the space of type T. The operation will block if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        T Query<T>();

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T matching condition. The operation will block if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition</returns>
        T Query<T>(Predicate<T> condition);

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T. The operation returns null if no elements match.
        /// </summary>
        /// <returns>An element of type T from the space</returns>
        T QueryP<T>();

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T matching condition. The operation returns null if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition, or null if no element satisfies</returns>
        T QueryP<T>(Predicate<T> condition);

        /// <summary>
        /// Retrieves clones of all objects from the space of type T. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <returns>A collection of all object of type T from the space</returns>
        IEnumerable<T> QueryAll<T>();

        /// <summary>
        /// Retrieves clones of all objects from the space of type T matching the condition. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <param name="condition">he condition the object needs to match</param>
        /// <returns>A collection of all object of type T from the space matching the condition</returns>
        IEnumerable<T> QueryAll<T>(Predicate<T> condition);

        /// <summary>
        /// Inserts an object of type T to the object space.
        /// </summary>
        /// <param name="element">The object ot be inserted to the space</param>
        void Put<T>(T element);
    }
}