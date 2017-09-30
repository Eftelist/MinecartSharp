using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecartSharp.Networking.Packets;
using MinecraftSharp.MinecartSharp.Objects;
using System.Net.Sockets;
using System.Timers;

namespace MinecartSharp.MinecartSharp.Networking.Wrappers
{
    public class ClientWrapper
    {
        public TcpClient TCPClient;
        public Player Player;
        public bool PlayMode = false;

        public ClientWrapper(TcpClient client)
        {
            TCPClient = client;
        }

        public void SendData(byte[] Data, int Length)
        {
            try
            {
                NetworkStream a = TCPClient.GetStream();
                a.Write(Data, 0, Length);
                a.Flush();
            }
            catch
            {
                Globals.Logger.Log(Utils.LogType.Error, "Packet failed");
            }
        }

        public void SendData(byte[] Data, int Offset, int Length)
        {
            try
            {
                NetworkStream a = TCPClient.GetStream();
                a.Write(Data, Offset, Length);
                a.Flush();
            }
            catch
            {
                Globals.Logger.Log(Utils.LogType.Error, "Packet failed");
            }
        }

        public void SendData(byte[] Data)
        {
            try
            {
                NetworkStream a = TCPClient.GetStream();
                a.Write(Data, 0, Data.Length);
                a.Flush();
            }
            catch
            {
                Globals.Logger.Log(Utils.LogType.Error, "Packet failed");
            }
        }

        Timer kTimer = new Timer();

        public void StartKeepAliveTimer()
        {
            kTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            kTimer.Interval = 5000;
            kTimer.Start();
        }

        public void StopKeepAliveTimer()
        {
            kTimer.Stop();
        }

        public void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            new KeepAlive().Write(this, new MSGBuffer(this), new object[0]);
        }
    }
}
