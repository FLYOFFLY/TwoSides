using System;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TwoSides.GUI
{
    public class TextFieldArgs : EventArgs
    {
        public string Text { get; set; }
        public TextFieldArgs(string text) => Text = text;
    }
    public sealed class TextField : GuiElement
    {
        static string _text;
        Vector2 _offset;
        static KeyboardState _keystateCurrentKeyboardState;

        [NonSerialized] readonly SpriteFont _font;
        
        public event EventHandler<TextFieldArgs> OnEnter;
        [NonSerialized] readonly Color _color;
        public TextField(Vector2 pos, SpriteFont font)
        {
            Pos = pos;
            _font = font;
            _text = null;
            _offset = Vector2.Zero;
            _color = Color.White;
        }
        public TextField(Vector2 pos, SpriteFont font,Color color)
        {
            Pos = pos;
            _font = font;
            _text = null;
            _offset = Vector2.Zero;
            _color = color;
        }

        public string GetText() => _text;

        public void RightSort()
        {
            if (_text != null)
                _offset.X = -_font.MeasureString(_text).X;
        }

        public override void Draw(Render render)
        {
            render.Start();

            render.DrawString(_font, $"{_text}|",
                new Vector2(GetPos().X + _offset.X, GetPos().Y + _offset.Y
                    ), _color);
            render.End();
        }
        public override void Update()
        {
            UpdateKey();
            base.Update();
        }
       void UpdateKey()
        {
            var leftshift = false;
            KeyboardState oldKeyboardState = _keystateCurrentKeyboardState;
            _keystateCurrentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys = _keystateCurrentKeyboardState.GetPressedKeys();
            if (_keystateCurrentKeyboardState.IsKeyDown(Keys.LeftShift)) leftshift = true;
            foreach (Keys key in pressedKeys)
            {
                if (oldKeyboardState.IsKeyDown(key) ) continue;

                var keyText = key.ToString();
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (key == Keys.Back && _text?.Length >= 1) // overflows
                    _text = _text.Remove(_text.Length - 1, 1);
                else if (key == Keys.Enter)
                {
                    if ( _text == null ) continue;

                    OnEnter?.Invoke(this,new TextFieldArgs(_text.ToUpper(CultureInfo.CurrentCulture)));
                    _text = null;
                }
                else if (key == Keys.Space)
                {
                    _text = _text?.Insert(_text.Length, " ");
                }
                else if (IsChar(key))
                {
                    if (leftshift) _text += keyText;
                    else _text += keyText.ToLower(CultureInfo.CurrentCulture);
                }
                else if ( IsNum(key))
                {
                    _text += keyText.Remove(0, 1);
                }
                else if (key == Keys.OemQuestion)
                {

                    _text += "/";
                }
            }
        }

        static bool IsNum(Keys key) => key == Keys.D3 || key == Keys.D2 || key == Keys.D1  || key == Keys.D0 ||
                                       key == Keys.D4 || key == Keys.D5 || key == Keys.D6 || key == Keys.D7 || key == Keys.D9 || key == Keys.D8;

        static bool IsChar(Keys key) => key >= Keys.A && key <= Keys.Z ;
    }
}
