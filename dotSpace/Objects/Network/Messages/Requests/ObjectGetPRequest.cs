using dotSpace.BaseClasses.Network.Messages;
using dotSpace.Enumerations;
using dotSpace.Objects.Json;
using System;

namespace dotSpace.Objects.Network.Messages.Requests
{
    /// <summary>
    /// Entity representing a message of a GetP request.
    /// </summary>
    public sealed class ObjectGetPRequest<T> : ObjectRequestBase<T>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructorss

        /// <summary>
        /// Initializes a new instances of the GetPRequest class.
        /// </summary>
        public ObjectGetPRequest()
        {
        }

        /// <summary>
        /// Initializes a new instances of the GetPRequest class.
        /// </summary>
        public ObjectGetPRequest( string source, string session, string target, Func<T, bool> condition) : base( ActionType.OBJECT_GETP_REQUEST, source, session, target)
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
