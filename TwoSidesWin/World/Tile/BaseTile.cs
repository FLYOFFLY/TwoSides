using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSIdes.Physics.Entity;
using TwoSIdes.World.Generation;

namespace TwoSIdes.World.Tile
{
    public class BaseTile
    {
        public int Id;
        public float MaxHp;

        public BaseTile(float maxHp,int id)
        {
            MaxHp = maxHp;
            Id = id;
        }

        public virtual bool IsSolId() => true;

        public virtual bool IsLightBlock() => false;

        public virtual int GetIdSIdeTexture() => -1;

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

        public virtual void Render(ITileDatecs tileDate, SpriteBatch spriteBatch, Texture2D texture, BaseDimension dimension, Vector2 pos, int x, int y, int frame, int subTexture, Color color)
        {
            spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, 16, 16), new Rectangle(16 * frame, 16 * subTexture, 16, 16), color,0.0f,Vector2.Zero,SpriteEffects.None,0);
        }
        public virtual void InTile(DynamicEntity entity)
        {
        }

        public virtual int GetSoildType() => 1;
        public virtual ITileDatecs ChangeTile() => null;
        public virtual bool BlockAdded(BaseDimension dimension,int x,int y) => true;
    }
}
