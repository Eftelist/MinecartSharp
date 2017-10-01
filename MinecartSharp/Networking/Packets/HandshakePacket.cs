using MinecartSharp.Utils;
using System;
using System.Net;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using MinecartSharp.Objects;
using Newtonsoft.Json;

namespace MinecartSharp.Networking.Packets
{
    public class Handshake : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x00;
            }
        }

        public bool IsPlayePacket
        {
            get
            {
                return false;
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
                    HandleStatusRequest(state, buffer);
                    break;
                case 2:
                    HandleLoginRequest(state, buffer);
                    break;
            }
        }

        private void HandleStatusRequest(ClientWrapper state, MSGBuffer buffer)
        {
            buffer.WriteVarInt(PacketID);

            Serverping serverlistping = new Serverping()
            {
                Version = new ServerpingVersion()
                {
                    Name = Globals.ProtocolName,
                    Protocol = Globals.ProtocolVersion
                },
                Players = new ServerpingPlayers()
                {
                    Max = Globals.MaxPlayers,
                    Online = Globals.PlayersOnline,
                },
                Description = new ServerpingDescription()
                {
                    Motd = Globals.Config.Motd
                },
                Favicon = (Globals.Favicon == null) ? "" : Globals.Favicon
        };

            buffer.WriteString(JsonConvert.SerializeObject(serverlistping));
            buffer.FlushData();
        }

        private void HandleLoginRequest(ClientWrapper state, MSGBuffer buffer)
        {
            string username = buffer.ReadUsername();
            string UUID = getUUID(username);
            Program.Logger.Log(LogType.Warning, "Logging in : " + UUID);
            try
            {
                new LoginSuccess().Write(state, buffer, new object[] { UUID, username });
            } catch(Exception e)
            {
                Globals.Logger.Log(LogType.Error, e.Message);
            }
            Globals.LastUniqueID++;
            state.Player = new Player() { UUID = UUID, Username = username, UniqueServerID = Globals.LastUniqueID, Wrapper = state, Gamemode = Gamemode.Creative, Dimension = 0};
            state.PlayMode = true; //Toggle the boolean to PlayMode so we know we are not handling Status stuff anymore.

            if (!Globals.UseCompression)
                new SetCompression().Write(state, buffer, new object[] { -1 }); //Turn off compression.

            new JoinGame().Write(state, buffer, new object[] { state.Player });
            //  new MapChunkBulk ().Write (state, buffer, new object[0]);
            new SpawnPosition().Write(state, buffer, new object[0]);
            // new PlayerPositionAndLook().Write(state, buffer, new object[0]);
            //new KeepAlive ().Write (state, buffer, new object[0]);
            state.StartKeepAliveTimer();
            state.Player.AddToList();
            state.Player.SendChunksFromPosition();
        }

        private string getUUID(string username)
        {
            if (string.IsNullOrEmpty(username))
                return "";

            using (WebClient webClient = new WebClient())
            {
               
                string result = "";
                try
                {
                    result = webClient.UploadString("https://api.mojang.com/profiles/minecraft", $"[\"{username}\"]");
                    
                }
                catch (WebException e)
                {
                    Globals.Logger.Log(LogType.Error, "Couldn't retrieve uuid for username" + " " + username);
                    Globals.Logger.Log(LogType.Error, e.Message);
                    return "";
                }

                dynamic json = JsonConvert.DeserializeObject(result);
                if (json.Count > 0)
                {
                    string UUID = json[0].id;
                    Globals.Logger.Log(LogType.Info, "UUID = " + Guid.Parse(UUID).ToString());
                    return Guid.Parse(UUID).ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }
    }
}
