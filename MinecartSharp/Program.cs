using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MinecartSharp
{
    class Program
    {

        public static Config Configuration = new Config();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting MinecartSharp v{0}", Assembly.GetExecutingAssembly().GetName().Version);

            // other things?

            Console.WriteLine("Loading server configuration");
            if (!File.Exists("config.json"))
            {
                using (StreamWriter file = File.CreateText("config.json"))
                {
                    file.Write(JsonConvert.SerializeObject(new Config(), Formatting.Indented));
                }
            }

            Configuration = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));


        }
    }
}
