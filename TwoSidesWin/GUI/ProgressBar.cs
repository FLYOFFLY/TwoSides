using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI
{
    public class ProgressBar
    {
        readonly int _heightPresss    ;
        readonly int _rightBorder     ;
        readonly int _leftBorder      ;
        float _value         ;
        float _maxValue      ;
        readonly int _y;
        string _text;
        public void Reset()
        {
            _value = 0;
        }

        readonly Color _color;
        public ProgressBar(int height, int y,int leftBorder,int rightBorder, float maxValue,string text,Color color){
            _heightPresss = height;
            _y = y;
            _leftBorder = leftBorder;
            _rightBorder = rightBorder;
            _maxValue = maxValue;
            _value = 0;
            _text = text;
            _color = color;
        }
        public void Add(float value)
        {
            _value += value;
        }
        public bool Final() => Math.Abs(_maxValue - _value) < float.Epsilon;

        public void SetText(string text)
        {
            _text = text;
        }
        public void SetMaxValue(int maxValue)
        {
            _maxValue = maxValue;
        }
        public void Render(Texture2D texture, Render render){
            render.Start();
            if (_text != null)
            {
                Vector2 textSize = Program.Game.Font1.MeasureString(_text);
                var widthProgress = _rightBorder - _leftBorder;
                Program.Game.DrawText(_text, _rightBorder-widthProgress/2 - (int)(textSize.X / 2), (int)(_y-textSize.Y*2),_color);
            } 
            render.Draw(texture, new Rectangle(_leftBorder, _y, _rightBorder - _leftBorder, _heightPresss), Color.White);
            render.Draw(texture, new Rectangle(_leftBorder, _y, (int)(_rightBorder * (_value / _maxValue)) - _leftBorder, _heightPresss), Color.BlueViolet);
            render.End();
    
        }
    }
}
