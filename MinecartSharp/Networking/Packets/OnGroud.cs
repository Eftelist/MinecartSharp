using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.MinecartSharp.Networking.Packets
{
    public class OnGround : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x0C;
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
            bool OnGround = buffer.ReadBool();
            state.Player.OnGround = OnGround;
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }
    }
}
