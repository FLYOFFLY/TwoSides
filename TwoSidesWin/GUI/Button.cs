using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace TwoSides.GUI
{
    public class Button : GUIElement
    {
        //Переменные
        bool b_IsClicked = false;
        string str_Text;

        //Объекты
        protected SpriteFont font_Font;
        protected Vector2 v2D_TextLocation;

        //Объекты NS
        [NonSerialized]
        protected MouseState ms_NewMouse;
        [NonSerialized]
        MouseState ms_OldMouse;
        [NonSerialized]
        protected Rectangle rect_Location;

        //Делегаты
        public delegate void onClicked(Object sender, EventArgs e);

        //События
        public event onClicked e_onClicked;

        //Текстуры NS
        [NonSerialized]
        protected Texture2D tex2D_Image;

        public Button(Texture2D L_texture, SpriteFont L_font, Rectangle L_rect, string L_text)
        {
            this.tex2D_Image = L_texture;
            this.font_Font = L_font;
            this.rect_Location = L_rect;
            setPos(new Vector2(this.rect_Location.X, (int)this.rect_Location.Y));
            this.Text = L_text;
        }

        public bool IsClicked()
        {
            return b_IsClicked;
        }

        public string Text
        {
            get { return this.str_Text; }
            set
            {
                this.str_Text = value;
                Vector2 size = font_Font.MeasureString(str_Text);
                this.v2D_TextLocation = new Vector2();
                this.v2D_TextLocation.Y = getPos().Y + ((rect_Location.Height / 2) - (size.Y / 2));
                this.v2D_TextLocation.X = getPos().X + ((rect_Location.Width / 2) - (size.X / 2));
            }
        }

        public void SetRect(Rectangle L_rect){
            this.rect_Location = L_rect;
            setPos(new Vector2(this.rect_Location.X, this.rect_Location.Y));
            this.Text = str_Text;
        }


        public override void Update()
        {
            this.rect_Location.X = (int)getPos().X;
            this.rect_Location.Y = (int)getPos().Y;
            this.ms_NewMouse = Mouse.GetState();
            if (this.b_IsClicked = true && this.ms_OldMouse.LeftButton == ButtonState.Released)
            {
                this.b_IsClicked = false;
            }
            if (this.ms_NewMouse.LeftButton == ButtonState.Released && this.ms_OldMouse.LeftButton == ButtonState.Pressed)
            {
                if (this.rect_Location.Contains(new Point(this.ms_NewMouse.X, this.ms_NewMouse.Y)))
                {
                    this.b_IsClicked = true;
                    if (this.e_onClicked != null) this.e_onClicked(this,null);
                }
            }

            this.ms_OldMouse = this.ms_NewMouse;
        }

        public override void Draw(SpriteBatch L_spriteBatch)
        {
            if (this.tex2D_Image == null) this.tex2D_Image = Program.game.button;
            if (this.font_Font == null) this.font_Font = Program.game.Font1;
            L_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Rectangle button = 
                    new Rectangle(0,0,tex2D_Image.Width,tex2D_Image.Height/2);
            if (this.rect_Location.Contains(new Point(this.ms_NewMouse.X, this.ms_NewMouse.Y)))
            {
                button.Y += button.Height;
            }

            L_spriteBatch.Draw(this.tex2D_Image,
                this.rect_Location, button, Color.White);
            L_spriteBatch.DrawString(this.font_Font,
                this.str_Text,
                this.v2D_TextLocation,
                Color.Black);


            L_spriteBatch.End();
        }


    }
}
