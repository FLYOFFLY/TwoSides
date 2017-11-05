using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.Physics.Entity;

namespace TwoSides
{
    public class Dust : DynamicEntity
    {
        public static Texture2D DustTexture;
        float _time;
        int _frame;
        readonly int _id;
        bool _isDestory;
        public static void LoadContent()
        {
            DustTexture = Program.Game.Content.Load<Texture2D>(Game1.ImageFolder+ @"tiles\dust");
        }
        public Dust(Vector2 position,Random rand)
        {
            position.X += rand.Next(-4, 4);
            position.Y += rand.Next(-4, 4);
            _id = 0;
            _frame = 0;
            _isDestory = false;
        }
        public void Render(SpriteBatch spriteBatch)
        {
            if (_isDestory) return;
            spriteBatch.Draw(DustTexture, Position, new Rectangle(8*_id,8*_frame,8,8),Color.White,0,Vector2.Zero,0,SpriteEffects.None,0);
        }
        public override void Update()
        {
            if (_isDestory) return;
            _time += Program.Game.Seconds;
            if (_time >= 1)
            {
                if (_frame >= 2) _isDestory = true;
                _frame++;
                _time = 0;
            } 
            if (Velocity.Y < 1) Velocity.Y++;
            Position += Velocity;
        }
    }
}
