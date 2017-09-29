using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.Utils;
using MinecraftSharp.MinecartSharp.Networking.Helpers;
using MinecraftSharp.MinecartSharp.Networking.Wrappers;
using System.Net;

namespace MinecartSharp.MinecartSharp.Networking.Packets
{
    public class HandshakePacket : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x00;
            }
        }

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            int Protocol = buffer.ReadVarInt();
            string Host = buffer.ReadString();
            short Port = buffer.ReadShort();
            int State = buffer.ReadVarInt();

            switch (State)
            {
                case 1:
                    HandleStatusRequest(state);
                    break;
                case 2:
                    HandleLoginRequest(state, buffer);
                    break;
            }
        }

        private void HandleStatusRequest(ClientWrapper state)
        {
            Program.Logger.Log(LogType.Info, "State test");
            state.MinecraftStream.WriteVarInt(PacketID);
            state.MinecraftStream.WriteString("{\"version\": {\"name\": \"" + Globals.ProtocolName + "\",\"protocol\": " + Globals.ProtocolVersion + "},\"players\": {\"max\": " + Globals.MaxPlayers + ",\"online\": " + Globals.PlayersOnline + "},\"description\": {\"text\":\"" + Globals.ServerMOTD + "\"}}");
            state.MinecraftStream.FlushData();
            Program.Logger.Log(LogType.Warning, Globals.ProtocolName);
        }

        private void HandleLoginRequest(ClientWrapper state, MSGBuffer buffer)
        {
            string Username = buffer.ReadString();
            string UUID = getUUID(Username);

            new LoginSuccess().Write(state, new object[] { Username, UUID });
        }

        private string getUUID(string username)
        {
            WebClient wc = new WebClient();
            string result = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username);
            string[] _result = result.Split('"');

            string UUID = _result[3];
            return UUID;
        }

        public void Write(ClientWrapper state, object[] Arguments)
        {

        }
    }
}