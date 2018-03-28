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

        void RenderButton(Render render,Color color)
        {
            render.Draw(Status ? Image : _offTexture ,
                               Area ,
                               color);
        }

        public override void Draw(Render render)
        {
            render.Start(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            RenderButton(render ,
                         Area.Contains(new Point(MouseStateNew.X , MouseStateNew.Y)) ? Color.Silver : Color.White);

            render.End();
        }
        public bool Status { get; set; }
    }
}
