using System.Collections.Generic;

using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class CloseDoor : BaseTile
    {
        public CloseDoor(float maxHp, int id)
            : base(maxHp,id)
        {
        }
        public override bool UseBlock(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            int xDoor = x;
            int yDoor = y + (2 - dimension.MapTile[x, y].IdSubTexture);
            int idDoor = dimension.MapTile[xDoor, yDoor].IdTexture;
            for (int j = 0; j < 3; j++)
            {
                dimension.Reset(xDoor, yDoor - j);
            }
            dimension.AddDoor(idDoor, xDoor, yDoor, true);
            return true;
        }
        public void DestorySIde(BaseDimension dimension,int xDoor,int yDoor)
        {
            for (int i = 0; i <= 3; i++)
            {
                dimension.Reset(xDoor, yDoor - i);
            }
        }
        public override List<Item> Destory(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            int yDoor = y;
            yDoor -= dimension.MapTile[x, y].IdSubTexture;
            DestorySIde(dimension, x, yDoor);
            return base.Destory(x, y, dimension, entity);
        }
        public override void Update(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            if (dimension.MapTile[x, y + 1].Active) return;
            dimension.MapTile[x, y].Destory(x, y, entity);
        }
    }
}
