using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class TimeUpdate : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x47;
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
            buffer.WriteLong(Globals.WorldAge);
            buffer.WriteLong(Globals.TimeOfDay);
            buffer.FlushData();
        }
    }
}
