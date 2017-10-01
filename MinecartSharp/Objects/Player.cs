﻿using MinecartSharp.MinecartSharp.Objects;
using System;
using MinecartSharp.Networking.Wrappers;
using System.Collections.Generic;
using MinecartSharp.MinecartSharp.Objects.Chunks;
using System.ComponentModel;
using MinecartSharp.Networking.Packets;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Objects.Chunks;

namespace MinecartSharp.Objects
{
    public class Player
    {
        public string Username { get; set; }
        public string UUID { get; set; }
        public ClientWrapper Wrapper { get; set; }
        public int UniqueServerID { get; set; }
        public Gamemode Gamemode { get; set; }
        public bool IsSpawned { get; set; }

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

        Vector2 CurrentChunkPosition = new Vector2(0, 0);
        public bool ForceChunkReload { get; set; }
        private Dictionary<string, ChunkColumn> _chunksUsed;

        public Player()
        {
             _chunksUsed = new Dictionary<string, ChunkColumn>();
        }

        public void AddToList()
        {
            Globals.Players.Add(this);
        }

        public void SendChunksFromPosition()
        {
            if (Coordinates == null)
            {
                Coordinates = Globals.WorldGen.GetSpawnPoint();
                ViewDistance = 9;
            }
            SendChunksForKnownPosition(false);
        }

        public void SendChunksForKnownPosition(bool force = false)
         {
             int centerX = (int)Coordinates.X / 16;
             int centerZ = (int)Coordinates.Z / 16;
 
             if (!force && IsSpawned && CurrentChunkPosition == new Vector2(centerX, centerZ)) return;
            CurrentChunkPosition.X = centerX;
 
             CurrentChunkPosition.Y = centerZ;
             var _worker = new BackgroundWorker();
             _worker.WorkerSupportsCancellation = true;
             _worker.DoWork += delegate(object sender, DoWorkEventArgs args)
             {
                BackgroundWorker worker = sender as BackgroundWorker;
                int Counted = 0;
                foreach (var chunk in Globals.WorldGen.GenerateChunks(ViewDistance, Coordinates.X, Coordinates.Z, force? new Dictionary<string, ChunkColumn>() : _chunksUsed))
                { 
                    if (worker.CancellationPending)
                    {
                        args.Cancel = true;
                        break;
                    }
                    Counted++;
                    new ChunkData().Write(Wrapper, new MSGBuffer(Wrapper), new object[]{ chunk.GetBytes()});


                   //Thread.Yield();
              }
                if (Counted >= ViewDistance && !IsSpawned)
                {
                   new PlayerPositionAndLook().Write(Wrapper, new MSGBuffer(Wrapper), new object[0]);

                    IsSpawned = true;
                    Globals.Players.Add(this);
                }

            };
            _worker.RunWorkerAsync();
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