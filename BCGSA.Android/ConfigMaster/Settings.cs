using Newtonsoft.Json;

namespace BCGSA.Android.ConfigMaster
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
        
        [JsonProperty("inversZ")]
        [JsonConverter(typeof(ParseBoolString))]
        public bool InversZ { get; set; }

        public static Settings FromJson(string json) => JsonConvert.DeserializeObject<Settings>(json, Serialize.Settings);
        public static string GetDefault() => new Settings { ConnectMode = "Game", InversX = false, InversY = false }.ToJson();
    }
}
