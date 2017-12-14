using dotSpace.BaseClasses.Network.Messages;
using dotSpace.Enumerations;
using dotSpace.Objects.Json;
using System;

namespace dotSpace.Objects.Network.Messages.Requests
{
    /// <summary>
    /// Entity representing a message of a GetAll request.
    /// </summary>
    public sealed class ObjectGetAllRequest<T> : ObjectRequestBase<T>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the GetAllRequest class.
        /// </summary>
        public ObjectGetAllRequest()
        {
        }

        /// <summary>
        /// Initializes a new instances of the GetAllRequest class.
        /// </summary>
        public ObjectGetAllRequest(string source, string session, string target) : base(ActionType.OBJECT_GETALL_REQUEST, source, session, target)
        {
        }

        #endregion


        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Methods

        /// <summary>
        /// Boxes the message contents from native .NET primitive types into language independent textual representations. 
        /// </summary>
        public override void Box()
        {
        }

        /// <summary>
        /// Unboxes the message contents from language independent textual representations into native .NET primitive types. 
        /// </summary>
        public override void Unbox()
        {
        }

        #endregion
    }
}
