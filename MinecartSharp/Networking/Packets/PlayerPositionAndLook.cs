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
    public class PlayerPositionAndLook : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x08;
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
            buffer.WriteDouble(Globals.WorldGen.GetSpawnPoint().X);
            buffer.WriteDouble(Globals.WorldGen.GetSpawnPoint().Y);
            buffer.WriteDouble(Globals.WorldGen.GetSpawnPoint().Z);
            buffer.WriteFloat(0f);
            buffer.WriteFloat(0f);
            buffer.WriteByte(111);
            buffer.FlushData();
        }
    }
}
