using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecartSharp.Networking.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecartSharp.Networking.Packets
{
    public class PlayerLook : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x05;
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
            float Yaw = buffer.ReadFloat();
            float Pitch = buffer.ReadFloat();
            bool OnGround = buffer.ReadBool();
            state.Player.Yaw = Yaw;
            state.Player.Pitch = Pitch;
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }
    }
}
