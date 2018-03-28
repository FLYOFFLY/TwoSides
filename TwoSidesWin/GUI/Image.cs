using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TwoSides.GUI;

namespace TwoSides.GUI
{
    public sealed class Image : GuiElement
    {

        Rectangle _rect;

        [NonSerialized]
        Texture2D _image;

        public Image(Texture2D image, Rectangle rect)
        {
            _image = image;
            _rect = rect;
            SetPos(new Vector2(_rect.X, _rect.Y));
        }

        public override void  Draw(Render render)
        {
            _rect.X = (int)GetPos().X;
            _rect.Y = (int)GetPos().Y;
            if (_image == null) _image = Program.Game.Dialogtex;
            render.Start();
            render.Draw(_image, _rect);
            render.End();
        }
        public bool InHover(MouseState ms) => _rect.Contains(new Point(ms.X, ms.Y));
        public Color this[int x , int y]
        {
            get
            {
                Color[] color = new Color[_image.Width * _image.Height];
                _image.GetData(color);
                return color[x + y * _image.Width];
            }
            set
            {
                Color[] color = new Color[_image.Width * _image.Height];
                color[x + y * _image.Width] = value;
                _image.SetData(color);
            }
        }
        
    }
}
