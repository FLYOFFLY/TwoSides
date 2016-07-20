using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI
{
    public class GUIElement
    {
        public Vector2 pos;
        GUIElement patern;
        public Vector2 getPos()
        {
            if (patern != null)
                return pos + patern.getPos();
            return pos;
        }
        public void setPos(Vector2 pos)
        {
            this.pos = pos;
        }
        public void setPatern(GUIElement patern)
        {
            this.patern = patern;
        }

        public virtual void Update() { }
        public virtual void Draw(SpriteBatch L_spriteBatch) { }
    }
}
