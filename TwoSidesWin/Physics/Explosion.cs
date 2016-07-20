using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.Physics
{
    public class Explosion
    {
        Animation explosionAnimation;
        Vector2 Position;
        public bool Active;
        //int timeToLive;
        public int Width
        {
            get { return explosionAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return explosionAnimation.FrameWidth; }
        }
        public Explosion(Animation anim,Vector2 pos)
        {
            this.explosionAnimation = anim;
            this.Position = pos;
            Active = true;
            //this.timeToLive = 30;
        }
        public void Update(GameTime gameTime)
        {
            this.explosionAnimation.Update(gameTime);
           // timeToLive-=1;
            //if (timeToLive < 0) Active = false;
        }
        public void Draw(SpriteBatch spriteBatch) {
            this.explosionAnimation.Draw(spriteBatch);
        }

     


    }
}
