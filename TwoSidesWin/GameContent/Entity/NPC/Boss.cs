using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.Physics.Entity.NPC;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.World;

namespace TwoSides.GameContent.Entity.NPC
{
    [Serializable]
    sealed public class Boss:Zombie
    {
        public void setSetting()
        {
            hp = 10;

            type = 5;
            maxRunSpeed = 1.0f;
            runAcceleration = 0.16f;
            runSlowdown = 0.1f;
            jumpHeight = 30;
            jumpSpeed = 10.01f;
        }
        public Boss(int blockx)
            : base(blockx)
        {
            setSetting();
        }
        public Boss(Vector2 positions)
            : base(positions)
        {
            setSetting();
        }

         public override void DrawNPC(SpriteEffects effect, SpriteBatch spriteBatch, SpriteFont Font1, Texture2D head, Texture2D head2,
              Texture2D body, Texture2D legs, Texture2D blood, Texture2D eye, Texture2D shadow)
        {
            effect = SpriteEffects.None;
            if (direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.DrawString(Font1, ((int)(hp)).ToString(), new Vector2((int)(position.X + (width - head.Width)), (int)(position.Y) - 30), Color.Black);
                spriteBatch.Draw(head2, new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height),Color.White , 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(body, new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(legs, new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(Clothes.shirt[1], new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(Clothes.pants[0], new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(Clothes.shoes[0], new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red, 0, Vector2.Zero, effect, 0);
            DrawShadow(shadow, spriteBatch);
        }
        public new void kill() 
        {
            foreach (Item slots in drop)
            {
                Program.game.adddrop((int)position.X, (int)position.Y, slots);
            }
            Program.game.player.bosskil();
        }

    }
}
