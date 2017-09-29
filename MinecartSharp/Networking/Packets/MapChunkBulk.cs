using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking;
using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecartSharp.Networking.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecartSharp.Networking.Packets
{
    public class MapChunkBulk : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x26;
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
            buffer.WriteBool(true);
            buffer.WriteVarInt(49);
            for (int i = 0; i < 49; i++)
            {
                buffer.Write(Globals.ChunkColums[i].GetBytes());
            }
            buffer.FlushData();
        }
    }
}
