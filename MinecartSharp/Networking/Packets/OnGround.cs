using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class OnGround : IPacket
    {
        public int PacketId { get; } = 0x0C;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            bool OnGround = buffer.ReadBool();
            state.Player.OnGround = OnGround;
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {

        }
    }
}
