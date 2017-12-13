using dotSpace.BaseClasses.Network.Messages;
using dotSpace.Enumerations;
using dotSpace.Objects.Json;
using System;

namespace dotSpace.Objects.Network.Messages.Requests
{
    /// <summary>
    /// Entity representing a message of a Get request.
    /// </summary>
    public sealed class ObjectGetRequest<T> : RequestBase
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the GetRequest class.
        /// </summary>
        public ObjectGetRequest()
        {
        }

        /// <summary>
        /// Initializes a new instances of the GetRequest class.
        /// </summary>
        public ObjectGetRequest(string source, string session, string target, Func<T, bool> condition) : base(ActionType.OBJECT_GET_REQUEST, source, session, target)
        {
            this.Condition = condition;
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Properties

        /// <summary>
        /// Gets or sets the underlying array of values constituting the template pattern.
        /// </summary>
        public Func<T, bool> Condition { get; set; }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Methods

        /// <summary>
        /// Boxes the message contents from native .NET primitive types into language independent textual representations. 
        /// </summary>
        public override void Box()
        {
            //this.Condition = TypeConverter.Box(this.Condition);
        }

        /// <summary>
        /// Unboxes the message contents from language independent textual representations into native .NET primitive types. 
        /// </summary>
        public override void Unbox()
        {
            //this.Condition = TypeConverter.Unbox(this.Condition);
        }

        #endregion
    }
}
