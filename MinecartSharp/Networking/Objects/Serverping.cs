using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinecartSharp.Networking.Objects
{
    class Serverping
    {
        [JsonProperty("version")] public ServerpingVersion Version;
        [JsonProperty("players")] public ServerpingPlayers Players;
        [JsonProperty("description")] public ServerpingDescription Description;

        [JsonProperty("favicon")] public String Favicon;
    }

    class ServerpingVersion
    {
        [JsonProperty("name")] public String Name;
        [JsonProperty("protocol")] public int Protocol;

    }

    class ServerpingPlayers
    {
        [JsonProperty("max")] public int Max;
        [JsonProperty("online")] public int Online;

        [JsonProperty("sample")] public List<ServerpingPlayer> Players;
    }

    class ServerpingPlayer
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("id")] public string Id;
    }

    class ServerpingDescription
    {
        [JsonProperty("text")] public string Motd;
    }
}
