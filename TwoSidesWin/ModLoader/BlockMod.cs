using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework;
using TwoSides.World.Tile;
using TwoSides.World;
using TwoSides.Physics.Entity;

namespace TwoSides.ModLoader
{
    public class BlockMod
    {
        public int ID;
        public int HP;
        public string IMG;
        public List<Item> drop = new List<Item>();
        public Texture2D texture;
      // bool isSoild;
       // bool isLight;

        public BlockMod(int id, string img,int hp,List<Item> drop)
        {
            this.ID = id;
            this.IMG = img;
            this.HP = hp;
            this.drop = drop;
        }
        
        public void setTexture(Texture2D tex){
            this.texture = tex;
        }

        public override bool Equals(object obj)
        {
            if (obj is BlockMod) { 
                BlockMod bm = (BlockMod)obj;
                if (bm.ID == ID) return true;
            }
            return false;
        }

        public virtual bool issolid()
        {
            return true;
        }

        public virtual bool isLightBlock()
        {
            return false;
        }

        public virtual Vector2 getOffset(int x, int y, ITile tile)
        {
            return Vector2.Zero;
        }

        public virtual bool isNeadTool(Item item)
        {
            return true;
        }

        public virtual Rectangle getBoxRect(int x, int y, ITile title)
        {
            return new Rectangle(0, 0, 16, 16);
        }

        public virtual bool blockuse(object player)
        {
            return false;
        }

        public virtual List<Item> destory(int id, CEntity entity)
        {
            
            return  drop;
        }
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
