using System;
using System.Collections.Generic;
using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking.Packets;

namespace MinecartSharp.MinecartSharp.Networking
{
    internal class Globals
    {
        public static string ProtocolName = "MinecartSharp 1.12";
        public static int ProtocolVersion = 340;
        public static int MaxPlayers = 10;
        public static int PlayersOnline = 0;
        public static string ServerMOTD = "Themepark lol";
        internal static List<IPacket> Packets = new List<IPacket>();

        internal static void setupPackets()
        {
            Packets.Add(new HandshakePacket());
            Packets.Add(new LoginSuccess());
        }
    }
}