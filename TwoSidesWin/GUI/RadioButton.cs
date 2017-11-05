using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI
{
    public class RadioButton : Button 
    {
        readonly Texture2D _offTexture;

        public RadioButton(bool on,Texture2D onTexture, Texture2D offTexture, SpriteFont font, Rectangle area): base(onTexture,font,area,"")
        {
            _offTexture = offTexture;
            OnClicked += (o , args) => Status = !Status;
            Status = on;
        }

        void RenderButton(SpriteBatch spriteBatch,Color color)
        {
            spriteBatch.Draw(Status ? Image : _offTexture ,
                               Area ,
                               color);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            RenderButton(spriteBatch ,
                         Area.Contains(new Point(MouseStateNew.X , MouseStateNew.Y)) ? Color.Silver : Color.White);

            spriteBatch.End();
        }
        public bool Status { get; set; }
    }
}
