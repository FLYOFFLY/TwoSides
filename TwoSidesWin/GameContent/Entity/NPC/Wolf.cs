using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.Physics.Entity.NPC;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.World.Tile;
using Microsoft.Xna.Framework;
using TwoSides.Physics.Entity;
using TwoSides.World;

namespace TwoSides.GameContent.Entity.NPC
{
    public class Wolf : BaseNpc
    {

        float frame = 0;
       // int frame;
        public Wolf(Texture2D npcSkin) : base(new Texture2D[]{npcSkin})
        {
            width = 42;
            height = 20;
        }

        public Wolf(int blockx, Texture2D npcSkin)
            : base(blockx, new Texture2D[] { npcSkin })
        {
            to = position.X;
            to = Program.game.player.position.X * ITile.TileMaxSize;
            width = 42;
            height = 20;
        }
        public override void update()
        {
            base.update();
            if ((int)frame < 4) frame += 1 / 24.0f;
            else frame = 0;
        }
        public override void DrawNPC(SpriteEffects effect, SpriteBatch spriteBatch, SpriteFont Font1, Texture2D shadow)
        {

            effect = SpriteEffects.None;
            if (direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.DrawString(Font1, ((int)(hp)).ToString(), new Vector2((int)(position.X + (width - npcSkin[0].Width)), (int)(position.Y) - 30), Color.Black);

            Rectangle src = new Rectangle(0, 0, npcSkin[0].Width, npcSkin[0].Height / 9);
            src.Y += (int)frame * src.Height;
            Rectangle rect = new Rectangle((int)(position.X + (width - npcSkin[0].Width)),
                    (int)(position.Y), src.Width, src.Height);
            for (int i = 0; i < npcSkin.Length; i++)
            {
                spriteBatch.Draw(npcSkin[i], rect, src, Color.White,
                        0, Vector2.Zero, effect, 0);
            }
            DrawShadow(shadow, spriteBatch);
        }
        protected void attackplayer()
        {
            if (Program.game.player.rect.Intersects(rect))
            {
                if (Program.game.dimension[Program.game.currentD].rand.Next(0, 100) <= 1)
                {
                    if (!Program.game.player.slot[Player.slotmax].IsEmpty)
                    {
                        Program.game.player.slot[Player.slotmax].damageslot(Math.Max(1, 5 - Program.game.player.slot[Player.slotmax].getDef()) * Program.game.seconds);
                        if (Program.game.player.slot[Player.slotmax].HP <= 2) Program.game.player.slot[Player.slotmax] = new Item();
                    }
                    if (Program.game.player.slot[Player.slotmax].IsEmpty)
                    {

                        Program.game.player.typeKill = 0;
                    }
                }
                else
                {
                    int b = Program.game.dimension[Program.game.currentD].rand.Next(0, 2);
                    if (!Program.game.player.slot[Player.slotmax + 1].IsEmpty)
                    {
                        Program.game.player.slot[Player.slotmax + 1].damageslot(Math.Max(1, 5 - Program.game.player.slot[Player.slotmax + 1].getDef()) * Program.game.seconds);
                        if (Program.game.player.slot[Player.slotmax + 1].HP < 2) Program.game.player.slot[Player.slotmax + 1] = new Item();
                    }
                    if (!Program.game.player.slot[Player.slotmax + 2].IsEmpty)
                    {
                        Program.game.player.slot[Player.slotmax + 2].damageslot(Math.Max(1, 5 - Program.game.player.slot[Player.slotmax + 2].getDef()) * Program.game.seconds);
                        if (Program.game.player.slot[Player.slotmax + 2].HP < 2) Program.game.player.slot[Player.slotmax + 2] = new Item();
                    }
                    if (b == 0 && Program.game.player.slot[Player.slotmax + 1].IsEmpty)
                    {
                        if (Program.game.player.position.X > position.X) b = 3;
                        else b = 2;
                        Program.game.player.bloods[b] = true;
                    }
                    else if (b == 1 && Program.game.player.slot[Player.slotmax + 2].IsEmpty)
                    {
                        if (Program.game.player.position.X > position.X) b = 0;
                        else b = 1;
                        Program.game.player.bloods[b] = true;
                    }
                }
            }
        }
    }
}
