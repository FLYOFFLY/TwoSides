using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace TwoSides.Physics.Entity
{
    public class CEntity
    {
        public Vector2 position;
        public Vector2 velocity;
        protected float gravity = 0.4f;
        protected float maxFallSpeed = 10f;
        public CEntity() { }
        public CEntity(Vector2 pos)
        {
            this.position = pos;
        }
        public CEntity(float x, float y)
        {
            position.X = x; 
            position.Y = y;
        }
        public void fail()
        {
            velocity.Y += gravity;
            if (this.velocity.Y > maxFallSpeed)
            {
                this.velocity.Y = maxFallSpeed;
            }

            float b = velocity.Y;
            velocity = Colision.TileCollision(this,position, velocity, 16, 16,false);
            position += velocity;
        
        }
        public virtual void load(BinaryReader reader)
        {
            position.X = (float)reader.ReadDouble();
            position.Y = (float)reader.ReadDouble();
            velocity.X = (float)reader.ReadDouble();
            velocity.Y = (float)reader.ReadDouble();

        }
        public virtual void save(BinaryWriter writer){
            writer.Write((double)position.X);
            writer.Write((double)position.Y);
            writer.Write((double)velocity.X);
            writer.Write((double)velocity.Y);
        }
        public virtual void update()
        {
            fail();
        }
    }
}
