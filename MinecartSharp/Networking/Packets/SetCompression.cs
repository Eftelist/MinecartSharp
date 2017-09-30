using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class SetCompression : IPacket
        {
            public int PacketID
            {
                get
                {
                    return 0x03;
                }
            }

            public bool IsPlayePacket
            {
                get
                {
                    return true;
                }
            }

            public void Read(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
            {

            }

            public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
            {
                buffer.WriteVarInt(PacketID);
                buffer.WriteVarInt((int)Arguments[0]);
                buffer.FlushData();
            }
        }
}