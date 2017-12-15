using dotSpace.Enumerations;
using dotSpace.Interfaces.Network;

namespace dotSpace.BaseClasses.Network.Messages
{
    public abstract class ObjectResponseBase<T> : ResponseBase, IObjectMessage<T>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the ObjectBasicResponse class.
        /// </summary>
        public ObjectResponseBase()
        {
        }

        /// <summary>
        /// Initializes a new instances of the ObjectBasicResponse class.
        /// </summary>
        public ObjectResponseBase(ActionType action, string source, string session, string target, StatusCode code, string message) : base(action, source, session, target, code, message)
        {
        }

        #endregion

    }
}