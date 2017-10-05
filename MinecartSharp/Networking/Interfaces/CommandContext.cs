using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Packets;
using MinecartSharp.Networking.Wrappers;
using Newtonsoft.Json;

namespace MinecartSharp.Networking.Interfaces
{
    class CommandContext
    {
        public string RawCommandString;

        public String[] Parameters;

        public MSGBuffer Buffer;
        public ClientWrapper State;

        public void SendMessage(string message)
        {
            ChatMessage json = new ChatMessage()
            {
                Text = message
            };

            SendRawMessage(json);
        }

        public void SendRawMessage(ChatMessage message)
        {
            new ChatMessagePacket().Write(State, Buffer, new object[]
            {
                JsonConvert.SerializeObject((ChatMessage)message),
                0
            });
        }

    }
}
