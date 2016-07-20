using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.Physics.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.GameContent.GenerationResources;

namespace TwoSides.Physics
{
    public class Cloud : CEntity
    {
        public Cloud(Vector2 pos) : base(pos) {
            velocity.X = -3;
        }
        public override void update()
        {
            position.X += velocity.X;
            if (position.X <= 0) position.X =SizeGeneratior.WorldWidth-5;
            base.update();
        }
    }
}
