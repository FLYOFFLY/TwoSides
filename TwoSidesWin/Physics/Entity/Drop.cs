using System;

using Microsoft.Xna.Framework.Graphics;

using TwoSides.World;

namespace TwoSides.Physics.Entity
{
    [Serializable]
    public sealed class Drop : DynamicEntity
    {
        readonly float _dirX;

        readonly Item _slot;
        public Drop() { }

        public Drop(Item slot, int x, int y, float dirX = 0) : base(x,y)
        {
            _slot = slot;
            _dirX = dirX;
            Velocity.Y = -2;
            Velocity.X = -dirX * 7;
        }

        public override void Update()
        {
            if (_dirX < 0 && Velocity.X > 0) Velocity.X -= 0.2f;
            else if (_dirX > 0 && Velocity.X < 0) Velocity.X += 0.2f;
            else Velocity.X = 0;
            Fail();
        }
        public Item GetSlot() => _slot;

        public void Render(SpriteBatch spriteBatch,int x,int y) => _slot.Render(spriteBatch, x, y);
    }
}
