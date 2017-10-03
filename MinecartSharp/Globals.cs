using System;
using System.Collections.Generic;
using MinecartSharp.Objects;
using MinecartSharp.Networking.Packets;
using MinecartSharp.Objects.Chunks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Timers;
using MinecartSharp.Networking.Helpers;
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
        public static long TimeOfDay = 1200;
        public static long WorldAge = 0;
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
        }

        #region TickTimer
        private static Thread TimerThread = new Thread(() => StartTimeTimer());

        public static void StartTimeOfDayTimer()
        {
            TimerThread.Start();
        }

        public static void StopTimeOfDayTimer()
        {
            TimerThread.Abort();
            TimerThread = new Thread(() => StartTimeTimer());
        }

        static System.Timers.Timer kTimer = new System.Timers.Timer();

        private static void StartTimeTimer()
        {
            kTimer.Elapsed += new ElapsedEventHandler(RunTick);
            kTimer.Interval = 1000;
            kTimer.Start();
        }

        private static void StopTimeTimer()
        {
            kTimer.Stop();
        }

        private static void RunTick(object source, ElapsedEventArgs e)
        {
            if (TimeOfDay < 24000)
            {
                TimeOfDay += 20;
            }
            else
            {
                TimeOfDay = 0;
                WorldAge++;
            }

            foreach (Player i in Globals.Players)
            {
                new TimeUpdate().Write(i.Wrapper, new MSGBuffer(i.Wrapper), new object[0]);
            }
        }
        #endregion

    }
}
