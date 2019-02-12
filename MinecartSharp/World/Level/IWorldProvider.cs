using System;
using System.Collections.Generic;
using MinecartSharp.Objects;
using MinecartSharp.World.Blocks;
using MinecartSharp.World.Chunks;

namespace MinecartSharp.World.Level
{
    public interface IWorldProvider
    {
        bool IsCaching { get; }
        void Initialize();
        ChunkColumn GenerateChunkColumn(Vector2 chunkCoordinates);
        Vector3 GetSpawnPoint();
        IEnumerable<ChunkColumn> GenerateChunks (int _viewDistance, double playerX, double playerZ, Dictionary<Tuple<int,int>, ChunkColumn> chunksUsed);
        void SaveChunks(string Folder);
        ChunkColumn LoadChunk(int x, int y);
        void SetBlock (INTVector3 cords, Block block, Level level, bool broadcast);
    }
}