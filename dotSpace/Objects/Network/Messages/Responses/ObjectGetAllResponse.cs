using dotSpace.BaseClasses;
using dotSpace.BaseClasses.Network.Messages;
using dotSpace.Enumerations;
using dotSpace.Interfaces;
using dotSpace.Objects.Json;
using System.Collections.Generic;

namespace dotSpace.Objects.Network.Messages.Responses
{
    /// <summary>
    /// Entity representing a message of a GetAll response.
    /// </summary>
    public sealed class ObjectGetAllResponse<T> : ResponseBase
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the ObjectGetAllResponse class.
        /// </summary>
        public ObjectGetAllResponse()
        {
        }

        /// <summary>
        /// Initializes a new instances of the ObjectGetAllResponse class.
        /// </summary>
        public ObjectGetAllResponse(string source, string session, string target, IEnumerable<T> result, StatusCode code, string message) : base(ActionType.OBJECT_GETALL_RESPONSE, source, session, target, code, message)
        {
            this.Result = result;
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Properties

        /// <summary>
        /// Gets or sets the enumerable set containing the results.
        /// </summary>
        public IEnumerable<T> Result { get; set; }

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
