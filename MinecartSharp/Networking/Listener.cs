using System;
using System.Net;
using System.Net.Sockets;
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
                throw new Exception("tcplistener is not initialized!");

            Program.Logger.Log(LogType.Info, "Ready to accept connections...");

            while (_isListening)
            {
                var client = _serverListener.AcceptTcpClient();
                IPEndPoint ip = client.Client.RemoteEndPoint as IPEndPoint;

# if DEBUG
                Program.Logger.Log(LogType.Info, "Connection from: " + (ip == null? "Uknown" : ip.Address.ToString()));
#endif
                new Task((() => { HandleClient(client); })).Start();
            }
        }

        public void HandleClient(TcpClient client)
        {
            var stream = client.GetStream();

            while (true)
            {
                //TODO: write packets system
            }
            
        }

        public void Stop()
        {
            _isListening = false;
            _serverListener.Stop();
        }

    }
}
