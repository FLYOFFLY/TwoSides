using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI
{
    public abstract class GuiElement
    {
        public Vector2 Pos;
        GuiElement _patern;
        public Vector2 GetPos()
        {
            if (_patern != null)
                return Pos + _patern.GetPos();
            return Pos;
        }
        public void SetPos(Vector2 pos) => Pos = pos;
        public void SetPatern(GuiElement patern) => _patern = patern;

        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
