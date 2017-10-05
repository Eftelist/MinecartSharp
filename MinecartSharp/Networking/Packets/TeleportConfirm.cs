using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    class TeleportConfirm : IPacket
    {
        public int PacketID { get; } = 0x00;

        public bool IsPlayePacket { get; } = true;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            int id = buffer.ReadVarInt();
            Console.WriteLine(id);
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            
        }
    }
}
