using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MinecartSharp.Networking.Objects
{
    class ChatMessage
    {
        [JsonProperty("text")] public string Text;

        [JsonProperty("bold")] public bool Bold;
        [JsonProperty("obfuscated")] public bool Obfuscated;
        [JsonProperty("strikethrough")] public bool Strikethrough;
        [JsonProperty("underlineD")] public bool Underline;
        [JsonProperty("italic")] public bool Italic;

        [JsonProperty("color")] public string Color;

        [JsonProperty("extra")] public List<ExtraText> Extra;

        //TODO: add clickevents, hoverevents etc
    }

    class ExtraText
    {
        [JsonProperty("text")] public string Text;

        [JsonProperty("bold")] public bool Bold;
        [JsonProperty("obfuscated")] public bool Obfuscated;
        [JsonProperty("strikethrough")] public bool Strikethrough;
        [JsonProperty("underlineD")] public bool Underline;
        [JsonProperty("italic")] public bool Italic;

        [JsonProperty("color")] public string Color;
    }
}
