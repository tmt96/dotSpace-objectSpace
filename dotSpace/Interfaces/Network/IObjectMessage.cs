using dotSpace.Objects.Network.Json;
using Newtonsoft.Json;

namespace dotSpace.Interfaces.Network
{
    [JsonConverter(typeof(JsonConverterWithType))]
    public interface IObjectMessage<T> : IMessage
    {
        
    }
}