using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;
using TwoSides.Physics.Entity;
using TwoSides.World.Generation;

namespace TwoSides.GameContent.Tiles
{
    public class BlockPortal : BaseTile
    {
        public BlockPortal(int hpMax,int id)
            : base(hpMax,id)
        {
        }
        public override bool issolid()
        {
            return false;
        }
        public override bool isNeadTool(World.Item item)
        {
            return false;
        }
        public override bool blockuse(int x, int y, BaseDimension dimension, CEntity entity)
        {
            ((Player)entity).TeleportToShowMap();
            return true;
        }
        public override int getTickFrame()
        {
            return 24;
        }
        public override int getAnimFrame()
        {
            return 2;
        }
    }
}
