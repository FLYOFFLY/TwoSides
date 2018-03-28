using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides
{
    public class Render
    {
        SpriteBatch _batch;
        public void SetBatch(SpriteBatch batch)
        {
            _batch = batch;
        }

        public void Start() => _batch.Begin();
        public void Start(SamplerState sampler) => _batch.Begin(samplerState: sampler);
        public void Start(SpriteSortMode sortMode, BlendState blendState) => _batch.Begin(sortMode, blendState);

        public void Start(Camera camera, GraphicsDeviceManager device) =>
            _batch.Begin(transformMatrix: camera.GetViewTran(device));
        public void Start(SamplerState sampler, Camera camera, GraphicsDeviceManager device) => 
            _batch.Begin(samplerState: sampler,transformMatrix:camera.GetViewTran(device));

        public void Draw(Texture2D image, Rectangle rect) => _batch.Draw(image, rect, Color.White);
        public void Draw(Texture2D texture2D, Vector2 pos, Color color) => _batch.Draw(texture2D, pos, color);
        public void Draw(Texture2D texture, Rectangle destRect, Color color) => _batch.Draw(texture, destRect, color);
        public void Draw(Texture2D texture,Rectangle destRect,Rectangle srcRect,Color color)=>_batch.Draw(texture, destRect, srcRect, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
        public void Draw(Texture2D texture, Vector2 pos, Rectangle srcRect, Color color) => _batch.Draw(texture, pos, srcRect, color);
        public void Draw(Texture2D texture, Rectangle destRect, Rectangle srcRect, Color color, SpriteEffects effect)=>_batch.Draw(texture, destRect, srcRect, color, 0.0f, Vector2.Zero, effect, 0);
        public void Draw(Texture2D texture2D, Rectangle desc, SpriteEffects effect, float angle, Vector2 center) => _batch.Draw(texture2D,desc, new Rectangle(0, 0, texture2D.Width, texture2D.Height), Color.White, angle, center, effect, 0);
        public void Draw(Texture2D texture, Vector2 destRect, Rectangle srcRect) => _batch.Draw(texture, destRect, srcRect, Color.White, 0, Vector2.Zero, 0, SpriteEffects.None, 0);

        public void DrawString(SpriteFont font, string toString, Vector2 vector2) =>_batch.DrawString(font, toString, vector2, Color.White);
        public void DrawString(SpriteFont font, string toString, Vector2 vector2, Color white) => _batch.DrawString(font,toString,vector2,white);

        public void End() => _batch.End();

    }
}
