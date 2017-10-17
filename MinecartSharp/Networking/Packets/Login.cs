using System;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using MinecartSharp.Objects;
using MinecartSharp.Utils;
using MinecartSharp.Utils.Mojang;

namespace MinecartSharp.Networking.Packets
{
    class Login : IPacket
    {
        public int PacketId { get; } = 0x00;

        public State State { get; } = State.Login;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            Write(state, buffer, new object[0]);
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            if (state.ProtocolVersion < Globals.ProtocolVersion)
            {
                new Disconnect().Write(state, buffer, new[]{ new ChatMessage()
                {
                    Text = "This server is running " + Globals.ProtocolName + " please upgrade your client!"
                }});
                return;
            }

            if (state.ProtocolVersion > Globals.ProtocolVersion)
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

            string UUID = new MojangApi().GetUUID(username);

            if (string.IsNullOrWhiteSpace(UUID))
            {
                new Disconnect().Write(state, buffer, new[]{ new ChatMessage()
                {
                    Text = "Something got wrong with parsing your uuid, please try again later! (Maybe mojang servers are offline)"
                }});
                return;
            }

            Program.Logger.Log(LogType.Info, username + " Logging in...");

            new LoginSuccess().Write(state, buffer, new object[] { UUID, username });

            Globals.LastUniqueId++;
            state.Player = new Player() { UUID = UUID, Username = username, UniqueServerID = Globals.LastUniqueId, Wrapper = state, buffer = buffer, Gamemode = Gamemode.Creative, Dimension = 0 };
            state.State = State.Play;

            new JoinGame().Write(state, buffer, new object[] { state.Player });
            // new MapChunkBulk ().Write (state, buffer, new object[0]);
            new SpawnPosition().Write(state, buffer, new object[0]);
            new PlayerPositionAndLook().Write(state, buffer, new object[0]);

            new Logger().Log(LogType.Info, $"{state.Player.Username} joined the game.");

            state.StartKeepAliveTimer(state, buffer);
        }
    }
}
