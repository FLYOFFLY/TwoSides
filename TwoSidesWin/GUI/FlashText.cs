using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI
{
    public sealed class FlashText : Label
    {
        bool b_isUp;  
        public FlashText(Vector2 L_pos, string L_text, SpriteFont L_font, Color L_color, bool L_isUp = false) : base(L_text, L_pos, L_font,L_color)
        {
            this.b_isUp = L_isUp;
        }

        //Если невидим текст
        public bool IsInvisible()
        {
            return  color_Diffuse.A <= 2;
        }
        
        public override void Update()
        {
            if(this.b_isUp) Up(1);
            this.color_Diffuse.A -= 1;
        }

    }
}
