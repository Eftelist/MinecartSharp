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
    public class JoinGame : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x23;
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
            buffer.WriteInt(state.Player.UniqueServerID);
            buffer.WriteByte((byte)state.Player.Gamemode);
            buffer.WriteByte((byte)state.Player.Dimension);
            buffer.WriteByte((byte)Globals.Difficulty);
            buffer.WriteByte((byte)Globals.MaxPlayers);
            buffer.WriteString(Globals.LVLType);
            buffer.WriteBool(false);
            buffer.FlushData();
        }
    }
}
