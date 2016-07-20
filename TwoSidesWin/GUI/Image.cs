using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.GUI
{
    sealed public class Image : GUIElement
    {

        Rectangle rect_IMGRECT;

        [NonSerialized]
        Texture2D tex2D_IMG;

        public Image(Texture2D L_img, Rectangle L_rect)
        {
            this.tex2D_IMG = L_img;
            this.rect_IMGRECT = L_rect;
            setPos(new Vector2(rect_IMGRECT.X, rect_IMGRECT.Y));
        }

        public override void  Draw(SpriteBatch L_spriteBatch)
        {
            this.rect_IMGRECT.X = (int)getPos().X;
            this.rect_IMGRECT.Y = (int)getPos().Y;
            if (this.tex2D_IMG == null) this.tex2D_IMG = Program.game.dialogtex;
            L_spriteBatch.Begin();
            L_spriteBatch.Draw(this.tex2D_IMG, this.rect_IMGRECT, Color.White);
            L_spriteBatch.End();
        }
    }
}
