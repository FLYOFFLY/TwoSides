using TwoSides.World;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class StoneBlock : BaseTile
    {
        public StoneBlock(int hpMax,int id)
            : base(hpMax, id)
        {
        }
        public override bool IsNeadTool(Item item) => item.GetTypeItem() == (int)Item.Type.PICKAXE;
    }
}
