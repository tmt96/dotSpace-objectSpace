using System;
using dotSpace.Interfaces.Network;
using dotSpace.Objects.Network.Json;
using Newtonsoft.Json.Linq;
using static Newtonsoft.Json.JsonConvert;

namespace dotSpace.BaseClasses.Network
{
    /// <summary>
    /// Provides basic functionality for json serializing and deserializing of ObjectMessage. This is an abstract class.
    /// </summary>
    public class ObjectEncoderBase : IEncoder
    {
        /// <summary>
        /// Template method for deserializing an ObjectMessage.
        /// </summary>
        public IMessage Decode(string msg)
        {
            var deserializedObject = JObject.Parse(msg);
            var type = deserializedObject[JsonConverterWithType.TypeEntry].ToString();
            return DeserializeObject(msg, Type.GetType(type)) as IMessage;
        }

        /// <summary>
        /// Composes and returns a new object based on the provided json string.
        /// </summary>
        public T Deserialize<T>(string json, params Type[] types)
        {
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// Serializes the passed message into interoperable types specified as a json string.
        /// </summary>
        public string Encode(IMessage message)
        {
            return this.Serialize(message);
        }

        /// <summary>
        /// Decomposes the passed object into a json string.
        /// </summary>
        public string Serialize(IMessage message, params Type[] types)
        {
            return SerializeObject(message, new JsonConverterWithType());
        }
    }
}