using Newtonsoft.Json;

namespace MinecartSharp.Networking.Helpers
{
    public class UUIDMojang
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}