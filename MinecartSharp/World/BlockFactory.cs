using MinecartSharp.World.Blocks;

namespace MinecartSharp.World
{
    class BlockFactory
    {
        public static Block GetBlockById(ushort id)
        {
            return new Block(id);
        }
    }
}