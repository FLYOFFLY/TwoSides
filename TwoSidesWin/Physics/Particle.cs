using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.Physics
{
    public class Particle
    {
        public Texture2D Texture;
        public Vector2 Pos;
        public Vector2 Velocity;
        public float Angle;
        public float AngularVelocity;
        public Vector4 Color;
        public float Size;
        public float SizeeVel;
        public float AlphaVel;
        public int CountMaxUpdate;
        public Particle(Texture2D texture,Vector2 position,Vector2 velocity,float angle,float angleVelocity, Vector4 color, float size,int countMaxUpdate,
            float sizeVel, float alphaVel)
        {
            Texture = texture;
            Pos = position;
            Velocity = velocity;
            Size = size;
            CountMaxUpdate = countMaxUpdate;
            SizeeVel = sizeVel;
            AlphaVel = alphaVel;
            Angle = angle;
            AngularVelocity = angleVelocity;
        }
        public void Update()
        {
            CountMaxUpdate--;
            Pos += Velocity;
            Angle += AngularVelocity;
            Size += SizeeVel;
            Color = new Vector4(Color.X, Color.Y, Color.Z, Color.W - AlphaVel);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle src = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f);
            spriteBatch.Draw(Texture, Pos, src, new Color(Color), Angle, origin, Size, SpriteEffects.None, 0);
        }
    }
}
