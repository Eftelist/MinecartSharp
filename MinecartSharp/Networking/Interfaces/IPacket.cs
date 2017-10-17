using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Interfaces
{
    public interface IPacket
    {
        int PacketId
        {
            get;
        }

        void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments);
        void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments);


        State State
        {
            get;
        }
    }
}