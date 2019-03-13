using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCGSA.ConfigMaster
{
    internal sealed class ParseBoolString: JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (bool.TryParse(value, out bool b))
            {
                return b;
            }
            throw new Exception("Невозможно распаковать тип bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
        }

        public static readonly ParseBoolString Singleton = new ParseBoolString();
    }
}
