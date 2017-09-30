using System;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class KeepAlive : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x1F;
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
            int id = new Random().Next(0, 100);
            buffer.WriteVarInt(PacketID);
            buffer.WriteVarInt(id);
            buffer.FlushData();
        }
    }
}
