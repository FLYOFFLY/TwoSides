using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.Physics
{
    public class Animation
    {
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public Texture2D sprite;
        float scale;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;

        Color color;
        Rectangle srcRect;
        Rectangle destRect;
        bool active;
        bool Looping;
        Vector2 pos;
        public Animation(Texture2D texture,Vector2 pos,int frameWidth,int frameHeight,int frameCount,int frameTime,Color color,float scale,bool looping)
        {
            this.sprite = texture;
            this.FrameWidth = FrameWidth;
            this.FrameHeight = FrameHeight;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.scale = scale;
            this.Looping = looping;
            this.pos = pos;
            this.color = color;

            elapsedTime = 0;
            currentFrame = 0;
            active = true;
            srcRect = new Rectangle();
            destRect = new Rectangle();
        }
        public void Update(GameTime gameTime)
        {
            if (!active) return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime >= frameTime)
            {
                currentFrame++;
                if (currentFrame >= frameCount)
                {
                    currentFrame = 0;
                    if (!Looping) active = false;
                }
                elapsedTime = 0;
            }
            srcRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            destRect = new Rectangle((int)pos.X-(int)(FrameWidth*scale)/2,
                (int)pos.Y-(int)(FrameHeight*scale)/2,
                (int)(FrameWidth*scale),
                (int)(FrameHeight*scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active) spriteBatch.Draw(sprite, destRect, srcRect, color);
        }
    }
}
