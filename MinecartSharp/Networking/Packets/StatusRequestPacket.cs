using System;
using System.Collections.Generic;
using System.Linq;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using MinecartSharp.Utils;
using Newtonsoft.Json;

namespace MinecartSharp.Networking.Packets
{
    class StatusRequestPacket : IPacket
    {
        public int PacketId { get; } = 0x00;

        public State State { get; } = State.Status;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            Write(state, buffer, new object[0]);
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            buffer.WriteVarInt(0x00);

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
    }
}
