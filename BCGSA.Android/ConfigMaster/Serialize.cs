using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BCGSA.Android.ConfigMaster
{
    internal static class Serialize
    {
        public static string ToJson(this Settings self) => 
            JsonConvert.SerializeObject(self, Settings);

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
