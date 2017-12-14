using System;
using System.Collections.Generic;

namespace dotSpace.Interfaces
{
    public interface IObjectSpace : IObjectSpaceSimple
    {
        /// <summary>
        /// Retrieves and removes the first object from the space of type T matching condition. The operation will block if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition</returns>
        T Get<T>(Func<T, bool> condition);

        /// <summary>
        /// Retrieves and removes the first object from the space of type T matching condition. The operation returns null if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition, or null if no element satisfies</returns>
        T GetP<T>(Func<T, bool> condition);

        /// <summary>
        /// Retrieves and removes all objects from the space of type T matching the condition. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <param name="condition">he condition the object needs to match</param>
        /// <returns>A collection of all object of type T from the space matching the condition</returns>
        IEnumerable<T> GetAll<T>(Func<T, bool> condition);

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T matching condition. The operation will block if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition</returns>
        T Query<T>(Func<T, bool> condition);

        /// <summary>
        /// Retrieves a clone of the first object from the space of type T matching condition. The operation returns null if no elements match.
        /// </summary>
        /// <param name="condition">The condition the object needs to match</param>
        /// <returns>An element of type T from the space matching the condition, or null if no element satisfies</returns>
        T QueryP<T>(Func<T, bool> condition);

        /// <summary>
        /// Retrieves clones of all objects from the space of type T matching the condition. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        /// <param name="condition">he condition the object needs to match</param>
        /// <returns>A collection of all object of type T from the space matching the condition</returns>
        IEnumerable<T> QueryAll<T>(Func<T, bool> condition);
    }
}