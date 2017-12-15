using dotSpace.Interfaces.Space;
using System.Collections.Generic;
using System;

namespace dotSpace.Interfaces.Network
{
    /// <summary>
    /// Defines the methods that allow operations on multiple object spaces.
    /// These methods are used for supporting networked object spaces.
    /// </summary>
    public interface IObjectRepository : IObjectRepositorySimple
    {
        /// <summary>
        /// Adds a new Space to the repository, identified by the specified parameter.
        /// </summary>
        void AddSpace(string identifier, IObjectSpace objectspace);
        /// <summary>
        /// Retrieves and removes the first obejct of type T from the target Space, matching the specified condition. The operation will block if no elements match.
        /// </summary>
        T Get<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves and removes the first object from the target Space, matching the specified condition. The operation is non-blocking. The operation will return null if no elements match.
        /// </summary>
        T GetP<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves and removes all objects from the target Space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        IEnumerable<T> GetAll<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves the first object from the target Space, matching the specified condition. The operation will block if no elements match.
        /// </summary>
        T Query<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves the first object from the target Space, matching the specified condition. The operation is non-blocking. The operation will return null if no elements match.
        /// </summary>
        T QueryP<T>(string target, Func<T, bool> condition);
        /// <summary>
        /// Retrieves all objects from the target Space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        IEnumerable<T> QueryAll<T>(string target, Func<T, bool> condition);
    }
}
