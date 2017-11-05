using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.Physics.Entity.NPC;
using TwoSides.Utils;
using TwoSides.World;
using TwoSides.World.Tile;

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

        public Zombie(int blockx, Race race, IReadOnlyList<Clothes> clslot, Color[] color):base(blockx,race,clslot,color)
        {
        }

        public Zombie(Vector2 positions, Race race, IReadOnlyList<Clothes> clslot, Color[] color) : base(positions,race,clslot,color) { }
        public virtual void RenderNpc( SpriteBatch spriteBatch, SpriteFont font1, Texture2D head, Texture2D head2,
            Texture2D body, Texture2D legs, Texture2D blood, Texture2D eye,Texture2D hand, Texture2D shadow)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.DrawString(font1, ((int)Hp).ToString(CultureInfo.CurrentCulture), new Vector2((int)(Position.X + (Width - head.Width)), (int)Position.Y - 30), Color.Black);
            Rect = new Rectangle((int)(Position.X + (Width - head.Width)),
                    (int)Position.Y, head.Width, head.Height);
            Rectangle src = new Rectangle(0, 0, head.Width, head.Height);

            RenderLeft(hand, spriteBatch);
            spriteBatch.Draw(head, Rect, src, Race.GetZombieColor(),
                    0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(eye, Rect, src, Color.White,
                    0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(body, Rect, src,
                Race.GetZombieColor(), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(legs,
               Rect, src,
                    Race.GetZombieColor(), 0, Vector2.Zero, effect, 0);
            ClothesRender(effect,spriteBatch,Rect,src);
            spriteBatch.Draw(blood, Rect, src,
               Color.White, 0, Vector2.Zero, effect, 0);
            DrawShadow(shadow, spriteBatch);
        }

        protected override void UpdateAi()
        {
            if (Tools.Distance((int)Program.Game.Player.Position.X / Tile.TileMaxSize, (int)Position.X / Tile.TileMaxSize, 5) &&
                Tools.Distance((int)Program.Game.Player.Position.Y / Tile.TileMaxSize, (int)Position.Y / Tile.TileMaxSize, 5))
            {
                WayPoint = Program.Game.Player.Position.X;
            }
            base.UpdateAi();
        }

        protected void AttackPlayer()
        {
            if ( !Program.Game.Player.Rect.Intersects(Rect) ) return;

            if ( Program.Game.Dimension[Program.Game.CurrentDimension].Rand.Next(0 , 100) <= 1 )
            {
                if ( !Program.Game.Player.Slot[Player.Slotmax].IsEmpty )
                {
                    Program.Game.Player.Slot[Player.Slotmax]
                           .DamageSlot(Math.Max(1 , 5 - Program.Game.Player.Slot[Player.Slotmax].GetDef()) *
                                       Program.Game.Seconds);
                    if ( Program.Game.Player.Slot[Player.Slotmax].Hp <= 2 )
                        Program.Game.Player.Slot[Player.Slotmax] = new Item();
                }
                if ( Program.Game.Player.Slot[Player.Slotmax].IsEmpty )
                {

                    Program.Game.Player.TypeKill = 0;
                }
            }
            else
            {
                int b = Program.Game.Dimension[Program.Game.CurrentDimension].Rand.Next(0 , 2);
                if ( !Program.Game.Player.Slot[Player.Slotmax + 1].IsEmpty )
                {
                    Program.Game.Player.Slot[Player.Slotmax + 1]
                           .DamageSlot(Math.Max(1 , 5 - Program.Game.Player.Slot[Player.Slotmax + 1].GetDef()) *
                                       Program.Game.Seconds);
                    if ( Program.Game.Player.Slot[Player.Slotmax + 1].Hp < 2 )
                        Program.Game.Player.Slot[Player.Slotmax + 1] = new Item();
                }
                if ( !Program.Game.Player.Slot[Player.Slotmax + 2].IsEmpty )
                {
                    Program.Game.Player.Slot[Player.Slotmax + 2]
                           .DamageSlot(Math.Max(1 , 5 - Program.Game.Player.Slot[Player.Slotmax + 2].GetDef()) *
                                       Program.Game.Seconds);
                    if ( Program.Game.Player.Slot[Player.Slotmax + 2].Hp < 2 )
                        Program.Game.Player.Slot[Player.Slotmax + 2] = new Item();
                }

                if ( b != 0 && b != 1 || !Program.Game.Player.Slot[Player.Slotmax + 1 + b].IsEmpty ) return;

                if ( b == 0 )
                    b = Program.Game.Player.Position.X > Position.X ? 3 : 2;
                else
                    b = Program.Game.Player.Position.X > Position.X ? 0 : 1;
                Program.Game.Player.Bloods[b] = true;
                Program.Game.Player.Zombie = true;
            }
        }
    }
}
