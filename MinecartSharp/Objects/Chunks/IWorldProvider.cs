using MinecartSharp.Objects;
using MinecartSharp.Objects.Chunks;

namespace MinecartSharp.MinecartSharp.Objects.Chunks
{
    public interface IWorldProvider
    {
        bool IsCaching { get; }
        void Initialize();
        ChunkColumn GenerateChunkColumn(Vector2 chunkCoordinates);
        Vector3 GetSpawnPoint();
    }
}