using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class Ping : IPacket
    {
        public int PacketId { get; } = 0x01;

        public State State { get; } = State.Status;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            state.SendData(buffer.BufferedData); //Echo the received packet back. :)
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {

        }
    }
}
