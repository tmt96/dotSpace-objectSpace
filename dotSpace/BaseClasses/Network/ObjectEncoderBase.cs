using System;
using dotSpace.Interfaces.Network;
using dotSpace.Objects.Network.Json;
using Newtonsoft.Json.Linq;
using static Newtonsoft.Json.JsonConvert;

namespace dotSpace.BaseClasses.Network
{
    public class ObjectEncoderBase : IEncoder
    {
        public IMessage Decode(string msg)
        {
            var deserializedObject = JObject.Parse(msg);
            var type = deserializedObject[JsonConverterWithType.TypeEntry].ToString();
            return DeserializeObject(msg, Type.GetType(type)) as IMessage;
        }

        public T Deserialize<T>(string json, params Type[] types)
        {
            return DeserializeObject<T>(json);
        }

        public string Encode(IMessage message)
        {
            return this.Serialize(message);
        }

        public string Serialize(IMessage message, params Type[] types)
        {
            return SerializeObject(message, new JsonConverterWithType());
        }
    }
}