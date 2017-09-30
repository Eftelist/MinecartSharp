using MinecartSharp.MinecartSharp.Objects;
using System;
using MinecartSharp.Networking.Wrappers;

namespace MinecartSharp.Objects
{
    public class Player
    {
        public string Username { get; set; }
        public string UUID { get; set; }
        public ClientWrapper Wrapper { get; set; }
        public int UniqueServerID { get; set; }
        public Gamemode Gamemode { get; set; }

        //Location stuff
        public int Dimension { get; set; }
        public Vector3 Coordinates { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }

        //Client settings
        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public byte ChatFlags { get; set; }
        public bool ChatColours { get; set; }
        public byte SkinParts { get; set; }

        public void AddToList()
        {
            Globals.Players.Add(this);
        }

        public static Player GetPlayer(ClientWrapper wrapper)
        {
            foreach (Player i in Globals.Players)
            {
                if (i.Wrapper == wrapper)
                {
                    return i;
                }
            }
            throw new ArgumentOutOfRangeException("The specified player could not be found ;(");
        }
    }
}