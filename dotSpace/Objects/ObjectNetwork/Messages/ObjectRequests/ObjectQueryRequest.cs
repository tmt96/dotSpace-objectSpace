using dotSpace.BaseClasses.Network.Messages;
using dotSpace.Enumerations;
using dotSpace.Objects.Json;
using System;

namespace dotSpace.Objects.Network.Messages.Requests
{
    /// <summary>
    /// Entity representing a message of a Query request.
    /// </summary>
    public sealed class ObjectQueryRequest<T> : ObjectRequestBase<T>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the QueryRequest class.
        /// </summary>
        public ObjectQueryRequest()
        {
        }

        /// <summary>
        /// Initializes a new instances of the QueryRequest class.
        /// </summary>
        public ObjectQueryRequest(string source, string session, string target) : base( ActionType.OBJECT_QUERY_REQUEST, source, session, target)
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
            //this.Template = TypeConverter.Box(this.Template);
        }

        /// <summary>
        /// Unboxes the message contents from language independent textual representations into native .NET primitive types. 
        /// </summary>
        public override void Unbox()
        {
            //this.Template = TypeConverter.Unbox(this.Template);
        }

        #endregion
    }
}
