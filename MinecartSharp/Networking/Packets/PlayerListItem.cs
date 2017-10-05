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
    class PlayerListItem : IPacket
    {
        public int PacketID { get; } = 0x2e;

        public bool IsPlayePacket { get; } = true;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            buffer.WriteVarInt(PacketID);
            buffer.WriteVarInt(0); // action?? idk fix later
            buffer.WriteVarInt(Globals.Players.Count);
            // players array or something? fix later
        }
    }
}
