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
 
            ClientWrapper Client = new ClientWrapper(tcpClient);

            while (tcpClient.Connected)
            {
                try
                {
                    MSGBuffer Buf = new MSGBuffer(Client, clientStream);
                    int ReceivedData = clientStream.Read(Buf.BufferedData, 0, Buf.BufferedData.Length);
                    if (ReceivedData > 0)
                    {
                        int length = Buf.ReadVarInt();
                        Buf.Size = length;
                        int packid = Buf.ReadVarInt();
                        bool found = false;

                        Console.WriteLine(packid.ToString("X2"));

                        foreach (IPacket i in Globals.Packets)
                        {
                            if (i.PacketID == packid && i.State == Client.State)
                            {
                                i.Read(Client, Buf, new object[0]);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            Globals.Logger.Log(LogType.Error, "Unknown packet received! \"0x" + packid.ToString("X2") + "\"");
                        }

                        Buf.Dispose();
                                
                    } else
                    {
                        break;
                    }
                } catch(Exception e)
                {
                    Globals.Logger.Log(LogType.Error, e.Message);
                    new Disconnect().Write(Client, new MSGBuffer(Client), new object[] { new ChatMessage(){ Text = "Server threw an exception!" } });
                    break;
                }
            }
            //Close the connection with the client. :)
            Client.StopKeepAliveTimer();
            if (Client.Player != null)
            {
                new Logger().Log(LogType.Info, $"{Client.Player.Username} left the game.");
                Globals.Players.Remove(Client.Player);
            }
            Client.TcpClient.Close();
        }
    }

}