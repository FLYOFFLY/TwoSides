using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.Physics.Entity;
using TwoSides.World.Generation;

namespace TwoSides.World.Tile
{
    public class BaseTile
    {
        public static object Locked = new object();
        public int Id;
        public float MaxHp;

        public BaseTile(float maxHp,int id)
        {
            MaxHp = maxHp;
            Id = id;
        }

        public virtual bool IsSolid() => true;

        public virtual bool IsLightBlock() => false;

        public virtual int GetIdSideTexture() => -1;

        public virtual void Update(int x, int y,BaseDimension dimension,DynamicEntity entity)
        {
        }

        public virtual bool IsNeadTool(Item item) => true;
        public virtual void Update(int x,int y, BaseDimension dimension,bool isView) { }
        public virtual Rectangle GetBoxRect(int x, int y, Tile title) => new Rectangle(0, 0, 16, 16);

        public virtual bool UseBlock(int x, int y, BaseDimension dimension, DynamicEntity entity) => false;

        public virtual List<Item> Destory(int x,int y,BaseDimension dimension,DynamicEntity entity) => new List<Item> { new Item(1, Id) };

        public virtual bool HasShadow() => true;

        public virtual int GetAnimFrame() => 1;

        public virtual int GetTickFrame() => 9999;

        public virtual void Render(ITileDatecs tileDate, Render render, Texture2D texture, BaseDimension dimension, Vector2 pos, int x, int y, int frame, int subTexture, Color color)
        {
            lock (Locked)
            {
                render.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, 16, 16), new Rectangle(16 * frame, 16 * subTexture, 16, 16), color);
            }
        }
        public virtual void InTile(DynamicEntity entity)
        {
        }

        public virtual int GetSoildType() => 1;
        public virtual ITileDatecs ChangeTile() => null;
        public virtual bool BlockAdded(BaseDimension dimension,int x,int y) => true;
    }
}
