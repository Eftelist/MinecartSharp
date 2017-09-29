using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MinecartSharp.Utils;
using System.Collections.Generic;
using MinecraftSharp.MinecartSharp.Networking.Wrappers;
using MinecraftSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking.Packets;
using MinecartSharp.MinecartSharp.Networking;

namespace MinecartSharp.Networking
{
    class Listener
    {

        private TcpListener _serverListener;
        private bool _isListening;
        private static List<byte> _buffer;
        private static NetworkStream _stream;

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
            ClientWrapper Client = new ClientWrapper(client);
            Client.MinecraftStream = new ByteBuffer(client.GetStream(), Client);
            var clientStream = client.GetStream();

            while (true)
            {
                // Ping
                while (true)
                {
                    MSGBuffer Buf = new MSGBuffer(Client);
                    int ReceivedData = clientStream.Read(Buf.BufferedData, 0, Buf.BufferedData.Length);
                    if (ReceivedData > 0)
                    {
                        int length = Buf.ReadVarInt();
                        Buf.Size = length;
                        int packid = Buf.ReadVarInt();
                        bool found = false;
                        foreach (IPacket i in Globals.Packets)
                        {
                            if (i.PacketID == packid)
                            {
                                i.Read(Client, Buf, new object[0]);
                                found = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Stop the while loop. Client disconnected!
                        break;
                    }
                }
            }
        }


        public void Stop()
        {
            _isListening = false;
            _serverListener.Stop();
        }

        internal static void WriteVarInt(int value)
        {
            while ((value & 128) != 0)
            {
                _buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            _buffer.Add((byte)value);
        }

        internal static void WriteString(string data)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);
            WriteVarInt(buffer.Length);
            _buffer.AddRange(buffer);
        }
        internal static void WriteShort(short value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        internal static void Write(byte b)
        {
            _stream.WriteByte(b);
        }

        internal static void Flush(int id = -1)
        {
            var buffer = _buffer.ToArray();
            _buffer.Clear();

            var add = 0;
            var packetData = new[] { (byte)0x00 };
            if (id >= 0)
            {
                WriteVarInt(id);
                packetData = _buffer.ToArray();
                add = packetData.Length;
                _buffer.Clear();
            }

            WriteVarInt(buffer.Length + add);
            var bufferLength = _buffer.ToArray();
            _buffer.Clear();

            _stream.Write(bufferLength, 0, bufferLength.Length);
            _stream.Write(packetData, 0, packetData.Length);
            _stream.Write(buffer, 0, buffer.Length);
        }
    }

}
