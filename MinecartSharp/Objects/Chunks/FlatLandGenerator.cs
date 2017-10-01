﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Utils.Extra;
using MinecartSharp.MinecartSharp.Objects.Chunks;

namespace MinecartSharp.Objects.Chunks
{
    public class FlatLandGenerator : IWorldProvider
    {
        public List<ChunkColumn> _chunkCache = new List<ChunkColumn>();
        public bool IsCaching { get; private set; }

        public FlatLandGenerator()
        {
            IsCaching = true;
        }

        public void Initialize()
        {
        }

        public IEnumerable<ChunkColumn> GenerateChunks(int _viewDistance, double playerX, double playerZ, Dictionary<string, ChunkColumn> chunksUsed)
        {
            lock (chunksUsed)
            {
                Dictionary<string, double> newOrders = new Dictionary<string, double>();
                double radiusSquared = _viewDistance / Math.PI;
                double radius = Math.Ceiling(Math.Sqrt(radiusSquared));
                var centerX = ((int)playerX) / 16;
                var centerZ = ((int)playerZ) / 16;

                for (double x = -radius; x <= radius; ++x)
                {
                    for (double z = -radius; z <= radius; ++z)
                    {
                        var distance = (x * x) + (z * z);
                        if (distance > radiusSquared)
                        {
                            continue;
                        }
                        var chunkX = x + centerX;
                        var chunkZ = z + centerZ;
                        string index = GetChunkHash(chunkX, chunkZ);
                        newOrders[index] = distance;
                    }
                }

                if (newOrders.Count > _viewDistance)
                {
                    foreach (var pair in newOrders.OrderByDescending(pair => pair.Value))
                    {
                        if (newOrders.Count <= _viewDistance) break;
                        newOrders.Remove(pair.Key);
                    }
                }


                foreach (var chunkKey in chunksUsed.Keys.ToArray())
                {
                    if (!newOrders.ContainsKey(chunkKey))
                    {
                        chunksUsed.Remove(chunkKey);
                    }
                }

                Stopwatch stopwatch = new Stopwatch();
                long avarageLoadTime = -1;
                foreach (var pair in newOrders.OrderBy(pair => pair.Value))
                {
                    if (chunksUsed.ContainsKey(pair.Key)) continue;

                    stopwatch.Restart();

                    int x = Int32.Parse(pair.Key.Split(new[] { ':' })[0]);
                    int z = Int32.Parse(pair.Key.Split(new[] { ':' })[1]);

                    ChunkColumn chunk = GenerateChunkColumn(new Vector2(x, z));
                    chunksUsed.Add(pair.Key, chunk);

                    long elapsed = stopwatch.ElapsedMilliseconds;
                    if (avarageLoadTime == -1) avarageLoadTime = elapsed;
                    else avarageLoadTime = (avarageLoadTime + elapsed) / 2;
                    Debug.WriteLine("Chunk {2} generated in: {0} ms (Avarage: {1} ms)", elapsed, avarageLoadTime, pair.Key);

                    yield return chunk;
                }

                if (chunksUsed.Count != 96) Debug.WriteLine("Too many chunks used: {0}", chunksUsed.Count);
            }
        }

        private string GetChunkHash(double chunkX, double chunkZ)
        {
            return string.Format("{0}:{1}", chunkX, chunkZ);
        }

        public ChunkColumn GenerateChunkColumn(Vector2 chunkCoordinates)
        {
            var firstOrDefault = _chunkCache.FirstOrDefault(chunk2 => chunk2 != null && chunk2.X == chunkCoordinates.X && chunk2.Y == chunkCoordinates.Y);
            if (firstOrDefault != null)
            {
                return firstOrDefault;
            }

            var generator = new FlatLandGenerator();

            var chunk = new ChunkColumn();
            chunk.X = chunkCoordinates.X;
            chunk.Y = chunkCoordinates.Y;
            generator.PopulateChunk(chunk);

            /* chunk.SetBlock(0, 5, 0, 7);
             chunk.SetBlock(1, 5, 0, 41);
             chunk.SetBlock(2, 5, 0, 41);
             chunk.SetBlock(3, 5, 0, 41);
             chunk.SetBlock(3, 5, 0, 41);
             */
            _chunkCache.Add(chunk);

            return chunk;
        }

        public Vector3 GetSpawnPoint()
        {
            return new Vector3(1, 1, 1);
        }

        public void PopulateChunk(ChunkColumn chunk)
        {
            var random = new CryptoRandom();
            var blocks = new byte[16 * 16 * 256];
            int Last = 0;

            //for (int i = 0; i < (256 * 2); i += 2)
            for (int i = 0; i < blocks.Length; i += 2)
            {
                if (i < (256 * 2))
                {
                    byte[] Bedrock = BitConverter.GetBytes((ushort)(7 << 4) | 0);
                    blocks[i] = Bedrock[0];
                    blocks[i + 1] = Bedrock[1];
                }

                if (i >= (256 * 2) && i < (256 * 4))
                {
                    byte[] Grass = BitConverter.GetBytes((ushort)(3 << 4) | 0);
                    blocks[i] = Grass[0];
                    blocks[i + 1] = Grass[1];
                }

                if (i >= (256 * 4) && i < (256 * 6))
                {
                    byte[] Grass = BitConverter.GetBytes((ushort)(3 << 4) | 0);
                    blocks[i] = Grass[0];
                    blocks[i + 1] = Grass[1];
                }

                if (i >= (256 * 6) && i < (256 * 8))
                {
                    byte[] Grass = BitConverter.GetBytes((ushort)(2 << 4) | 0);
                    blocks[i] = Grass[0];
                    blocks[i + 1] = Grass[1];
                }
            }

            chunk.Blocks = blocks.ToArray();
        }

    }
}
