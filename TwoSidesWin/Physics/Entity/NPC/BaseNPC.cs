using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GameContent.GenerationResources;
using TwoSides.World;
using TwoSides.World.Tile;

namespace TwoSides.Physics.Entity.NPC
{
    public class BaseNpc : DynamicEntity
    {
        protected float WayPoint;
        protected float MaxRunSpeed = 0.5f;
        protected float RunAcceleration = 0.08f;
        protected float RunSlowdown = 0.2f;
        protected int JumpHeight = 15;
        protected float JumpSpeed = 5.01f;
        public bool ControlLeft;
        public bool ControlRight;
        public bool ControlUp;
        public bool ControlDown;
        public bool ControlJump;
        public bool ReleaseJump;
        public int Width = 20;
        public int Height = 42;
        public int Direction = 1;
        public int Jump;
        public int Type;
        public float Hp = 4;
       
        public List<Item> Drop = new List<Item>();

        public Rectangle Rect;
        protected Texture2D[] NpcSkin;

        public BaseNpc()
        {
        }

        public BaseNpc(Texture2D[] npcSkin) => NpcSkin = npcSkin;

        public BaseNpc(int blockx, Texture2D[] npcSkin)
        {
            if (blockx <= 0) blockx = 0;
            else if (blockx > SizeGeneratior.WorldWidth) blockx = SizeGeneratior.WorldWidth - 1;

            Position.X = blockx * Tile.TILE_MAX_SIZE;
            Position.Y = Program.Game.Dimension[Program.Game.CurrentDimension].MapHeight[blockx] * Tile.TILE_MAX_SIZE - Tile.TILE_MAX_SIZE * 3;
            Type = Program.Game.Rand.Next(2);
            Rect.Width = Width;

            NpcSkin = npcSkin;
            WayPoint = Position.X;
            WayPoint += Program.Game.Rand.Next(-1, 1) * Tile.TILE_MAX_SIZE;
            Rect.Height = Height;
            Drop.Clear();
            Drop.Add(new Item(1, 7));
            NpcSkin = npcSkin;
        }

        public BaseNpc(Vector2 positions, Texture2D[] npcSkin)
        {
            Position = positions;
            WayPoint = Position.X;
            WayPoint += Program.Game.Rand.Next(-Tile.TILE_MAX_SIZE, Tile.TILE_MAX_SIZE);
            Rect.Width = Width;
            Rect.Height = Height;
            Drop.Clear();
            Drop.Add(new Item(1, 7));
            NpcSkin = npcSkin;
        }
        
        protected void DrawShadow(Texture2D shadow, Render render)
        {
            var startnew = (int)Position.Y / Tile.TILE_MAX_SIZE;
            startnew += 3;
            var endnew = startnew + 10;
            for (var j = startnew; j < endnew; j++)
            {
                if (endnew >= SizeGeneratior.WorldHeight) continue;
                if ( !Program.Game.Dimension[Program.Game.CurrentDimension]
                             .MapTile[(int) Math.Floor(Position.X) / Tile.TILE_MAX_SIZE , j].Active ) continue;

                var del = j - startnew;
                //if (del == 0) del = 1;
                Rectangle rectShadow = new Rectangle((int)Math.Floor(Position.X),
                                                     j * Tile.TILE_MAX_SIZE, shadow.Width, shadow.Height - del);

                render.Draw(shadow, rectShadow, Color.Black);
                break;
            }
        }

        public virtual void RenderNpc(Render render, SpriteFont font, Texture2D shadow)
        {

            SpriteEffects effect = SpriteEffects.None;
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            render.DrawString(font, ((int)Hp).ToString(CultureInfo.CurrentCulture), new Vector2((int)(Position.X + (Width - NpcSkin[0].Width)), (int)Position.Y - 30), Color.Black);
            Rect = new Rectangle((int)(Position.X + (Width - NpcSkin[0].Width)),
                    (int)Position.Y, NpcSkin[0].Width, NpcSkin[0].Height);
            Rectangle src = new Rectangle(0, 0, NpcSkin[0].Width, NpcSkin[0].Height);
            foreach ( Texture2D skin in NpcSkin )
            {
                render.Draw(skin, Rect, src, Color.White, effect);
            }
            DrawShadow(shadow, render);
        }

        public void Move(float x)
        {
            WayPoint = x;
        }

