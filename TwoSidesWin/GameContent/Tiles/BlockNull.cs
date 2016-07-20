using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;
using TwoSides.World.Generation;
using TwoSides.Physics.Entity;
using TwoSides.GameContent.GenerationResources;

namespace TwoSides.GameContent.Tiles
{
    public class BlockNull : BaseTile
    {
        public BlockNull(int id): base(0,id){}
        public override bool hasShadow()
        {
            return false;
        }
        public override bool issolid()
        {
            return false;
        }
        public override List<World.Item> destory(int x,int y, BaseDimension dimension,Physics.Entity.CEntity entity)
        {
            List<World.Item> itemDrop = new List<World.Item>();
            if (id == 27)
            {
                if (Program.game.rand.Next(100) < 20)
                    itemDrop.Add(new World.Item(1, 19));
                else if (Program.game.rand.Next(100) < 50)
                    itemDrop.Add(new World.Item(1,50));
            } 
            return itemDrop;
                
        }
        public override int getTickFrame()
        {
            if (id == 27)
                return 2;
            if (id == 29)
                return 60;
            return 9999;
        }
        public void addTexture(int newX, int newY,BaseDimension dimension)
        {
            if (newX < 0 || newX >= SizeGeneratior.WorldWidth) return;
            if (dimension.map[newX, newY].active) return;
            if (!dimension.map[newX, newY+1].issolid()) return;

            dimension.settexture(newX, newY, id);
        }
        public override void Update(int x, int y, BaseDimension dimension, bool isView)
        {
            if (id == 27)
            {
                if (isView) return;
                if (dimension.rand.Next(0, 3) != 0) return;
                int direction = dimension.rand.Next(0, 2);
                if (direction == 0) addTexture(x + 1, y, dimension);
                if (direction == 1) addTexture(x + 1, y, dimension);
            }
            if (id == 32 && dimension.map[x,y].subTexture <3)
            {
                if (dimension.map[x,y].timeCount%120 != 110) return;
                dimension.map[x, y].subTexture++;
            }
        }
        public override void update(int x, int y, BaseDimension dimension, CEntity entity)
        {
            if (dimension.map[x, y + 1].active) return;
            if (entity is Player)
            {
                ((Player)entity).getDrop(x, y);
            }
        }
        public override void InTile(Physics.Entity.CEntity entity)
        {
            if (entity is Player)
            {
               /// ((Player)entity).activespecial(0);
            }
        }
        public override int getAnimFrame()
        {
            if(id == 27)
                return 4;
            if (id == 29)
                return 2;
            return 1;
        }
    }
}
