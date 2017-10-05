using System;
using System.Diagnostics;
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
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            // use the program uptime as keep alive id
            double id = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalMilliseconds;

            buffer.WriteVarInt(0x1F);
            buffer.WriteLong(Convert.ToInt64(id));
            buffer.FlushData();
        }
    }
}
