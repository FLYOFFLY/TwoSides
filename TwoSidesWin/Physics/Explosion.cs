using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.Physics
{
    public class Explosion
    {
        readonly Animation _explosionAnimation;
        public bool Active;
        //int timeToLive;
        public int Width => _explosionAnimation.FrameWidth;
        public int Height => _explosionAnimation.FrameWidth;

        public Explosion(Animation anim,Vector2 pos)
        {
            _explosionAnimation = anim;
            Active = true;
            //this.timeToLive = 30;
        }
        public void Update(GameTime gameTime)
        {
            _explosionAnimation.Update(gameTime);
           // timeToLive-=1;
            //if (timeToLive < 0) Active = false;
        }
        public void Draw(SpriteBatch spriteBatch) {
            _explosionAnimation.Draw(spriteBatch);
        }

     


    }
}
