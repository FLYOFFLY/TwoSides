using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace TwoSides.GUI
{
    public class TextFieldArgs : EventArgs
    {
        public string Text { get; set; }
        public TextFieldArgs(string Text)
        {
            this.Text = Text;
        }
    }
    sealed public class TextField : GUIElement
    {
        bool b_IsEnter = true;
        static string str_textString;
        Vector2 v2D_Offset = new Vector2(0, 0);
        static KeyboardState keystate_CurrentKeyboardState;

        [NonSerialized]
        SpriteFont font_Font;

        public delegate void OnEnter(Object sender,TextFieldArgs e);

        public event OnEnter e_OnEnter;
        [NonSerialized]
        public Color colorDiff;
        public TextField(Vector2 L_pos, SpriteFont L_font)
        {

            this.pos = L_pos;
            this.font_Font = L_font;
            str_textString = null;
            this.v2D_Offset = Vector2.Zero;
            this.colorDiff = Color.White;
        }
        public TextField(Vector2 L_pos, SpriteFont L_font,Color color)
        {
            this.pos = L_pos;
            this.font_Font = L_font;
            str_textString = null;
            this.v2D_Offset = Vector2.Zero;
            this.colorDiff = color;
        }

        public string getText()
        {
            return str_textString;
        }

        public void rightSort()
        {
            if (str_textString != null)
                this.v2D_Offset.X = -this.font_Font.MeasureString(str_textString).X;
        }

        public override void Draw(SpriteBatch L_spriteBatch)
        {
            string s = str_textString + "|";
            L_spriteBatch.Begin();

            L_spriteBatch.DrawString(this.font_Font, s,
                new Vector2(this.getPos().X + this.v2D_Offset.X, this.getPos().Y + this.v2D_Offset.Y
                    ), colorDiff);
            L_spriteBatch.End();
        }

        public void updateKey()
        {
            bool leftshift = false;
            KeyboardState oldKeyboardState = keystate_CurrentKeyboardState;
            keystate_CurrentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys;
            pressedKeys = keystate_CurrentKeyboardState.GetPressedKeys();
            if (keystate_CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) leftshift = true;
            foreach (Keys key in pressedKeys)
            {
                if (oldKeyboardState.IsKeyUp(key))
                {
                    if (key == Keys.Back && str_textString.Count() >= 1) // overflows
                        str_textString = str_textString.Remove(str_textString.Length - 1, 1);
                    else if (key == Keys.Enter && b_IsEnter)
                    {
                        if (str_textString != null && e_OnEnter != null)
                        {
                            e_OnEnter(this,new TextFieldArgs(str_textString.ToUpper()));
                            str_textString = null;
                        }
                    }
                    else if (key == Keys.Space)
                    {
                        if (str_textString != null)
                        {
                            str_textString = str_textString.Insert(str_textString.Length, " ");
                        }
                    }
                    else if (key == Keys.T || key == Keys.Q || key == Keys.W || key == Keys.E || key == Keys.R ||
                       key == Keys.Y || key == Keys.U || key == Keys.I || key == Keys.O || key == Keys.P ||
                       key == Keys.A || key == Keys.S || key == Keys.D || key == Keys.F || key == Keys.G ||
                       key == Keys.Z || key == Keys.L || key == Keys.K || key == Keys.J || key == Keys.H ||
                       key == Keys.X || key == Keys.C || key == Keys.V || key == Keys.B || key == Keys.N || key == Keys.M
                       )
                    {
                        if (leftshift) str_textString += key.ToString();
                        else str_textString += key.ToString().ToLower();
                    }
                    else if (key == Keys.D3 || key == Keys.D2 || key == Keys.D1 || key == Keys.D1 || key == Keys.D0 ||
                       key == Keys.D4 || key == Keys.D5 || key == Keys.D6 || key == Keys.D7 || key == Keys.D9 || key == Keys.D8)
                    {
                        str_textString += key.ToString().Remove(0, 1);
                    }
                    else if (key == Keys.OemQuestion)
                    {

                        str_textString += "/";
                    }
                }
            }
        }
    }
}
