﻿using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using MinecartSharp.Objects;

namespace MinecartSharp.Networking.Packets
{
    public class PlayerPosition : IPacket
    {
        public int PacketID { get; } = 0x0D;

        public State State { get; } = State.Play;

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {
            Player targetplayer = state.Player;
            double X = buffer.ReadDouble();
            double FeetY = buffer.ReadDouble();
            double Z = buffer.ReadDouble();
            bool OnGround = buffer.ReadBool();
            targetplayer.Coordinates = new Vector3(X, FeetY, Z);
            state.Player.OnGround = OnGround;
        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] arguments)
        {

        }
    }
}
