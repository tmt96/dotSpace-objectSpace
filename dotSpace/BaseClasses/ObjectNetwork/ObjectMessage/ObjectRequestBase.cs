using dotSpace.Enumerations;
using dotSpace.Interfaces.Network;

namespace dotSpace.BaseClasses.Network.Messages
{
    public abstract class ObjectRequestBase<T> : RequestBase, IObjectMessage<T>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the ObjectBasicRequest class.
        /// </summary>
        public ObjectRequestBase()
        {
        }

        /// <summary>
        /// Initializes a new instances of the ObjectBasicRequest class.
        /// </summary>
        public ObjectRequestBase(ActionType action, string source, string session, string target) : base(action, source, session, target)
        {
        }

        #endregion

    }
}