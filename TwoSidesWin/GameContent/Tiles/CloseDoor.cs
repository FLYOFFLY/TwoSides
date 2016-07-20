using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;
using TwoSides.World.Generation;

namespace TwoSides.GameContent.Tiles
{
    public class CloseDoor : BaseTile
    {
        public CloseDoor(float MaxHP, int id)
            : base(MaxHP,id)
        {
        }
        public override bool blockuse(int x, int y, World.Generation.BaseDimension dimension, Physics.Entity.CEntity entity)
        {
            int xDoor = x;
            int yDoor = y + (2 - dimension.map[x, y].subTexture);
            int iddoor = dimension.map[xDoor, yDoor].idtexture;
            for (int j = 0; j < 3; j++)
            {
                dimension.Reset(xDoor, yDoor - j);
            }
            dimension.addDoor(iddoor, xDoor, yDoor, true);
            return true;
        }
        public void destorySide(BaseDimension dimension,int xDoor,int yDoor)
        {
            for (int i = 0; i <= 3; i++)
            {
                dimension.Reset(xDoor, yDoor - i);
            }
        }
        public override List<World.Item> destory(int x, int y, World.Generation.BaseDimension dimension, Physics.Entity.CEntity entity)
        {
            int yDoor = y;
            yDoor -= dimension.map[x, y].subTexture;
            destorySide(dimension, x, yDoor);
            return base.destory(x, y, dimension, entity);
        }
        public override void update(int x, int y, World.Generation.BaseDimension dimension, Physics.Entity.CEntity entity)
        {
            if (dimension.map[x, y + 1].active) return;
            dimension.map[x, y].destory(x, y, entity);
        }
    }
}
