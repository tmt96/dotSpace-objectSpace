using dotSpace.Objects.Network.Json;
using Newtonsoft.Json;

namespace dotSpace.Interfaces.Network
{
    /// <summary>
    /// A message correspond to objects of type T.
    /// </summary>
    public interface IObjectMessage<T> : IMessage
    {
    }
}