using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI
{
    public class Label : GUIElement
    {
        
        string str_Text;

        protected Color color_Diffuse; //Цвет

        [NonSerialized]
        SpriteFont font_Font;

        public Label(string L_text, Vector2 L_pos, SpriteFont L_font)
        {
            this.str_Text =         L_text;
            this.pos = L_pos;
            this.font_Font = L_font;
            this.color_Diffuse = Color.Black;
        }

        public Label(string L_text, Vector2 L_pos,SpriteFont L_font,Color L_color)
        {
            this.str_Text =         L_text;
            this.pos                = L_pos;
            this.font_Font =        L_font;
            this.color_Diffuse = L_color;
        }

        public string GetText()
        {
            return str_Text;
        }

        public void Up(int y)
        {
           this.pos.Y -= y;
        }

        public void SetText(string L_text){
            this.str_Text = L_text;
        }

        public override void  Draw(SpriteBatch L_spriteBatch)
        {
            L_spriteBatch.Begin();
            L_spriteBatch.DrawString(font_Font,
                str_Text,
                this.getPos(),
                color_Diffuse);

            L_spriteBatch.End();
        }

    }
}
