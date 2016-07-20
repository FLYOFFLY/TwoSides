using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.World;

namespace TwoSides.Physics.Entity
{
    [Serializable]
    sealed public class Drop : CEntity
    {
        float dirx;

        Item slot;
        public Drop() { }

        public Drop(Item slot, int x, int y, float dirx = 0) : base(x,y)
        {
            this.slot = slot;
            this.dirx = dirx; ;
            this.velocity.Y = -2;
            this.velocity.X = -dirx * 7;
        }

        public override void update()
        {
            if (dirx < 0 && velocity.X > 0) velocity.X -= 0.2f;
            else if (dirx > 0 && velocity.X < 0) velocity.X += 0.2f;
            else velocity.X = 0;
            fail();
        }
        public Item getslot()
        {
            return slot;
        }


        public void render(SpriteBatch spriteBatch,int x,int y)
        {
            slot.Render(spriteBatch, x, y);
        }
    }
}
