using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class OpenDoor : CloseDoor 
    {

        public OpenDoor(float maxHp, int id)
            : base(maxHp,id)
        {
        }
        public override bool IsSolid() => false;

        public override void Render(ITileDatecs tileDate, Render render, Texture2D texture, BaseDimension dimension, Vector2 pos, int x, int y, int frame, int subTexture, ColorScheme color)
        {
            if (dimension.MapTile[x, y].IdTexture == dimension.MapTile[x - 1, y].IdTexture) frame += 1;
            base.Render(tileDate,render, texture, dimension, pos, x, y, frame, subTexture, color);
        }
        public override bool UseBlock(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            var xDoor = x;
            var yDoor = y+(2-dimension.MapTile[x,y].IdSubTexture);
            int iddoor = dimension.MapTile[xDoor, yDoor].IdTexture;
            if (dimension.MapTile[xDoor, yDoor].IdTexture == dimension.MapTile[xDoor - 1,yDoor].IdTexture) xDoor -= 1;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    dimension.Reset(xDoor+i, yDoor-j);
                }
            }
            dimension.AddDoor(iddoor-1,xDoor,yDoor,false);
            return true;
        }
        public override List<Item> Destory(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            var yDoor = y;
            yDoor -= dimension.MapTile[x, y].IdSubTexture;
            if (dimension.MapTile[x, y].IdTexture != dimension.MapTile[x - 1, y].IdTexture) DestorySIde(dimension, x + 1, yDoor);
            else DestorySIde(dimension, x - 1, yDoor);
            return base.Destory(x, y, dimension, entity);
        }
    }
}
