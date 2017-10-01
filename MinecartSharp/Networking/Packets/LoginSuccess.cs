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
            string uuid = Arguments[0].ToString();
            string user = Arguments[1].ToString();
            buffer.WriteString(uuid);
            buffer.WriteString(user);
            Globals.Logger.Log(Utils.LogType.Error, "uuidandshit: " + uuid + " : " +user);
            buffer.FlushData();
        }
    }
}
