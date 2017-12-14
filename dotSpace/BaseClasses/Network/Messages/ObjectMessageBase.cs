using dotSpace.Enumerations;
using dotSpace.Interfaces.Network;

namespace dotSpace.BaseClasses.Network.Messages
{
    public abstract class ObjectMessageBase<T> : MessageBase, IObjectMessage<T>
    {
        public ObjectMessageBase()
        {
        }

        public ObjectMessageBase(ActionType action, string source, string session, string target) : base(action, source, session, target)
        {
        }
    }
}