using System;
using System.Diagnostics;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class KeepAlive : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x0b;
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
            //TODO: add system to kick client if no keep alive is send for 20 seconds
            Console.WriteLine("client responds at keep alive");
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            Random random = new Random();
            long result = random.Next();
            result = (result << 32);
            result = result | (long)random.Next();

            buffer.WriteVarInt(0x1F);
            buffer.WriteLong(result);
            buffer.FlushData();
        }
    }
}
