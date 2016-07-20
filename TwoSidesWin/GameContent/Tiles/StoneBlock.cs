using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;
using TwoSides.World;

namespace TwoSides.GameContent.Tiles
{
    public class StoneBlock : BaseTile
    {
        public StoneBlock(int hpMax,int id)
            : base(hpMax, id)
        {
        }
        public override bool isNeadTool(Item item)
        {
            if (item.getTypeItem() == (int)Item.Type.PICKAXE) return true;
            return false;
        }
    }
}
