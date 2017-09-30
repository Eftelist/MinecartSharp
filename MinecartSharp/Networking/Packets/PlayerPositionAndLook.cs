using MinecartSharp.MinecaftSharp.Networking.Interfaces;
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
    public class PlayerPositionAndLook : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x0E;
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
            double X = buffer.ReadDouble();
            double FeetY = buffer.ReadDouble();
            double Z = buffer.ReadDouble();
            float Yaw = buffer.ReadFloat();
            float Pitch = buffer.ReadFloat();
            bool OnGround = buffer.ReadBool();

            state.Player.OnGround = OnGround;
            state.Player.Yaw = Yaw;
            state.Player.Pitch = Pitch;
            state.Player.Coordinates = new Vector3(X, FeetY, Z);
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            buffer.WriteVarInt(0x08);
            buffer.WriteDouble(Globals.WorldGen.GetSpawnPoint().X);
            buffer.WriteDouble(Globals.WorldGen.GetSpawnPoint().Y);
            buffer.WriteDouble(Globals.WorldGen.GetSpawnPoint().Z);
            buffer.WriteFloat(0f);
            buffer.WriteFloat(0f);
            buffer.WriteByte(111);
            buffer.FlushData();
        }
    }
}
