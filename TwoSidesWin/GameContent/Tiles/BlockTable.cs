using System.Collections.Generic;

using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class BlockTable : BaseTile
    {
        public BlockTable(float maxHp,int id)
            : base(maxHp,id)
        {
        }
        public override bool IsSolid() => false;

        public override bool HasShadow() => false;

        public override List<Item> Destory(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            if (Id == 19) dimension.Reset(x-1, y);
            else dimension.Reset(x+1, y);
            List<Item> drop = new List<Item> {new Item(1 , 26)};
            return drop;
        }
    }
}
