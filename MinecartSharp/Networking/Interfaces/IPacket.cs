using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.MinecaftSharp.Networking.Interfaces
{
    public interface IPacket
    {
        int PacketID
        {
            get;
        }

        void Read(ClientWrapper state, MSGBuffer buffer, object[] Arguments);
        void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments);


        bool IsPlayePacket
        {
            get;
        }
    }
}