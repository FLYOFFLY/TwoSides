using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.GUI
{
    public class RadioButton : Button 
    {
        Texture2D offTexture;
        bool on;
        public RadioButton(bool on,Texture2D onTexture, Texture2D offTexture, SpriteFont L_font, Rectangle L_rect): base(onTexture,L_font,L_rect,"")
        {
            this.offTexture = offTexture;
            e_onClicked += new onClicked(RadioButton_e_onClicked);
            this.on = on;
        }

        void RadioButton_e_onClicked(Object sender,EventArgs e)
        {
            on = !on;
        }

        void RenderButton(SpriteBatch L_spriteBatch,Color color)
        {
            if (on)
            {
                L_spriteBatch.Draw(this.tex2D_Image,
                    this.rect_Location,
                    color);
            }
            else
            {
                L_spriteBatch.Draw(this.offTexture,
                    this.rect_Location,
                    color);
            }
        }

        public override void Draw(SpriteBatch L_spriteBatch)
        {
            L_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (this.rect_Location.Contains(new Point(this.ms_NewMouse.X, this.ms_NewMouse.Y)))
            {
                RenderButton(L_spriteBatch, Color.Silver);
            }
            else
            {
                RenderButton(L_spriteBatch, Color.White);
            }


            L_spriteBatch.End();
        }
        public bool Status
        {
            get { return this.on; }
        }

    }
}
