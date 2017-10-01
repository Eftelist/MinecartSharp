using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class LoginSuccess : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x02;
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
            buffer.WriteString(Arguments[0].ToString());
            buffer.WriteString(Arguments[1].ToString());
            buffer.FlushData();
        }
    }
}
