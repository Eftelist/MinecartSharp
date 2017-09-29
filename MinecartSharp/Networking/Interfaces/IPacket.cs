using MinecraftSharp.MinecartSharp.Networking.Helpers;
using MinecraftSharp.MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.MinecaftSharp.Networking.Interfaces
{
    public interface IPacket
    {
        int PacketID
        {
            get;
        }

        void Read(ClientWrapper state, MSGBuffer buffer, object[] Arguments);
        void Write(ClientWrapper state, object[] Arguments);
    }
}