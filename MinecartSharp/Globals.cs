﻿using System;
using System.Timers;
using System.Collections.Generic;
using MinecartSharp.Objects;
using MinecartSharp.Networking.Packets;
using MinecartSharp.Objects.Chunks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Utils;
namespace MinecartSharp
{
    internal class Globals
    {
        public static string ProtocolName = "MinecartSharp 1.12.2";
        public static int ProtocolVersion = 340;
        public static int MaxPlayers;
        public static int PlayersOnline = 0;
        public static Config Config;
        public static String Favicon;
        internal static List<IPacket> Packets = new List<IPacket>();
        public static int LastUniqueID = 0;
        public static byte Difficulty = 0;
        public static TcpListener ServerListener;
        public static List<Player> Players = new List<Player>();
        public static long TimeOfDay = 6000;
        public static long WorldAge;
        public static Logger Logger = new Logger();


        public static void setup()
        {
            ServerListener = new TcpListener(IPAddress.Any, 25565);
        }

        public static void LoadDebugChunks()
        {
            Program.Logger.Log(Utils.LogType.Info, "Generating debug chunks");
            Globals.WorldGen.GenerateChunkColumn(new Vector2(0, 0));
            Program.Logger.Log(Utils.LogType.Info, "Done debug chunks");
        }

        #region WorldGeneration
        public static FlatLandGenerator WorldGen = new FlatLandGenerator();
        public static List<ChunkColumn> ChunkColums = new List<ChunkColumn>();
        public static string LVLType = "flat";

        #endregion


        internal static void setupPackets()
        {
            Packets.Add(new Handshake());
            Packets.Add(new Ping());
            Packets.Add(new PlayerPosition());
            Packets.Add(new PlayerPositionAndLook());
            Packets.Add(new PlayerLook());
            Packets.Add(new ClientSettings());
            Packets.Add(new OnGround());
            Packets.Add(new KeepAlive());
            Packets.Add(new ChatMessagePacket());
        }

        #region TickTimer

        private static Thread _worldTimeThread;
        private static System.Timers.Timer _worldTimer;

        public static void StartWorldTimer()
        {
            _worldTimeThread = new Thread((StartTimeTimer));

            _worldTimeThread.Start();
            StartTimeTimer();
        }

        public static void StopWorldTimer()
        {
            Logger.Log(LogType.Info, "Stopping time of day timer");

            _worldTimer.Stop();
            _worldTimeThread.Abort();
        }

        private static void StartTimeTimer()
        {
            _worldTimer = new System.Timers.Timer(50);

            _worldTimer.Elapsed += (sender, args) =>
            {
                TimeOfDay++;
                WorldAge++;

                foreach (Player i in Globals.Players)
                {
                    new TimeUpdate().Write(i.Wrapper, i.buffer, new object[0]);
                }
            };
            _worldTimer.Start();
        }

        #endregion

    }
}
