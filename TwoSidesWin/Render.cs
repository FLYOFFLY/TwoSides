using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides
{
    public class ColorScheme
    {
        public static readonly ColorScheme BaseColor = new ColorScheme(Color.White);
        public static readonly ColorScheme InterfaceColor = new ColorScheme(Color.Blue);
        public static readonly ColorScheme BloodColor = new ColorScheme(Color.Red);
        public static readonly ColorScheme BaseClothesColor = new ColorScheme(Color.Red);
        public static readonly ColorScheme NotActiveRecipe = new ColorScheme(Color.Black);
        public static readonly ColorScheme ActiveRecipe = new ColorScheme(Color.Green);
        public static readonly ColorScheme BaseRace = new ColorScheme(new Color(136, 105, 75));
        public static readonly ColorScheme EuropianRace = new ColorScheme(new Color(213, 171, 123));
        public static readonly ColorScheme NiggerRace = new ColorScheme(new Color(81, 21, 21));
        public static readonly ColorScheme Shadow = new ColorScheme(Color.Black);
        public static readonly ColorScheme ProgressColor = new ColorScheme(Color.BlueViolet);
        public static readonly ColorScheme ActiveRadioColor = new ColorScheme(Color.Silver);



        static Color lightColor = Color.White;
        static Color DarkColor = Color.Black;
        public Color Color { get; }
        public ColorScheme(Color color)
        {
            Color = color;
        }

        public static ColorScheme GenColorGradient(float t)
        {
            Color result = DarkColor;
            result.R = (byte) ((1 - t) * result.R + t * lightColor.R);
            result.G = (byte)((1 - t) * result.G + t * lightColor.G);
            result.B = (byte)((1 - t) * result.B + t * lightColor.B);
            return new ColorScheme(result);
        }
    }
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



        public void Draw(string nameTexture, Vector2 pos) =>
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), pos, ColorScheme.BaseColor.Color);

        public void Draw(string nameTexture, Rectangle rect) => 
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), rect, ColorScheme.BaseColor.Color);

        public void Draw(string nameTexture, Vector2 destRect, Rectangle srcRect) => 
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), destRect, srcRect, ColorScheme.BaseColor.Color, 0, Vector2.Zero, 0, SpriteEffects.None, 0);

        public void Draw(string nameTexture, Rectangle destRect, Rectangle srcRect) => 
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), destRect, srcRect, ColorScheme.BaseColor.Color, 0.0f, Vector2.Zero, SpriteEffects.None, 0);

        public void Draw(string nameTexture, Rectangle destRect, Rectangle srcRect, SpriteEffects effect) => 
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), destRect, srcRect, ColorScheme.BaseColor.Color, 0.0f, Vector2.Zero, effect, 0);

        public void Draw(string nameTexture, Rectangle desc, SpriteEffects effect, float angle, Vector2 center) {
           Texture2D texture2D = ResourceManager.GetTexture2D(nameTexture);
            _batch.Draw(texture2D, desc, new Rectangle(0, 0, texture2D.Width, texture2D.Height),
                ColorScheme.BaseColor.Color, angle, center, effect, 0);
        }



        public void Draw(string nameTexture, Vector2 pos,ColorScheme color) =>
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), pos, color.Color);

        public void Draw(string nameTexture, Rectangle rect, ColorScheme color) =>
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), rect, color.Color);

        public void Draw(string nameTexture, Vector2 destRect, Rectangle srcRect, ColorScheme color) =>
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), destRect, srcRect, color.Color, 0, Vector2.Zero, 0, SpriteEffects.None, 0);

        public void Draw(string nameTexture, Rectangle destRect, Rectangle srcRect, ColorScheme color) =>
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), destRect, srcRect, color.Color, 0.0f, Vector2.Zero, SpriteEffects.None, 0);

        public void Draw(string nameTexture, Rectangle destRect, Rectangle srcRect, SpriteEffects effect, ColorScheme color) =>
            _batch.Draw(ResourceManager.GetTexture2D(nameTexture), destRect, srcRect, color.Color, 0.0f, Vector2.Zero, effect, 0);

        public void Draw(string nameTexture, Rectangle desc, SpriteEffects effect, float angle, Vector2 center, ColorScheme color)
        {
            Texture2D texture2D = ResourceManager.GetTexture2D(nameTexture);
            _batch.Draw(texture2D, desc, new Rectangle(0, 0, texture2D.Width, texture2D.Height),color.Color, angle, center, effect, 0);
        }



        public void DrawString(SpriteFont font, string toString, Vector2 vector2) =>
            _batch.DrawString(font, toString, vector2, Color.White);
        public void DrawString(SpriteFont font, string toString, Vector2 vector2, Color white) => 
            _batch.DrawString(font,toString,vector2,white);

        public void End() => _batch.End();

    }
}
