using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecartSharp.Networking.Wrappers;
using MinecartSharp.MinecartSharp.Objects;
using MinecraftSharp.MinecartSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecartSharp.Networking.Packets
{
    public class PlayerPosition : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x04;
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
            Player targetplayer = state.Player;
            double X = buffer.ReadDouble();
            double FeetY = buffer.ReadDouble();
            double Z = buffer.ReadDouble();
            bool OnGround = buffer.ReadBool();
            targetplayer.Coordinates = new Vector3(X, FeetY, Z);
            state.Player.OnGround = OnGround;
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }
    }
}
