using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class ClientSettings : IPacket
    {
        public int PacketId { get; } = 0x04;

        public State State { get; } = State.Play;
        
        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
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

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {

        }
    }
}
