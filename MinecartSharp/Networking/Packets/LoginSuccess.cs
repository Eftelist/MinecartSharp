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
            Globals.Logger.Log(Utils.LogType.Error, "" + Arguments[0].ToString() + " : " +Arguments[1].ToString());
            buffer.WriteVarInt(PacketID);
            buffer.WriteString((string)Arguments[0]);
            buffer.WriteString((string)Arguments[1]);
            buffer.FlushData();
        }
    }
}
