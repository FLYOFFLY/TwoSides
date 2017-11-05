using System.Collections.Generic;

using TwoSides.GameContent.Entity;
using TwoSides.GameContent.GenerationResources;
using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class BlockNull : BaseTile
    {
        public BlockNull(int id): base(0,id){}
        public override bool HasShadow() => false;

        public override bool IsSolid() => false;

        public override List<Item> Destory(int x,int y, BaseDimension dimension,DynamicEntity entity)
        {
            List<Item> itemDrop = new List<Item>();
            float procent = Program.Game.Player.Carma / 100.0f;
            switch ( Id ) {
                case 27:
                    if ( Program.Game.Rand.Next(100) < (int) (80 * procent) )
                        itemDrop.Add(new Item(1 , 50));
                    else if ( Program.Game.Rand.Next(100) < (int) (90 * procent) )
                        itemDrop.Add(new Item(1 , 19));
                    break;
                case 32 when dimension.MapTile[x , y].IdSubTexture == 3:
                    int minValue = 2 + (int) (4 * procent);
                    itemDrop.Add(new Item(dimension.Rand.Next(minValue , minValue + minValue / 2) , 51));
                    itemDrop.Add(new Item(dimension.Rand.Next(minValue , minValue + minValue * 2) , 50));
                    break;
            }
            return itemDrop;
        }
        public override int GetTickFrame()
        {
            if (Id == 27) return 2;

            return Id == 29 ? 60 : 9999;
        }
        public void AddTexture(int newX, int newY,BaseDimension dimension)
        {
            if (newX < 0 || newX >= SizeGeneratior.WorldWidth) return;
            if (dimension.MapTile[newX, newY].Active) return;
            if (!dimension.MapTile[newX, newY+1].IsSolid()) return;

            dimension.SetTexture(newX, newY, Id);
        }
        public override void Update(int x, int y, BaseDimension dimension, bool isView)
        {
            switch ( Id ) {
                case 27:
                    if (isView) return;
                    if (dimension.Rand.Next(0, 3) != 0) return;
                    int direction = dimension.Rand.Next(0, 2);
                    switch ( direction ) {
                        case 0:
                            AddTexture(x + 1, y, dimension);
                            break;
                        case 1:
                            AddTexture(x + 1, y, dimension);
                            break;
                    }
                    break;
                case 32 when dimension.MapTile[x,y].IdSubTexture <3:
                    if (dimension.MapTile[x,y].TimeCount%120 != 110) return;
                    dimension.MapTile[x, y].IdSubTexture++;
                    break;
            }
        }
        public override void Update(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            if (dimension.MapTile[x, y + 1].Active) return;
            if (entity is Player player)
                player.GetDrop(x, y);
        }
        public override void InTile(DynamicEntity entity)
        {
            if (entity is Player)
            {
               // ((Player)entity).ActiveSpecial(0);
            }
        }
        public override int GetAnimFrame()
        {
            switch (Id)
            {
                case 27:
                    return 4;
                case 29:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
