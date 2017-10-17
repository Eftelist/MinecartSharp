using MinecartSharp.Utils;
using System.Net.Sockets;
using System.Threading.Tasks;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Wrappers;
using System;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Packets;

namespace MinecartSharp.Networking
{
    public class BasicListener
    {
        public System.Threading.Tasks.Task ListenForClientsAsync()
        {
            Globals.ServerListener.Start();
            Program.Logger.Log(LogType.Info, "Ready for connections");
            while (true)
            {
                TcpClient tcp = Globals.ServerListener.AcceptTcpClient();
                NetworkStream stream = tcp.GetStream();

#if DEBUG
                Program.Logger.Log(LogType.Info, "A new connection has been made");
#endif
                new Task((() => { HandleClientCommNew(tcp ,stream); })).Start();
            }
        }
        private void HandleClientCommNew(TcpClient tcpClient, NetworkStream clientStream)
        {
 
            ClientWrapper client = new ClientWrapper(tcpClient);

            while (tcpClient.Connected)
            {
                try
                {
                    var buf = new MSGBuffer(client, clientStream);
                    var receivedData = clientStream.Read(buf.BufferedData, 0, buf.BufferedData.Length);

                    if (receivedData > 0)
                    {
                        int length = buf.ReadVarInt();
                        buf.Size = length;
                        int packid = buf.ReadVarInt();
                        bool found = false;

                        foreach (IPacket i in Globals.Packets)
                        {
                            if (i.PacketID == packid && i.State == client.State)
                            {
                                i.Read(client, buf, new object[0]);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            Globals.Logger.Log(LogType.Error, "Unknown packet received! \"0x" + packid.ToString("X2") + "\"");
                        }

                        buf.Dispose();
                                
                    } else
                    {
                        break;
                    }
                } catch(Exception e)
                {
                    Globals.Logger.Log(LogType.Error, e.Message);
                    new Disconnect().Write(client, new MSGBuffer(client), new object[] { new ChatMessage(){ Text = "Server threw an exception!" } });
                    break;
                }
            }

            //Close the connection with the client.
            client.StopKeepAliveTimer();
            if (client.Player != null)
            {
                new Logger().Log(LogType.Info, $"{client.Player.Username} left the game.");
                Globals.Players.Remove(client.Player);
            }
            client.TcpClient.Close();
        }
    }

}