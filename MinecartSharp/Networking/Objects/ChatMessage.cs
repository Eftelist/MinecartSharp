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
        [JsonProperty("text")]
        public string Text;

        //TODO: add bold, underlined and extra array http://wiki.vg/Chat#Inheritance

    }
}
