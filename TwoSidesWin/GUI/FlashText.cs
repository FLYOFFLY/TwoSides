using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI
{
    public sealed class FlashText : Label
    {
        readonly bool _isFly;  
        public FlashText(Vector2 pos, string text, SpriteFont font, Color color, bool isFly = false) : base(text, pos, font,color) => _isFly = isFly;

        //Если невидим текст
        public bool IsInvisible() => Color.A <= 2;

        public override void Update()
        {
            if(_isFly) Up(1);
            Color.A -= 1;
        }

    }
}
