using TwoSides.GameContent.Entity;
using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class BlockPortal : BaseTile
    {
        public BlockPortal(int hpMax,int id)
            : base(hpMax,id)
        {
        }
        public override bool IsSolid() => false;

        public override bool IsNeadTool(Item item) => false;

        public override bool UseBlock(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            ((Player)entity).TeleportToShowMap();
            return true;
        }
        public override int GetTickFrame() => 24;

        public override int GetAnimFrame() => 2;
    }
}
