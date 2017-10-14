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

        public Dictionary<string, bool> GameRules { get; set; } = new Dictionary<string, bool>()
        {
            {"DoDaylightCycle", false }
        };

        [JsonProperty("Player-inventory")]
        public Dictionary<int, int> PlayerInventory { get; set; } = new Dictionary<int, int>()
        {
            {399, 4}
        };

        public List<String> Ops { get; set; } = new List<string>()
        {
            "TheIndra"
        };

        public bool GetGamerule(string name)
        {
            if (!GameRules.ContainsKey(name))
                return false;

            return GameRules[name];
        }

    }
}
