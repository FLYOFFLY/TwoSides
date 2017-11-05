using System;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.Physics.Entity.NPC;
using TwoSides.World;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Entity.NPC
{
    public class Wolf : BaseNpc
    {

        float _frame;
       // int frame;
        public Wolf(Texture2D npcSkin) : base(new[]{npcSkin})
        {
            Width = 42;
            Height = 20;
        }

        public Wolf(int blockx, Texture2D npcSkin)
            : base(blockx, new[] { npcSkin })
        {
            WayPoint = Program.Game.Player.Position.X * Tile.TileMaxSize;
            Width = 42;
            Height = 20;
        }
        public override void Update()
        {
            base.Update();
            if ((int)_frame < 4) _frame += 1 / 24.0f;
            else _frame = 0;
        }
        public override void RenderNpc( SpriteBatch spriteBatch, SpriteFont font, Texture2D shadow)
        {

            SpriteEffects effect = SpriteEffects.None;
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.DrawString(font, ((int)Hp).ToString(CultureInfo.CurrentCulture), new Vector2((int)(Position.X + (Width - NpcSkin[0].Width)), (int)Position.Y - 30), Color.Black);

            Rectangle src = new Rectangle(0, 0, NpcSkin[0].Width, NpcSkin[0].Height / 9);
            src.Y += (int)_frame * src.Height;
            Rect = new Rectangle((int)(Position.X + (Width - NpcSkin[0].Width)),
                    (int)Position.Y, src.Width, src.Height);
            foreach ( Texture2D skin in NpcSkin )
            {
                spriteBatch.Draw(skin, Rect, src, Color.White,
                                 0, Vector2.Zero, effect, 0);
            }
            DrawShadow(shadow, spriteBatch);
        }
        protected void AttackPlayer()
        {
            if ( !Program.Game.Player.Rect.Intersects(Rect) ) return;

            if (Program.Game.Dimension[Program.Game.CurrentDimension].Rand.Next(0, 100) <= 1)
            {
                if (!Program.Game.Player.Slot[Player.Slotmax].IsEmpty)
                {
                    Program.Game.Player.Slot[Player.Slotmax].DamageSlot(Math.Max(1, 5 - Program.Game.Player.Slot[Player.Slotmax].GetDef()) * Program.Game.Seconds);
                    if (Program.Game.Player.Slot[Player.Slotmax].Hp <= 2) Program.Game.Player.Slot[Player.Slotmax] = new Item();
                }
                if (Program.Game.Player.Slot[Player.Slotmax].IsEmpty)
                {

                    Program.Game.Player.TypeKill = 0;
                }
            }
            else
            {
                int b = Program.Game.Dimension[Program.Game.CurrentDimension].Rand.Next(0, 2);
                if (!Program.Game.Player.Slot[Player.Slotmax + 1].IsEmpty)
                {
                    Program.Game.Player.Slot[Player.Slotmax + 1].DamageSlot(Math.Max(1, 5 - Program.Game.Player.Slot[Player.Slotmax + 1].GetDef()) * Program.Game.Seconds);
                    if (Program.Game.Player.Slot[Player.Slotmax + 1].Hp < 2) Program.Game.Player.Slot[Player.Slotmax + 1] = new Item();
                }
                if (!Program.Game.Player.Slot[Player.Slotmax + 2].IsEmpty)
                {
                    Program.Game.Player.Slot[Player.Slotmax + 2].DamageSlot(Math.Max(1, 5 - Program.Game.Player.Slot[Player.Slotmax + 2].GetDef()) * Program.Game.Seconds);
                    if (Program.Game.Player.Slot[Player.Slotmax + 2].Hp < 2) Program.Game.Player.Slot[Player.Slotmax + 2] = new Item();
                }
                if ( b != 0 && b != 1 || !Program.Game.Player.Slot[Player.Slotmax + 1 + b].IsEmpty ) return;

                if (b == 0)
                    b = Program.Game.Player.Position.X > Position.X ? 3 : 2;
                else
                    b = Program.Game.Player.Position.X > Position.X ? 0 : 1;
                Program.Game.Player.Bloods[b] = true;
            }
        }
    }
}
