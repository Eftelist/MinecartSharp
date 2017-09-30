using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.MinecartSharp.Networking.Packets
{
    public class ClientSettings : IPacket
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
            string Locale = buffer.ReadString();
            byte ViewDistance = (byte)buffer.ReadByte();
            byte ChatFlags = (byte)buffer.ReadByte();
            bool ChatColours = buffer.ReadBool();
            byte SkinParts = (byte)buffer.ReadByte();

            state.Player.Locale = Locale;
            state.Player.ViewDistance = ViewDistance;
            state.Player.ChatColours = ChatColours;
            state.Player.ChatFlags = ChatFlags;
            state.Player.SkinParts = SkinParts;
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }
    }
}
