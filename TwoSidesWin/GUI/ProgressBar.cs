using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.GUI
{
    public class ProgressBar
    {  
        int heightPresss    ;
        int rightBorder     ;
        int LeftBorder      ;
        float value         ;
        float MaxValue      ;
        int y;
        string text;
        public void Reset()
        {
            value = 0;
        }
        public Color color;
        public ProgressBar(int Height, int y,int LeftBorder,int rightBorder, float MaxValue,string text,Color color){
            this.heightPresss = Height;
            this.y = y;
            this.LeftBorder = LeftBorder;
            this.rightBorder = rightBorder;
            this.MaxValue = MaxValue;
            this.value = 0;
            this.text = text;
            this.color = color;
        }
        public void Add(float value)
        {
            this.value += value;
        }
        public bool final()
        {
            return MaxValue == value;
        }
        public void setText(string text)
        {
            this.text = text;
        }
        public void setMaxValue(int maxValue)
        {
            this.MaxValue = maxValue;
        }
        public void Render(Texture2D texture, SpriteBatch spriteBatch){;
            spriteBatch.Begin();
            if (this.text != null)
            {
                Vector2 textSize = Program.game.Font1.MeasureString(text);
                int widthProgress = rightBorder - LeftBorder;
                Program.game.DrawText(this.text, rightBorder-widthProgress/2 - (int)(textSize.X / 2), (int)(y-textSize.Y*2),color);
            } 
            spriteBatch.Draw(texture, new Rectangle(LeftBorder, y, rightBorder - LeftBorder, heightPresss), Color.White);
            spriteBatch.Draw(texture, new Rectangle(LeftBorder, y, (int)(rightBorder * (value / MaxValue)) - LeftBorder, heightPresss), Color.BlueViolet);
            spriteBatch.End();
    
        }
    }
}
