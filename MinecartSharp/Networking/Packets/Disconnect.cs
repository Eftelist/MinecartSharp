using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using Newtonsoft.Json;

namespace MinecartSharp.Networking.Packets
{
    class Disconnect : IPacket
    {
        public int PacketID { get; } = 0x40;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments) { }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            if (state.State == State.Play)
            {
                buffer.WriteVarInt(PacketID);
            }
            else
            {
                buffer.WriteVarInt(0x00);
            }

            buffer.WriteString(JsonConvert.SerializeObject((ChatMessage)arguments[0], new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            buffer.FlushData();
        }
    }
}
