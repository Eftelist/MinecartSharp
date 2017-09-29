using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.Utils;
using MinecraftSharp.MinecartSharp.Networking.Helpers;
using MinecraftSharp.MinecartSharp.Networking.Wrappers;
using Newtonsoft.Json;
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
            Program.Logger.Log(LogType.Info, Username);
            string UUID = getUUID(Username);
           
            new LoginSuccess().Write(state, new object[] { Username, UUID });
        }

        private string getUUID(string username)
        {
            UuID UUID = null;
            string uuid = null;
            using (var wc = new System.Net.WebClient())
            {
                string username2 = username.Replace("\0\n", "");
                var json = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username2);
                UUID = JsonConvert.DeserializeObject<UuID>(json);
                uuid = UUID.id;
                Program.Logger.Log(LogType.Info, "New connection from: " + username2 + ", uuid = " + uuid);
            }
            return uuid;
        }

        public void Write(ClientWrapper state, object[] Arguments)
        {

        }
    }

    internal class UuID
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}