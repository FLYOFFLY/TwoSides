using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class BlockSit : BaseTile
    {
        public BlockSit(int id,float maxHp):base(maxHp,id)
        {
        }
        public override Rectangle GetBoxRect(int x, int y, Tile title) => title.IdSubTexture == 0 ? base.GetBoxRect(x,y,title) : new Rectangle(0,0,6,16);

        public override bool IsSolid() => true;

        public override bool HasShadow() => false;

        public override List<Item> Destory(int x,int y, BaseDimension dimension, DynamicEntity entity)
        {
            if (dimension.MapTile[x, y].IdSubTexture == 0) dimension.Reset(x, y-1);
            else dimension.Reset(x, y+1);
            List<Item> drop = new List<Item> {new Item(1 , 27)};
            return drop;
        }
        public override bool UseBlock(int x,int y,BaseDimension dimension,DynamicEntity entity) => true;

        public override bool BlockAdded(BaseDimension dimension, int x, int y)
        {
            if (dimension.MapTile[x, y - 1].Active) return false;
            dimension.SetTexture(x, y -1, Id,1); return true;
        }
    }
}
