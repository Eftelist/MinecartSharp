using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace MinecartSharp
{
    class Config
    {

        public int Port { get; set; } = 25565;
        public string World { get; set; } = "World";
        [JsonProperty("Max-players")]
        public int MaxPlayer { get; set; } = 16;

        public string Motd { get; set; }
        [JsonProperty("Online-mode")]
        public bool Whitelist { get; set; } = false;
        public bool OnlineMode { get; set; } = true;

        [JsonProperty("Resource-pack")]
        public string ResourcePack { get; set; } = "";

        public List<String> Ops { get; set; } = new List<string>()
        {
            "TheIndra"
        };

    }
}
