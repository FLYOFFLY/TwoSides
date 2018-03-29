using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TwoSides.Utils;

using TwoSides.GUI;

namespace TwoSides.GUI
{
    public class Button : GuiElement
    {
        //Переменные
        bool _isClicked;
        string _text;

        //Объекты
        protected SpriteFont Font;
        protected Vector2 TextLocation;

        //Объекты NS
        [NonSerialized]
        protected MouseState MouseStateNew;
        [NonSerialized]
        MouseState _mouseStateOld;
        [NonSerialized]
        protected Rectangle Area;

        //Делегаты

        //События
        public event EventHandler<EventArgs> OnClicked;

        //Текстуры NS
        [NonSerialized]
        protected Texture2D Image;
        public Button(Texture2D image, SpriteFont font, Rectangle area, string text)
        {
            Image = image;
            Font = font;
            Area = area;
            SetPos(new Vector2(Area.X, Area.Y));
            Text = Localisation.GetName(text);
        }

        bool IsClicked() => _isClicked;

        public string Text
        {
            get => _text;
            set
            {
                _text = Localisation.GetName(value);
                Vector2 size = Font.MeasureString(_text);
                TextLocation = new Vector2(GetPos().X + (Area.Width / 2.0f - size.X / 2.0f) ,
                                           GetPos().Y + (Area.Height / 2.0f - size.Y / 2.0f));
            }
        }

        public void SetRect(Rectangle area){
            Area = area;
            SetPos(new Vector2(Area.X, Area.Y));
            Text = _text;
        }


        public override void Update()
        {
            Area.X = (int)GetPos().X;
            Area.Y = (int)GetPos().Y;
            MouseStateNew = Mouse.GetState();
            if (_isClicked && _mouseStateOld.LeftButton == ButtonState.Released)
                _isClicked = false;
            if ( MouseStateNew.LeftButton == ButtonState.Released && _mouseStateOld.LeftButton == ButtonState.Pressed &&
                 Area.Contains(new Point(MouseStateNew.X , MouseStateNew.Y)) )
            {
                _isClicked = true;
                OnClicked?.Invoke(this , null);
            }

            _mouseStateOld = MouseStateNew;
        }

        public override void Draw(Render render)
        {
            if (Image == null) Image = Program.Game.Button;
            if (Font == null) Font = Program.Game.Font1;
            render.Start(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Rectangle button = 
                    new Rectangle(0,0,Image.Width,Image.Height/2);
            if (Area.Contains(new Point(MouseStateNew.X, MouseStateNew.Y)))
            {
                button.Y += button.Height;
            }

            render.Draw(Image, Area, button);
            render.DrawString(Font,
                _text,
                TextLocation,
                Color.Black);


            render.End();
        }


    }
}
