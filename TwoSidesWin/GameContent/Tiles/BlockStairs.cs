using TwoSides.GameContent.Entity;
using TwoSides.Physics.Entity;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class BlockStairs : BaseTile
    {
        public BlockStairs(float maxHp, int id) : base(maxHp, id)
        {
        }

        public override void InTile(DynamicEntity entity)
        {
            if (entity is Player player)
            {
                player.IsHorisontal = true;
            }
        }
        public override bool IsSolid() => false;
    }
}
