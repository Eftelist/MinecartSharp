using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Interfaces
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