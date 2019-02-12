using MinecartSharp.Objects;

namespace MinecartSharp.World.Blocks
{
    public class Block
    {
        public INTVector3 Coordinates { get; set; }
        public ushort Id { get; set; }
        public bool IsReplacible { get; set; }
        public ushort Metadata { get; set; }
        public bool IsSolid { get; set; }
        public float Durability { get; set; }

        internal Block(ushort id)
        {
            Id = id;
            IsSolid = true;
            Durability = 0.5f;
        }

        public bool CanPlace(Level.Level world)
        {
            return CanPlace(world, Coordinates);
        }

        protected virtual bool CanPlace(Level.Level world, INTVector3 blockCoordinates)
        {
            return world.GetBlock(blockCoordinates).IsReplacible;
        }

        public virtual void BreakBlock(Level.Level world)
        {
            world.SetBlock(new BlockAir() {Coordinates = Coordinates});
        }

        public virtual bool PlaceBlock(Level.Level world, Player player, INTVector3 blockCoordinates, BlockFace face)
        {
            // No default placement. Return unhandled.
            return false;
        }

        public virtual bool Interact(Level.Level world, Player player, INTVector3 blockCoordinates, BlockFace face)
        {
            // No default interaction. Return unhandled.
            return false;
        }

        protected INTVector3 GetNewCoordinatesFromFace(Vector3 target, BlockFace face)
        {
            INTVector3 intVector = new INTVector3((int) target.X, (int) target.Y, (int) target.Z);
            switch (face)
            {
                case BlockFace.NegativeY:
                    intVector.Y--;
                    break;
                case BlockFace.PositiveY:
                    intVector.Y++;
                    break;
                case BlockFace.NegativeZ:
                    intVector.Z--;
                    break;
                case BlockFace.PositiveZ:
                    intVector.Z++;
                    break;
                case BlockFace.NegativeX:
                    intVector.X--;
                    break;
                case BlockFace.PositiveX:
                    intVector.X++;
                    break;
                default:
                    break;
            }

            return intVector;
        }

    }

    public enum BlockFace
    
    {
        NegativeY,
        PositiveY,
        NegativeZ,
        PositiveZ,
        NegativeX,
        PositiveX
    }
}