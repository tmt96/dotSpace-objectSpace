using dotSpace.Interfaces.Space;
using System.Collections.Generic;
using System;

namespace dotSpace.Interfaces.Network
{
    /// <summary>
    /// Defines the methods that allow operations on multiple tuple spaces.
    /// These methods are used for supporting networked tuple spaces.
    /// </summary>
    public interface IObjectRepository
    {
        /// <summary>
        /// Adds a new Gate to the repository based on the provided connectionstring.
        /// </summary>
        void AddGate(string connectionstring);
        /// <summary>
        /// Adds a new Space to the repository, identified by the specified parameter.
        /// </summary>
        void AddSpace(string identifier, IObjectSpace tuplespace);
        /// <summary>
        /// Returns the local instance of the space identified by the parameter.
        /// </summary>
        IObjectSpace GetSpace(string target);
        /// <summary>
        /// Retrieves and removes the first tuple from the target Space, matching the specified pattern. The operation will block if no elements match.
        /// </summary>
        T Get<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves and removes the first tuple from the target Space, matching the specified pattern. The operation is non-blocking. The operation will return null if no elements match.
        /// </summary>
        T GetP<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves and removes all tuples from the target Space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        IEnumerable<T> GetAll<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves the first tuple from the target Space, matching the specified pattern. The operation will block if no elements match.
        /// </summary>
        T Query<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves the first tuple from the target Space, matching the specified pattern. The operation is non-blocking. The operation will return null if no elements match.
        /// </summary>
        T QueryP<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves all tuples from the target Space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        IEnumerable<T> QueryAll<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Inserts the tuple passed as argument into the target Space.
        /// </summary>
        void Put<T>(string target, T element);
    }
}
