using System.IO;

using Microsoft.Xna.Framework;

namespace TwoSides.Physics.Entity
{
    public class DynamicEntity
    {
        public Vector2 Position;
        public Vector2 Velocity;
        protected float Gravity = 0.4f;
        protected float MaxFallSpeed = 10f;
        public DynamicEntity() { }
        public DynamicEntity(Vector2 pos) => Position = pos;

        public DynamicEntity(float x, float y)
        {
            Position.X = x; 
            Position.Y = y;
        }
        public void AddForce(Vector2 force, float mass)
        {
            if (force.X >= 0)
                Velocity.X += mass / 2;
            else
                Velocity.X -= mass / 2;
        }
        public void Fail()
        {
            Velocity.Y += Gravity;
            if (Velocity.Y > MaxFallSpeed)
            {
                Velocity.Y = MaxFallSpeed;
            }
            Velocity = Colision.TileCollision(this, Position, Velocity, 16, 16, false);
            Position += Velocity;
        }
        public virtual void Load(BinaryReader reader)
        {
            Position.X = (float)reader.ReadDouble();
            Position.Y = (float)reader.ReadDouble();
            Velocity.X = (float)reader.ReadDouble();
            Velocity.Y = (float)reader.ReadDouble();

        }
        public virtual void Save(BinaryWriter writer){
            writer.Write((double)Position.X);
            writer.Write((double)Position.Y);
            writer.Write((double)Velocity.X);
            writer.Write((double)Velocity.Y);
        }
        public virtual void Update() => Fail();
    }
}
