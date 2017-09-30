using System;
using System.Collections.Generic;
using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking.Packets;
using MinecartSharp.Objects;
using MinecartSharp.Networking.Packets;
using MinecartSharp.Objects.Chunks;
using MinecartSharp.MinecartSharp.Objects.Chunks;
using System.Net.Sockets;
using System.Net;
using MinecraftSharp.MinecartSharp.Objects;
using System.Threading;
using System.Timers;
using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecaftSharp.Networking.Packets;

namespace MinecartSharp.MinecartSharp.Networking
{
    internal class Globals
    {
        public static string ProtocolName = "MinecartSharp 1.12";
        public static int ProtocolVersion = 340;
        public static int MaxPlayers = 10;
        public static int PlayersOnline = 0;
        internal static List<IPacket> Packets = new List<IPacket>();
        public static int LastUniqueID = 0;
        public static byte Difficulty = 0;
        public static bool UseCompression = false;
        public static TcpListener ServerListener;
        public static List<Player> Players = new List<Player>();
        public static long TimeOfDay = 1200;
        public static long WorldAge = 0;


        public static void setup()
        {
                   ServerListener = new TcpListener(IPAddress.Any, 25565);
        }

        public static void LoadDebugChunks()
        {
            Program.Logger.Log(Utils.LogType.Info, "Generating debug chunks");
            Globals.ChunkColums.Add(Globals.WorldGen.GenerateChunkColumn(new Vector2(0, 0)));
            /*   int r = 49; //Radius. (13*4 = 52) and we need 49 Chunks for a player to spawn. So this should be fine :)
               int ox = 0, oy = 0; //Middle point

               int done = 0;
               for (int x = -r; x < r ; x++)
               {
                   int height = (int)Math.Sqrt(r * r - x * x);

                   for (int y = -height; y < height; y++)
                   {
                       Globals.ChunkColums.Add (Globals.WorldGen.GenerateChunkColumn (new Vector2 (x + ox, y + oy)));
                       done++;
                       if (done == r)
                           break;
                   }
                   if (done == r)
                       break;
               }*/
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
            Packets.Add(new LoginSuccess());
            Packets.Add(new Ping());
            Packets.Add(new KeepAlive());
            Packets.Add(new PlayerPosition());
            Packets.Add(new PlayerPositionAndLook());
            Packets.Add(new PlayerLook());
            Packets.Add(new ClientSettings());
            Packets.Add(new OnGround());
        }
        public static string[] ServerMOTD = new string[]
        {
            "§6§lSharpMC\n-§eComplete rewrite!",
            "§6§lSharpMC\n-§eThis server is written by Wuppie/Kennyvv!",
            "§6§lSharpMC\n-§eC# Powered!",
            "§6§lSharpMC\n-§eNow supports Minecraft 1.8 (Partially)"
        };

        public static string RandomMOTD
        {
            get
            {
                Random i = new Random();
                int Chosen = i.Next(0, ServerMOTD.Length);
                return ServerMOTD[Chosen];
            }
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
