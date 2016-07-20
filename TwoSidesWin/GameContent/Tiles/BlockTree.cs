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
    class BlockTree: BaseTile
    {
        public BlockTree(float MaxHP,int id)
            : base(MaxHP,id)
        {
        }
        public override bool issolid()
        {
            return false;
        }
        public override void InTile(CEntity entity)
        {
            if (entity is Player) {
                ((Player)entity).isHorisontal = true;
            }
        }
        public override void update(int x, int y, BaseDimension dimension,CEntity entity)
        {
            if (dimension.map[x, y + 1].active) return;
            if (entity is Player)
            {
                ((Player)entity).getDrop(x, y);
            }
        }
    }
}
