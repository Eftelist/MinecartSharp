using Newtonsoft.Json;

namespace MinecartSharp.Networking.Helpers
{
    public class UUIDMojang
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
    }
}