using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class LoginSuccess : IPacket
    {
        public int PacketID { get; } = 0x02;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
           
            buffer.WriteVarInt(PacketID);
            buffer.WriteString(arguments[0].ToString());
            buffer.WriteString(arguments[1].ToString());
            buffer.FlushData();
        }
    }
}
