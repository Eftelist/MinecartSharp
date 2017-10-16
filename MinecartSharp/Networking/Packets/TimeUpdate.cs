using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class TimeUpdate : IPacket
    {
        public int PacketID { get; } = 0x47;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments) { }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            buffer.WriteVarInt(PacketID);
            buffer.WriteLong(Globals.WorldAge);
            buffer.WriteLong(Globals.TimeOfDay);
            buffer.FlushData();
        }
    }
}
