using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Tasks.Xaml;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using MinecartSharp.Utils;
using Newtonsoft.Json;

namespace MinecartSharp.Networking.Packets
{
    class ChatMessagePacket : IPacket
    {
        public int PacketId { get; } = 0x02;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            var msg = buffer.ReadString();

            switch (msg.StartsWith("/"))
            {
                case true:
                    HandleCommand(state, buffer, msg);
                    break;
                case false:
                    HandleChat(state, buffer, msg);
                    break;
            }
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            buffer.WriteVarInt(0x0F);
            buffer.WriteString((string)arguments[0]);
            buffer.WriteByte((byte)arguments[1]);
            buffer.FlushData();
        }

        private void HandleChat(ClientWrapper state, MSGBuffer buffer, string msg)
        {
            new Logger().Log(LogType.Info, $"<{state.Player.Username}> {msg}");

            var json = JsonConvert.SerializeObject(new ChatMessage()
            {
                Text = $"<{state.Player.Username}> {msg}"
            }, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            foreach (var player in Globals.Players)
            {
                Write(player.Wrapper, player.buffer, new object[] { json, (byte)0 });
            }
        }

        private void HandleCommand(ClientWrapper state, MSGBuffer buffer, string msg)
        {
            // fix later
        }
    }
}
