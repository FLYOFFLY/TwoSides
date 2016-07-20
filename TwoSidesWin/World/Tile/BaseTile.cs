using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World;
using Microsoft.Xna.Framework;
using TwoSides.Physics.Entity;
using TwoSides.World.Generation;

namespace TwoSides.World.Tile
{
    public class BaseTile
    {
        public int id;
        public float maxHP;

        public BaseTile(float MaxHP,int id)
        {
            this.maxHP = MaxHP;
            this.id = id;
        }

        public virtual bool issolid()
        {
            return true;
        }

        public virtual bool isLightBlock()
        {
            return false;
        }


        public virtual int getIDSideTexture()
        {
            return -1;
        }

        public virtual void update(int x, int y,BaseDimension dimension,CEntity entity)
        {
        }

        public virtual bool isNeadTool(Item item)
        {
            return true;
        }
        public virtual void Update(int x,int y, BaseDimension dimension,bool isView) { }
        public virtual Rectangle getBoxRect(int x, int y, ITile title)
        {
            return new Rectangle(0, 0, 16, 16);
        }

        public virtual bool blockuse(int x, int y, BaseDimension dimension, CEntity entity)
        {
            return false;
        }

        public virtual List<Item> destory(int x,int y,BaseDimension dimension,CEntity entity)
        {
            List<Item> drop = new List<Item>();
            drop.Add(new Item(1, id));
            return drop;
        }

        public virtual bool hasShadow()
        {
            return true;
        }

        public virtual int getAnimFrame()
        {
            return 1;
        }

        public virtual int getTickFrame()
        {
            return 9999;
        }
        public virtual int getFrame(int x, int y, BaseDimension dimension, int frame)
        {
            return 0;
        }
        public virtual void InTile(CEntity entity)
        {
        }

        public virtual int getSoildType()
        {
            return 1;
        }
    }
}
