using System;
using System.Net.Sockets;
using System.Timers;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Packets;
using MinecartSharp.Objects;

namespace MinecartSharp.Networking.Wrappers
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

        public void StartKeepAliveTimer(ClientWrapper client, MSGBuffer buffer)
        {
            kTimer.Elapsed += (sender, args) =>
            {
                new KeepAlive().Write(client, buffer, new object[0]);
            };
            kTimer.Interval = 10000;
            kTimer.Start();
        }

        public void StopKeepAliveTimer()
        {
            kTimer.Stop();
        }
    }
}
