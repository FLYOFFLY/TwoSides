using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;
using TwoSides.World.Generation;

namespace TwoSides.GameContent.Tiles
{
    public class OpenDoor : CloseDoor 
    {

        public OpenDoor(float MaxHP, int id)
            : base(MaxHP,id)
        {
        }
        public override int getFrame(int x,int y,BaseDimension dimension, int frame)
        {
            if (dimension.map[x, y].idtexture == dimension.map[x - 1, y].idtexture) return 1;
            else return 0;
        }
        public override bool issolid()
        {
            return false;
        }
        public override bool blockuse(int x, int y, World.Generation.BaseDimension dimension, Physics.Entity.CEntity entity)
        {
            int xDoor = x;
            int yDoor = y+(2-dimension.map[x,y].subTexture);
            int iddoor = dimension.map[xDoor, yDoor].idtexture;
            if (dimension.map[xDoor, yDoor].idtexture == dimension.map[xDoor - 1,yDoor].idtexture) xDoor -= 1;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    dimension.Reset(xDoor+i, yDoor-j);
                }
            }
            dimension.addDoor(iddoor-1,xDoor,yDoor,false);
            return true;
        }
        public override List<World.Item> destory(int x, int y, World.Generation.BaseDimension dimension, Physics.Entity.CEntity entity)
        {
            int yDoor = y;
            yDoor -= dimension.map[x, y].subTexture;
            if (getFrame(x, y, dimension, 0) == 0) destorySide(dimension, x + 1, yDoor);
            else destorySide(dimension, x - 1, yDoor);
            return base.destory(x, y, dimension, entity);
        }
    }
}
