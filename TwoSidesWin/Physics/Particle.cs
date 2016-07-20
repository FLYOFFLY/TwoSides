using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.Physics
{
    public class Particle
    {
        public Texture2D texture;
        public Vector2 pos;
        public Vector2 velocity;
        public float Angle;
        public float AngularVelocity;
        public Vector4 color;
        public float size;
        public float SizeeVel;
        public float AlphaVel;
        public int TIL;
        public Particle(Texture2D texture,Vector2 position,Vector2 velocity,float angle,float angleVelocity, Vector4 color, float size,int ttl,
            float sizeVel, float alphaVel)
        {
            this.texture = texture;
            this.pos = position;
            this.velocity = velocity;
            this.size = size;
            this.TIL = ttl;
            this.SizeeVel = sizeVel;
            this.AlphaVel = alphaVel;
            this.Angle = angle;
            this.AngularVelocity = angleVelocity;
        }
        public void update()
        {
            TIL--;
            pos += velocity;
            Angle += AngularVelocity;
            size += SizeeVel;
            color = new Vector4(color.X, color.Y, color.Z, color.W - AlphaVel);

        }
        public void draw(SpriteBatch spriteBatch)
        {
            Rectangle src = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, pos, src, new Color(color), Angle, origin, size, SpriteEffects.None, 0);
        }
    }
}
