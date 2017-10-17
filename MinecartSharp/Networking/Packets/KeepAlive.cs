using System;
using System.Diagnostics;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Networking.Packets
{
    public class KeepAlive : IPacket
    {
        public int PacketId { get; } = 0x0b;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            //TODO: add system to kick client if no keep alive is send for 20 seconds
            Console.WriteLine("client responds at keep alive");
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)); //http://i.imgur.com/S9ReRjS.png
            var unixtime = (long)timeSpan.TotalSeconds;

            buffer.WriteVarInt(0x1F);
            buffer.WriteLong(unixtime);
            buffer.FlushData();
        }
    }
}
