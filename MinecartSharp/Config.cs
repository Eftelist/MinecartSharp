using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinecartSharp
{
    class Config
    {

        public int Port { get; set; } = 25565;
        public string World { get; set; } = "World";
        [JsonProperty("Max-players")]
        public int MaxPlayer { get; set; } = 16;

        public string Motd { get; set; } = "§cA MinecartSharp server";
        public bool Whitelist { get; set; } = false;
        [JsonProperty("Online-mode")]
        public bool OnlineMode { get; set; } = true;

        [JsonProperty("Resource-pack")]
        public string ResourcePack { get; set; } = "";

        public List<String> Ops { get; set; } = new List<string>()
        {
            "TheIndra"
        };

    }
}
