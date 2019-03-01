using Newtonsoft.Json;

namespace BCGSA.ConfigMaster
{
    internal sealed class Settings
    {
        [JsonProperty("connectMode")]
        public string ConnectMode { get; set; }

        [JsonProperty("inversX")]
        [JsonConverter(typeof(ParseBoolString))]
        public bool InversX { get; set; }

        [JsonProperty("inversY")]
        [JsonConverter(typeof(ParseBoolString))]
        public bool InversY { get; set; }

        public static Settings FromJson(string json) => JsonConvert.DeserializeObject<Settings>(json, Serialize.Settings);
    }
}
