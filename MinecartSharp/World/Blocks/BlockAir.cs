namespace MinecartSharp.World.Blocks
{
    public class BlockAir : Block
    {
        internal BlockAir() : base(0)
        {
            IsReplacible = true;
            IsSolid = false;
        }
    }
}