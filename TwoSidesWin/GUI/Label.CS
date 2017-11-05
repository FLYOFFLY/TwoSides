using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.Utils;
using TwoSides.GUI;

namespace TwoSides.GUI
{
    public class Label : GuiElement
    {
        
        string _text;

        protected Color Color; //Цвет

        [NonSerialized] readonly SpriteFont _font;

        public Label(string text, Vector2 pos, SpriteFont font)
        {
            _text = Localisation.GetName(text);
            Pos = pos;
            _font = font;
            Color = Color.Black;
        }

        public Label(string text, Vector2 pos,SpriteFont font,Color color)
        {
            _text = Localisation.GetName(text);
            Pos                = pos;
            _font =        font;
            Color = color;
        }

        public string GetText() => _text;

        public void Up(int y)
        {
           Pos.Y -= y;
        }

        public void SetText(string text)
        {
            _text = Localisation.GetName(text);
        }

        public override void  Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(_font,
                _text,
                GetPos(),
                Color);

            spriteBatch.End();
        }

    }
}
