using System;
using dotSpace.Interfaces.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotSpace.Objects.Network.Json
{
    internal class JsonConverterWithType : JsonConverter
    {
        internal static string TypeEntry = "$MessageType";

        [ThreadStatic]
        static bool disabled;

        // Disables the converter in a thread-safe manner.
        bool Disabled { get { return disabled; } set { disabled = value; } }

        public override bool CanWrite { get { return !Disabled; } }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IMessage).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject o;
            using (new PushValue<bool>(true, () => Disabled, (canWrite) => Disabled = canWrite))
            {
                o = JObject.FromObject(value, serializer);
            }
            o.AddFirst(new JProperty(TypeEntry, value.GetType().FullName));
            o.WriteTo(writer);
        }
    }
}

internal struct PushValue<T> : IDisposable
{
    Func<T> getValue;
    Action<T> setValue;
    T oldValue;

    internal PushValue(T value, Func<T> getValue, Action<T> setValue)
    {
        if (getValue == null || setValue == null)
            throw new ArgumentNullException();
        this.getValue = getValue;
        this.setValue = setValue;
        this.oldValue = getValue();
        setValue(value);
    }

    #region IDisposable Members

    // By using a disposable struct we avoid the overhead of allocating and freeing an instance of a finalizable class.
    public void Dispose()
    {
        if (setValue != null)
            setValue(oldValue);
    }

    #endregion
}
