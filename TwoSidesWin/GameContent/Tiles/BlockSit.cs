using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;
using Microsoft.Xna.Framework;
using TwoSides.World;
using TwoSides.Physics.Entity;
using TwoSides.World.Generation;

namespace TwoSides.GameContent.Tiles
{
    public class BlockSit : BaseTile
    {
        public BlockSit(int id,float MaxHP):base(MaxHP,id)
        {
        }
        public override Rectangle getBoxRect(int x, int y, ITile title)
        {
            if (id == 17) return base.getBoxRect(x,y,title);
            else return new Rectangle(0,0,6,16);
        }
        public override bool issolid()
        {
            return true;
        }
        public override bool hasShadow()
        {
            return false;
        }

        public override List<Item> destory(int x,int y, BaseDimension dimension, CEntity entity)
        {
            if (id == 17) dimension.Reset(x, y-1);
            else dimension.Reset(x, y+1);
            List<Item> drop = new List<Item>();
            drop.Add(new Item(1, 27));
            return drop;
        }
        public override bool blockuse(int x,int y,BaseDimension dimension,CEntity entity)
        {
            Program.game.AddExplosion(new Vector2(x * 16, y * 16));
            return true;
        }
    }
}
