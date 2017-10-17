using MinecartSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class Handshake : IPacket
    {
        public int PacketID { get; } = 0x00;

        public State State { get; } = State.Unknown;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            var playerstate = state.State;

            int protocol = buffer.ReadVarInt();
            string host = buffer.ReadString();
            short port = buffer.ReadShort();
            int nextstate = buffer.ReadVarInt();

            state.ProtocolVersion = protocol;

            switch (nextstate)
            {
                case 1:
                    state.State = State.Status;
                    break;
                case 2:
                    state.State = State.Login;
                    break;
            }
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            // nothing to write
        }
    }
}
