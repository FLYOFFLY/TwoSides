using Microsoft.Xna.Framework;

using TwoSides.GameContent.GenerationResources;
using TwoSides.Physics.Entity;

namespace TwoSides.Physics
{
    public class Cloud : DynamicEntity
    {
        public Cloud(Vector2 pos) : base(pos) => Velocity.X = -3;

        public override void Update()
        {
            Position.X += Velocity.X;
            if (Position.X <= 0) Position.X =SizeGeneratior.WorldWidth-5;
            base.Update();
        }
    }
}
