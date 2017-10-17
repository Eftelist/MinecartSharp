using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using MinecartSharp.Networking;
using MinecartSharp.Utils;

namespace MinecartSharp
{
    class Program
    {

        public static Logger Logger = new Logger();

        static void Main(string[] args)
        {
            Console.Title = "MinecartSharp server";
            Console.WriteLine("Starting MinecartSharp v{0}", Assembly.GetExecutingAssembly().GetName().Version);
            Globals.Setup();
            Globals.setupPackets();

            Logger.Log(LogType.Info, "Loading server configuration");
            if (!File.Exists("config.json"))
            {
                using (StreamWriter file = File.CreateText("config.json"))
                {
                    file.Write(JsonConvert.SerializeObject(new Config(), Formatting.Indented));
                }
            }

            Globals.Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            Globals.MaxPlayers = Globals.Config.MaxPlayer;

            // daylightcycle seems broken (client gets kicked with packet error)
            if (Globals.Config.GetGamerule("DoDaylightCycle"))
            {
                Globals.StartWorldTimer();
            }

            if (File.Exists("server-icon.png"))
            {
                var servericon = File.ReadAllBytes("server-icon.png");
                Globals.Favicon = "data:image/png;base64," + Convert.ToBase64String(servericon);
            }

            //TODO: add world loading shit

            var clientListener = new Thread(() => new BasicListener().ListenForClientsAsync());
            clientListener.Start();

        }
    }
}
