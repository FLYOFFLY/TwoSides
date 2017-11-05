using TwoSIdes.World.Tile;

namespace TwoSIdes.GameContent.Tiles
{
    public class BlockPlatform : BaseTile
    {
        public BlockPlatform(float maxHp, int id) : base(maxHp, id)
        {
        }
        public override int GetSoildType() => 2;
    }
}
