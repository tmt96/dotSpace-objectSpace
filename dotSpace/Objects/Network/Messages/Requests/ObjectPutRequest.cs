using dotSpace.BaseClasses.Network.Messages;
using dotSpace.Enumerations;
using dotSpace.Objects.Json;
using System;

namespace dotSpace.Objects.Network.Messages.Requests
{
    /// <summary>
    /// Entity representing a message of a Put request.
    /// </summary>
    public sealed class ObjectPutRequest<T> : ObjectRequestBase<T>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the PutRequest class.
        /// </summary>
        public ObjectPutRequest()
        {
        }

        /// <summary>
        /// Initializes a new instances of the PutRequest class.
        /// </summary>
        public ObjectPutRequest(string source, string session, string target, T element) : base( ActionType.OBJECT_PUT_REQUEST, source, session, target)
        {
            this.Element = element;
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Properties

        /// <summary>
        /// Gets or sets the underlying array of values constituting the tuple values.
        /// </summary>
        public T Element { get; set; }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Methods

        /// <summary>
        /// Boxes the message contents from native .NET primitive types into language independent textual representations. 
        /// </summary>
        public override void Box()
        {
            //this.Tuple = TypeConverter.Box(this.Tuple);
        }

        /// <summary>
        /// Unboxes the message contents from language independent textual representations into native .NET primitive types. 
        /// </summary>
        public override void Unbox()
        {
            //this.Tuple = TypeConverter.Unbox(this.Tuple);
        }

        #endregion
    }
}
