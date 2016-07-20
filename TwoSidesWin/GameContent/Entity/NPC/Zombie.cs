using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.Physics.Entity.NPC;
using TwoSides;
using TwoSides.Physics.Entity;
using TwoSides.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.World.Tile;
using TwoSides.Utils;

namespace TwoSides.GameContent.Entity.NPC
{
    public class Zombie : BaseCivNpc
    {
        public Zombie() 
        {
        }

        public Zombie(int blockx):base(blockx)
        {
        }

        public Zombie(Vector2 positions):base(positions)
        {
        }

        public Zombie(int blockx, Race race, Clothes[] clslot, Color[] color):base(blockx,race,clslot,color)
        {
        }

        public Zombie(Vector2 positions, Race race, Clothes[] clslot, Color[] color) : base(positions,race,clslot,color) { }
        public virtual void DrawNPC(SpriteEffects effect, SpriteBatch spriteBatch, SpriteFont Font1, Texture2D head, Texture2D head2,
            Texture2D body, Texture2D legs, Texture2D blood, Texture2D eye, Texture2D shadow)
        {
            effect = SpriteEffects.None;
            if (direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.DrawString(Font1, ((int)(hp)).ToString(), new Vector2((int)(position.X + (width - head.Width)), (int)(position.Y) - 30), Color.Black);
            int hairid = cl[0].getid();
            int shirtid = cl[1].getid();
            int pantsid = cl[2].getid();
            int shoesid = cl[3].getid();
            int beltid = cl[4].getid();
            int glovesid = cl[5].getid();
            Rectangle rect = new Rectangle((int)(position.X + (width - head.Width)),
                    (int)(position.Y), head.Width, head.Height);
            Rectangle src = new Rectangle(0, 0, head.Width, head.Height);
            spriteBatch.Draw(head, rect, src, race.getZombieColor(),
                    0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(eye, rect, src, Color.White,
                    0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(body, rect, src,
                race.getZombieColor(), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(legs,
               rect, src,
                    race.getZombieColor(), 0, Vector2.Zero, effect, 0);
            if (hairid != -1) spriteBatch.Draw(Clothes.hair[hairid], rect, src, Color.Black, 0, Vector2.Zero, effect, 0);
            if (shirtid != -1) spriteBatch.Draw(Clothes.shirt[shirtid], rect, src, colors[0], 0, Vector2.Zero, effect, 0);
            if (pantsid != -1) spriteBatch.Draw(Clothes.pants[pantsid], rect, src, colors[1], 0, Vector2.Zero, effect, 0);
            if (shoesid != -1) spriteBatch.Draw(Clothes.shoes[shoesid], rect, src, colors[2], 0, Vector2.Zero, effect, 0);
            if (beltid != -1) spriteBatch.Draw(Clothes.belt[beltid], rect, src, colors[3], 0, Vector2.Zero, effect, 0);
            if (glovesid != -1) spriteBatch.Draw(Clothes.gloves[glovesid], rect, src, colors[4], 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(blood, rect, src,
               Color.White, 0, Vector2.Zero, effect, 0);
            DrawShadow(shadow, spriteBatch);
        }

        protected override void aiupdate()
        {
            if (Util.directional((int)Program.game.player.position.X / ITile.TileMaxSize, (int)position.X / ITile.TileMaxSize, 5) &&
                Util.directional((int)Program.game.player.position.Y / ITile.TileMaxSize, (int)position.Y / ITile.TileMaxSize, 5))
            {
                to = Program.game.player.position.X;
            }
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
                        Program.game.player.zombie = true;
                    }
                    else if (b == 1 && Program.game.player.slot[Player.slotmax + 2].IsEmpty)
                    {
                        if (Program.game.player.position.X > position.X) b = 0;
                        else b = 1;
                        Program.game.player.bloods[b] = true;
                        Program.game.player.zombie = true;
                    }
                }
            }
        }
    }
}
