﻿using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecartSharp.Networking.Packets;
using MinecartSharp.MinecartSharp.Networking.Wrappers;
using MinecartSharp.MinecartSharp.Objects;
using MinecartSharp.Networking.Packets;
using MinecartSharp.Utils;
using MinecraftSharp.MinecartSharp.Objects;
using System;
using System.Net;
using MinecartSharp;
using MinecartSharp.Networking.Objects;
using Newtonsoft.Json;

namespace MinecaftSharp.Networking.Packets
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
            Program.Logger.Log(LogType.Warning, "Logging in...");
            new LoginSuccess().Write(state, buffer, new object[] { UUID, username });
            Globals.LastUniqueID++;
            state.Player = new Player() { UUID = UUID, Username = username, UniqueServerID = Globals.LastUniqueID, Wrapper = state, Gamemode = Gamemode.Creative };
            state.PlayMode = true; //Toggle the boolean to PlayMode so we know we are not handling Status stuff anymore.

            if (!Globals.UseCompression)
                new SetCompression().Write(state, buffer, new object[] { -1 }); //Turn off compression.

            new JoinGame().Write(state, buffer, new object[0]);
            for (int i = 0; i < Globals.ChunkColums.Count; i++)
            {
                new ChunkData().Write(state, buffer, new object[] { Globals.ChunkColums[i].GetBytes() }); //Just testing if the packet get's received correctly by the client...
            }
            //  new MapChunkBulk ().Write (state, buffer, new object[0]);
            new SpawnPosition().Write(state, buffer, new object[0]);
            new PlayerPositionAndLook().Write(state, buffer, new object[0]);
            //new KeepAlive ().Write (state, buffer, new object[0]);
            state.StartKeepAliveTimer();
            state.Player.AddToList();
        }

        private string getUUID(string username)
        {
            using(WebClient webClient = new WebClient())
            {
                string result = "";

                try
                {
                    result =
                        webClient.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username + "?at=" + (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
                }
                catch (WebException e)
                {
                    Globals.Logger.Log(LogType.Error, "Couldn't retrieve uuid for username " + username);
                    return "";
                }

                dynamic json = JsonConvert.DeserializeObject(result);

                if (json.Count > 1)
                {
                    string UUID = json.id;
                    Program.Logger.Log(LogType.Info, "UUID = " + new Guid(UUID).ToString());
                    return new Guid(UUID).ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }
    }
}
