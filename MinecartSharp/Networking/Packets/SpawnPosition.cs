using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using MinecartSharp.Objects;

namespace MinecartSharp.Networking.Packets
{
    public class SpawnPosition : IPacket
    {
        public int PacketID { get; } = 0x46;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {

        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            Vector3 D = new Vector3(1, 1, 1);
            long Data = (((long)D.X & 0x3FFFFFF) << 38) | (((long)D.Y & 0xFFF) << 26) | ((long)D.Z & 0x3FFFFFF);
            buffer.WriteVarInt(PacketID);
            buffer.WriteLong(Data);
            buffer.FlushData();
        }
    }
}
