using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;
using TwoSides.World;
using TwoSides.Physics.Entity;
using TwoSides.World.Generation;

namespace TwoSides.GameContent.Tiles
{
    public class BlockTable : BaseTile
    {
        public BlockTable(float MaxHP,int id)
            : base(MaxHP,id)
        {
        }
        public override bool issolid()
        {
            return false;
        }
        public override bool hasShadow()
        {
            return false;
        }
        public override List<Item> destory(int x, int y, BaseDimension dimension, CEntity entity)
        {
            if (id == 19) dimension.Reset(x-1, y);
            else dimension.Reset(x+1, y);
            List<Item> drop = new List<Item>();
            drop.Add(new Item(1, 26));
            return drop;
        }
    }
}
