using MinecartSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
            int protocol = buffer.ReadVarInt();
            string host = buffer.ReadString();
            short port = buffer.ReadShort();
            int nextstate = buffer.ReadVarInt();

            switch (nextstate)
            {
                case 1:
                    HandleStatusRequest(state, buffer);
                    break;
                case 2:
                    HandleLoginRequest(state, buffer, protocol);
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
                    Online = Globals.Players.Count,
                    Players = Globals.Players.Select(xp => new ServerpingPlayer { Name = xp.Username, Id = xp.UUID }).ToList()
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

        private void HandleLoginRequest(ClientWrapper state, MSGBuffer buffer, int protocol)
        {
            if (protocol < Globals.ProtocolVersion)
            {
                new Disconnect().Write(state, buffer, new []{ new ChatMessage()
                {
                    Text = "This server is running " + Globals.ProtocolName + " please upgrade your client!"
                }});
                return;
            }

            if (protocol > Globals.ProtocolVersion)
            {
                new Disconnect().Write(state, buffer, new[]{ new ChatMessage()
                {
                    Text = "This server is still running " + Globals.ProtocolName + " please downgrade your client!"
                }});
                return;
            }

            string username = buffer.ReadUsername();
            Console.WriteLine(username);

            if (string.IsNullOrWhiteSpace(username))
            {
                new Disconnect().Write(state, buffer, new[]{ new ChatMessage()
                {
                    Text = "Something got wrong with parsing your username, please try again later!"
                }});
                return;
            }

            string UUID = getUUID(username);
            Program.Logger.Log(LogType.Warning, "Logging in : " + UUID);
            
            new LoginSuccess().Write(state, buffer, new object[] { UUID, username });

            Globals.LastUniqueID++;
            state.Player = new Player() { UUID = UUID, Username = username, UniqueServerID = Globals.LastUniqueID, Wrapper = state, buffer = buffer, Gamemode = Gamemode.Creative, Dimension = 0 };
            state.PlayMode = true;

            new JoinGame().Write(state, buffer, new object[] { state.Player });
            // new MapChunkBulk ().Write (state, buffer, new object[0]);
            new SpawnPosition().Write(state, buffer, new object[0]);
            new PlayerPositionAndLook().Write(state, buffer, new object[0]);

            new Logger().Log(LogType.Info, $"{state.Player.Username} joined the game.");

            state.StartKeepAliveTimer(state, buffer);
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
