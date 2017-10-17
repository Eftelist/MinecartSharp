using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class PlayerLook : IPacket
    {
        public int PacketId { get; } = 0x0F;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            float Yaw = buffer.ReadFloat();
            float Pitch = buffer.ReadFloat();
            bool OnGround = buffer.ReadBool();
            state.Player.Yaw = Yaw;
            state.Player.Pitch = Pitch;
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {

        }
    }
}
