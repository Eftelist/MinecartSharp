using System;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class JoinGame : IPacket
    {
        public int PacketID { get; } = 0x23;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments) { }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            buffer.WriteVarInt(PacketID);
            buffer.WriteInt(state.Player.UniqueServerID);
            buffer.WriteByte((byte)state.Player.Gamemode);
            buffer.WriteInt(state.Player.Dimension);
            buffer.WriteByte((byte)Globals.Difficulty);
            buffer.WriteByte((byte)Globals.MaxPlayers);
            buffer.WriteString("flat");
            buffer.WriteBool(false);
            buffer.FlushData();
        }
    }
}
