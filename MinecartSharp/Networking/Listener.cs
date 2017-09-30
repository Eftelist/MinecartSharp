﻿using MinecartSharp.Utils;
using System.Net.Sockets;
using System.Threading.Tasks;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Wrappers;

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
                MSGBuffer Buf = new MSGBuffer(Client, clientStream);

               
                int ReceivedData =clientStream.Read(Buf.BufferedData, 0, Buf.BufferedData.Length);
                if (ReceivedData > 0)
                {
                    int length = Buf.ReadVarInt();
                    Buf.Size = length;
                    int packid = Buf.ReadVarInt();
                    bool found = false;
                    foreach (IPacket i in Globals.Packets)
                    {
                        if (i.PacketID == packid && i.IsPlayePacket == Client.PlayMode)
                        {
                            i.Read(Client, Buf, new object[0]);
                            found = true;
                            Program.Logger.Log(LogType.Info, "packet received! \"0x" + packid.ToString("X2") + "\"");
                            break;
                        }
                    }
                    if (!found)
                    {
                        Program.Logger.Log(LogType.Warning, "Unknown packet received! \"0x" + packid.ToString("X2") + "\"");
                    }
                }
                else
                {
                    //Stop the while loop. Client disconnected!
                    break;
                }
            }
            //Close the connection with the client. :)
            Client.StopKeepAliveTimer();
            if (Client.Player != null)
                Globals.Players.Remove(Client.Player);
            Client.TCPClient.Close();
        }
    }

}