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

        public Zombie(int blockx, Race race, IReadOnlyList<Clothes> clslot, ColorScheme[] color):base(blockx,race,clslot,color)
        {
        }

        public Zombie(Vector2 positions, Race race, IReadOnlyList<Clothes> clslot, ColorScheme[] color) : base(positions,race,clslot,color) { }
        public virtual void RenderNpc( Render render, SpriteFont font1, Texture2D head, Texture2D head2,
            Texture2D body, Texture2D legs, Texture2D blood, Texture2D eye,Texture2D hand, Texture2D shadow)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            render.DrawString(font1, ((int)Hp).ToString(CultureInfo.CurrentCulture), new Vector2((int)(Position.X + (Width - head.Width)), (int)Position.Y - 30), Color.Black);
            Rect = new Rectangle((int)(Position.X + (Width - head.Width)),
                    (int)Position.Y, head.Width, head.Height);
            Rectangle src = new Rectangle(0, 0, head.Width, head.Height);

            RenderLeft(hand, render);
            render.Draw(head, Rect, src,effect, Race.GetZombieColor());
            render.Draw(eye, Rect, src, effect);
            render.Draw(body, Rect, src,effect, Race.GetZombieColor());
            render.Draw(legs, Rect, src,  effect, Race.GetZombieColor());
            ClothesRender(effect,render,Rect,src);
            render.Draw(blood, Rect, src, effect);
            DrawShadow(shadow, render);
        }

        protected override void UpdateAi()
        {
            if (Tools.Distance((int)Program.Game.Player.Position.X / Tile.TILE_MAX_SIZE, (int)Position.X / Tile.TILE_MAX_SIZE, 5) &&
                Tools.Distance((int)Program.Game.Player.Position.Y / Tile.TILE_MAX_SIZE, (int)Position.Y / Tile.TILE_MAX_SIZE, 5))
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
                if ( !Program.Game.Player.Slot[Player.SLOTMAX].IsEmpty )
                {
                    Program.Game.Player.Slot[Player.SLOTMAX]
                           .DamageSlot(Math.Max(1 , 5 - Program.Game.Player.Slot[Player.SLOTMAX].GetDef()) *
                                       Program.Game.Seconds);
                    if ( Program.Game.Player.Slot[Player.SLOTMAX].Hp <= 2 )
                        Program.Game.Player.Slot[Player.SLOTMAX] = new Item();
                }
                if ( Program.Game.Player.Slot[Player.SLOTMAX].IsEmpty )
                {

                    Program.Game.Player.TypeKill = 0;
                }
            }
            else
            {
                var b = Program.Game.Dimension[Program.Game.CurrentDimension].Rand.Next(0 , 2);
                if ( !Program.Game.Player.Slot[Player.SLOTMAX + 1].IsEmpty )
                {
                    Program.Game.Player.Slot[Player.SLOTMAX + 1]
                           .DamageSlot(Math.Max(1 , 5 - Program.Game.Player.Slot[Player.SLOTMAX + 1].GetDef()) *
                                       Program.Game.Seconds);
                    if ( Program.Game.Player.Slot[Player.SLOTMAX + 1].Hp < 2 )
                        Program.Game.Player.Slot[Player.SLOTMAX + 1] = new Item();
                }
                if ( !Program.Game.Player.Slot[Player.SLOTMAX + 2].IsEmpty )
                {
                    Program.Game.Player.Slot[Player.SLOTMAX + 2]
                           .DamageSlot(Math.Max(1 , 5 - Program.Game.Player.Slot[Player.SLOTMAX + 2].GetDef()) *
                                       Program.Game.Seconds);
                    if ( Program.Game.Player.Slot[Player.SLOTMAX + 2].Hp < 2 )
                        Program.Game.Player.Slot[Player.SLOTMAX + 2] = new Item();
                }

                if ( b != 0 && b != 1 || !Program.Game.Player.Slot[Player.SLOTMAX + 1 + b].IsEmpty ) return;

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
