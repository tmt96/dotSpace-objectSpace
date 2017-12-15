using dotSpace.BaseClasses.Network.Messages;
using dotSpace.Enumerations;
using dotSpace.Objects.Json;
using System;

namespace dotSpace.Objects.Network.Messages.Requests
{
    /// <summary>
    /// Entity representing a message of a QueryP request.
    /// </summary>
    public sealed class ObjectQueryPRequest<T> : ObjectRequestBase<T>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the QueryPRequest class.
        /// </summary>
        public ObjectQueryPRequest()
        {
        }

        /// <summary>
        /// Initializes a new instances of the QueryPRequest class.
        /// </summary>
        public ObjectQueryPRequest(string source, string session, string target) : base(ActionType.OBJECT_QUERYP_REQUEST, source, session, target)
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
