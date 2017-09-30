using Newtonsoft.Json;

namespace MinecaftSharp.Networking.Helpers
{
    public class UUIDMojang
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
    }
}