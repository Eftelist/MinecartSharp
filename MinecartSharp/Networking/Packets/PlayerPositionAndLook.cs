using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using MinecartSharp.Objects;

namespace MinecartSharp.Networking.Packets
{
    public class PlayerPositionAndLook : IPacket
    {
        public int PacketID { get; } = 0x0E;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
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

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            buffer.WriteVarInt(0x2F);
            buffer.WriteDouble(1.0);
            buffer.WriteDouble(1.0);
            buffer.WriteDouble(1.0);
            buffer.WriteFloat(0f);
            buffer.WriteFloat(0f);
            buffer.WriteByte(111);
            buffer.WriteVarInt(1);
            buffer.FlushData();
        }
    }
}
