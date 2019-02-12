using fNbt;

namespace MinecartSharp.World.NBT
{
    public interface INbtSerializable
    {
        NbtTag Serialize(string tagName);
        void Deserialize(NbtTag value);
    }
}