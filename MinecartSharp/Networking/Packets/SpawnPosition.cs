﻿using MinecartSharp.MinecaftSharp.Networking.Interfaces;
using MinecartSharp.MinecartSharp.Networking;
using MinecartSharp.MinecartSharp.Networking.Helpers;
using MinecartSharp.MinecartSharp.Networking.Wrappers;
using MinecartSharp.MinecartSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecartSharp.Networking.Packets
{
    public class SpawnPosition : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x46;
            }
        }

        public bool IsPlayePacket
        {
            get
            {
                return true;
            }
        }

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            Vector3 D = Globals.WorldGen.GetSpawnPoint();
            long Data = (((long)D.X & 0x3FFFFFF) << 38) | (((long)D.Y & 0xFFF) << 26) | ((long)D.Z & 0x3FFFFFF);
            buffer.WriteVarInt(PacketID);
            buffer.WriteLong(Data);
            buffer.FlushData();
        }
    }
}