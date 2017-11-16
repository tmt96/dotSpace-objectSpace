﻿using dotSpace.Interfaces;
using dotSpace.Interfaces.Network;
using System;
using static Newtonsoft.Json.JsonConvert;

namespace dotSpace.BaseClasses.Network
{
    /// <summary>
    /// Provides basic functionality for serializing and deserializing json objects. This is an abstract class.
    /// </summary>
    public abstract class EncoderBase : IEncoder
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Methods

        /// <summary>
        /// Composes and returns a new object based on the provided json string.
        /// </summary>
        public T Deserialize<T>(string json, params Type[] types)
        {
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// Decomposes the passed object into a json string.
        /// </summary>
        public string Serialize(IMessage message, params Type[] types)
        {
            return SerializeObject(message);
        }

        /// <summary>
        /// Boxes and serializes the passed message into interoperable types specified as a json string.
        /// </summary>
        public string Encode(IMessage message)
        {
            message.Box();
            return this.Serialize(message);
        }

        /// <summary>
        /// Template method for deserializing and unboxing the interoperable types specified in json into valid .NET primitive types.
        /// </summary>
        public abstract IMessage Decode(string msg);


        #endregion
    }
}
