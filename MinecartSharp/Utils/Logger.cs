using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecartSharp.Utils
{
    class Logger
    {

        public void Log(LogType type, String message)
        {
            Console.Write(DateTime.Now.ToString("HH:mm:ss") + " [");

            switch (type)
            {
                case LogType.Info:
                    Console.Write("INFO");
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("WARNING");
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERROR");
                    break;
            }

            Console.ResetColor();

            Console.Write("] ");
            Console.WriteLine(message);
        }

    }
}
