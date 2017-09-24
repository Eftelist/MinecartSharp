using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MinecartSharp.Utils;

namespace MinecartSharp.Networking
{
    class Listener
    {

        private TcpListener _serverListener;
        private bool _isListening;

        public Listener()
        {
            _serverListener = new TcpListener(IPAddress.Any, Program.Configuration.Port);
            _serverListener.Start();

            _isListening = true;
        }

        public void HandleConnections()
        {
            if (_isListening == false)
                throw new Exception("tcpserver is not initialized!");

            Program.Logger.Log(LogType.Info, "Ready to accept connections...");

            while (_isListening)
            {
                // handle connections
            }
        }

        public void Stop()
        {
            _isListening = false;
            _serverListener.Stop();
        }

    }
}
