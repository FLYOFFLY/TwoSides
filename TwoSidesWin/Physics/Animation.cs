using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.Physics
{
    public class Animation
    {
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }

        public Texture2D Sprite { get; }

        readonly float _scale;
        int _elapsedTime;
        readonly int _frameTime;
        readonly int _frameCount;
        int _currentFrame;

        readonly ColorScheme _color;
        Rectangle _srcRect;
        Rectangle _destRect;
        bool _active;
        readonly bool _looping;
        Vector2 _pos;
        public Animation(Texture2D texture,Vector2 pos,int frameWidth,int frameHeight,int frameCount,int frameTime, ColorScheme color,float scale,bool looping)
        {
            Sprite = texture;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            _frameCount = frameCount;
            _frameTime = frameTime;
            _scale = scale;
            _looping = looping;
            _pos = pos;
            _color = color;

            _elapsedTime = 0;
            _currentFrame = 0;
            _active = true;
            _srcRect = new Rectangle();
            _destRect = new Rectangle();
        }
        public void Update(GameTime gameTime)
        {
            if (!_active) return;
          
            _elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_elapsedTime >= _frameTime)
            {
                _currentFrame++;
                if (_currentFrame >= _frameCount)
                {
                    _currentFrame = 0;
                    if (!_looping) _active = false;
                }
                _elapsedTime = 0;
            }
            _srcRect = new Rectangle(_currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            _destRect = new Rectangle((int)_pos.X-(int)(FrameWidth*_scale)/2,
                (int)_pos.Y-(int)(FrameHeight*_scale)/2,
                (int)(FrameWidth*_scale),
                (int)(FrameHeight*_scale));
        }

        public void Draw(Render render)
        {
            if (_active) render.Draw(Sprite, _destRect, _srcRect, _color);
        }
    }
}
