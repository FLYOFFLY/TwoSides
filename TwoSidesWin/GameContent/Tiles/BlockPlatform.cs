using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class BlockPlatform : BaseTile
    {
        public BlockPlatform(float maxHp, int id) : base(maxHp, id)
        {
        }
        public override int GetSoildType() => 2;
    }
}