        public virtual void Kill()
        {
            foreach (Item slots in Drop)
            {
                Program.Game.AddDrop((int)Position.X, (int)Position.Y, slots);
                Program.Game.Console.AddLog("NPC DROPin");
            }
            Program.Game.Console.AddLog("NPC DROP:" + Drop.Count);
        }

        protected virtual void UpdateAi()
        {
            if ((int)WayPoint / Tile.TILE_MAX_SIZE == (int)Position.X / Tile.TILE_MAX_SIZE)
            {
                WayPoint += Program.Game.Rand.Next(-Tile.TILE_MAX_SIZE, Tile.TILE_MAX_SIZE);
                if (WayPoint < 5 * Tile.TILE_MAX_SIZE) WayPoint = 5 * Tile.TILE_MAX_SIZE;
            }
            if (WayPoint > Position.X)
                ControlRight = true;
            if (WayPoint < Position.X)
                ControlLeft = true;
        }

        public override void Update()
        {
            if (Hp <= 0) return;
            ControlLeft = false;
            ControlRight = false;
            UpdateAi();
            HorisontalMove();
            VerticalMove();
            Move();
        }

        protected void Move()
        {
            ControlJump = false;
            var a = Velocity.X;
            Velocity = Colision.TileCollision(this, Position, Velocity, Width, Height, false);
            Position += Velocity;
            if (Position.X < 0)
            {
                Position.X = 0;
            }
            if (Position.X > (SizeGeneratior.WorldWidth - 1) * Tile.TILE_MAX_SIZE)
            {
                Position.X = (SizeGeneratior.WorldWidth - 1) * Tile.TILE_MAX_SIZE;
            }
            if (Position.Y < 0)
            {
                Position.Y = 0;
            }
            if (Position.Y > (SizeGeneratior.WorldHeight - 1) * Tile.TILE_MAX_SIZE)
            {
                Position.Y = (SizeGeneratior.WorldHeight - 1) * Tile.TILE_MAX_SIZE;
            }
            if (Math.Abs(a - Velocity.X) > float.Epsilon) ControlJump = true;
            Rect.X = (int)Position.X;
            Rect.Y = (int)Position.Y;
        }

        protected void HorisontalMove()
        {

            if (ControlLeft && Velocity.X > -MaxRunSpeed)
            {
                if (Velocity.X > RunSlowdown)
                {
                    Velocity.X = Velocity.X - RunSlowdown;
                }
                Run(-1);
            }
            else if (ControlRight && Velocity.X < MaxRunSpeed)
            {
                if (Velocity.X < -RunSlowdown)
                {
                    Velocity.X = Velocity.X + RunSlowdown;
                }
                Run(1);
            }
            else if (Math.Abs(Velocity.Y) < float.Epsilon)
            {
                if (Velocity.X > RunSlowdown)
                {
                    Velocity.X = Velocity.X - RunSlowdown;
                }
                else if (Velocity.X < -RunSlowdown)
                {
                    Velocity.X = Velocity.X + RunSlowdown;
                }
                else
                {
                    Velocity.X = 0f;
                }
            }
            else if (Velocity.X > RunSlowdown * 0.5)
            {
                Velocity.X = Velocity.X - RunSlowdown * 0.5f;
            }
            else if (Velocity.X < -(double)RunSlowdown * 0.5)
            {
                Velocity.X = Velocity.X + RunSlowdown * 0.5f;
            }
            else
            {
                Velocity.X = 0f;
            }
        }

        void Run(int direction)
        {
            Direction = direction;
            Velocity.X = Velocity.X + RunAcceleration * direction;
        }

        protected void VerticalMove()
        {
            if (ControlJump)
            {
                if (Jump > 0)
                {
                    if (Velocity.Y > -JumpSpeed + Gravity * 2f)
                    {
                        Jump = 0;
                    }
                    else
                    {
                        Velocity.Y = -JumpSpeed;
                        Jump--;
                    }
                }
                else if (Math.Abs(Velocity.Y) < float.Epsilon && ReleaseJump)
                {
                    Velocity.Y = -JumpSpeed;
                    Jump = JumpHeight;
                }
            }
            else
            {

                Jump = 0;
                ReleaseJump = true;
            }
            Velocity.Y = Velocity.Y + Gravity;
            if (Velocity.Y > MaxFallSpeed)
            {
                Velocity.Y = MaxFallSpeed;
            }
        }
    }
}
