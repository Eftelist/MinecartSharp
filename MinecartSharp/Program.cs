using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using MinecartSharp.Networking;
using MinecartSharp.Utils;
using MinecartSharp.MinecartSharp.Networking;

namespace MinecartSharp
{
    class Program
    {

        public static Config Configuration = new Config();
        public static Logger Logger = new Logger();

        static void Main(string[] args)
        {
            Console.Title = "MinecartSharp server";
            Console.WriteLine("Starting MinecartSharp v{0}", Assembly.GetExecutingAssembly().GetName().Version);
            Globals.setup();
            Globals.setupPackets();
            Globals.LoadDebugChunks();

            // time of day and other things

            Globals.StartTimeOfDayTimer();


            // other things?

            Logger.Log(LogType.Info, "Loading server configuration");
            if (!File.Exists("config.json"))
            {
                using (StreamWriter file = File.CreateText("config.json"))
                {
                    file.Write(JsonConvert.SerializeObject(new Config(), Formatting.Indented));
                }
            }

            Configuration = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

            //TODO: add world loading shit

            var ClientListener = new Thread(() => new BasicListener().ListenForClientsAsync());
            ClientListener.Start();

        }
    }
}
