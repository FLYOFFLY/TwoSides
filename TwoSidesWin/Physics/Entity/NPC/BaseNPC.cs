
        using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.World;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Tile;
using System.IO;
namespace TwoSides.Physics.Entity.NPC
{
    public class BaseNpc : CEntity
    {
        protected float to = 0;
        protected  float maxRunSpeed = 0.5f;
        protected  float runAcceleration = 0.08f;
        protected  float runSlowdown = 0.2f;
        protected  int jumpHeight = 15;
        protected  float jumpSpeed = 5.01f;
        public bool controlLeft;
        public bool controlRight;
        public bool controlUp;
        public bool controlDown;
        public bool controlJump;
        public bool releaseJump;
        public int width = 20;
        public int height = 42;
        public int direction = 1;
        public int jump;
        public int type = 0;
        public float hp = 4;
        public override void load(BinaryReader reader)
        {
            base.load(reader);
            
        }
        public override void save(BinaryWriter writer)
        {
            base.save(writer);
        }
        public ArrayList drop = new ArrayList();

        public Rectangle rect;
        protected Texture2D[] npcSkin;

        public BaseNpc()
        {
        }

        public BaseNpc(Texture2D[] npcSkin)
        {
            this.npcSkin = npcSkin;
        }

        public BaseNpc(int blockx,Texture2D[] npcSkin)
        {
            position.X = blockx * ITile.TileMaxSize;
            position.Y = Program.game.dimension[Program.game.currentD].mapHeight[blockx] * ITile.TileMaxSize - ITile.TileMaxSize * 3;
            type = Program.game.rand.Next(2);
            rect.Width = width;

            this.npcSkin = npcSkin;
            to = position.X;
            to += Program.game.rand.Next(-1, 1) * ITile.TileMaxSize;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1,7));
            this.npcSkin = npcSkin;
        }

        public BaseNpc(Vector2 positions, Texture2D[] npcSkin)
        {
            position = positions;
            to = position.X;
            to += Program.game.rand.Next(-ITile.TileMaxSize, ITile.TileMaxSize);
            rect.Width = width;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1,7));
            this.npcSkin = npcSkin;
        }

        public BaseNpc(int blockx, Texture2D[] npcSkin, Color[] color)
        {
            position.X = blockx * ITile.TileMaxSize;
            if (blockx <= 0) blockx = 0;
            else if (blockx >= SizeGeneratior.WorldWidth) blockx = SizeGeneratior.WorldWidth - 2;
            position.Y = Program.game.dimension[Program.game.currentD].mapHeight[blockx] * ITile.TileMaxSize - ITile.TileMaxSize * 3;
            type = Program.game.rand.Next(2);
            rect.Width = width;

            to = position.X;
            to += Program.game.rand.Next(-1, 1) * ITile.TileMaxSize;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1, 7));
            this.npcSkin = npcSkin;
        }

        public BaseNpc(Vector2 positions, Texture2D[] npcSkin, Color[] color)
        {
            position = positions;
            to = position.X;
            to += Program.game.rand.Next(-ITile.TileMaxSize, ITile.TileMaxSize);
            rect.Width = width;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1, 7));
            this.npcSkin = npcSkin;
        }

        protected void DrawShadow(Texture2D shadow, SpriteBatch spriteBatch)
        {
            int startnew = (int)position.Y / ITile.TileMaxSize;
            startnew += 3;
            int endnew = startnew + 10;
            for (int j = startnew; j < endnew; j++)
            {
                if (endnew >= SizeGeneratior.WorldHeight) continue;
                if (Program.game.dimension[Program.game.currentD].map[(int)Math.Floor(position.X) / ITile.TileMaxSize, j].active)
                {
                    int del = j - startnew;
                    //if (del == 0) del = 1;
                    Rectangle rect = new Rectangle((int)Math.Floor(position.X) ,
                    j * ITile.TileMaxSize, shadow.Width, (int)(shadow.Height - (del)));

                    spriteBatch.Draw(shadow, rect, Color.Black);
                    break;
                }
            }
        }

        public virtual void DrawNPC(SpriteEffects effect, SpriteBatch spriteBatch, SpriteFont Font1,Texture2D shadow)
        {
            effect = SpriteEffects.None;
            if (direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.DrawString(Font1, ((int)(hp)).ToString(), new Vector2((int)(position.X + (width - npcSkin[0].Width)), (int)(position.Y) - 30), Color.Black);
            Rectangle rect = new Rectangle((int)(position.X + (width -npcSkin[0].Width)),
                    (int)(position.Y), npcSkin[0].Width, npcSkin[0].Height);
            Rectangle src = new Rectangle(0, 0, npcSkin[0].Width, npcSkin[0].Height);
            for (int i = 0; i < npcSkin.Length; i++)
            {
                spriteBatch.Draw(npcSkin[i], rect, src,Color.White,
                        0, Vector2.Zero, effect, 0);
            }
            DrawShadow(shadow, spriteBatch);
        }

        public void walkto(float x)
        {
            to = x;
        }

        public void kill()
        {
            foreach (Item slots in drop)
            {
                Program.game.adddrop((int)position.X, (int)position.Y, slots);
                Program.game.console.addLog("NPC DROPin");
            }
            Program.game.console.addLog("NPC DROP:" + drop.Count);
        }

        protected virtual void aiupdate() { }

        public override void update()
        {
            this.controlLeft = false;
            this.controlRight = false;
            aiupdate();
            if ((int)to / ITile.TileMaxSize == (int)position.X / ITile.TileMaxSize)
            {
                to += Program.game.rand.Next(-ITile.TileMaxSize, ITile.TileMaxSize);
                if (to < 5 * ITile.TileMaxSize) to = 5 * ITile.TileMaxSize;
            }
            if (to > position.X)
            {
                this.controlRight = true;
                //   this.controlJump = true;
            }
            if (to < position.X)
            {
                this.controlLeft = true;
            }
            if (this.controlLeft && this.velocity.X > -maxRunSpeed)
            {
                if (this.velocity.X > runSlowdown)
                {
                    this.velocity.X = this.velocity.X - runSlowdown;
                }
                this.velocity.X = this.velocity.X - runAcceleration;
                 this.direction = -1;
            }
            else if (this.controlRight && this.velocity.X < maxRunSpeed)
            {
                if (this.velocity.X < -runSlowdown)
                {
                    this.velocity.X = this.velocity.X + runSlowdown;
                }
                this.velocity.X = this.velocity.X + runAcceleration;
                this.direction = 1;
            }
            else if (this.velocity.Y == 0f)
            {
                if (this.velocity.X > runSlowdown)
                {
                    this.velocity.X = this.velocity.X - runSlowdown;
                }
                else if (this.velocity.X < -runSlowdown)
                {
                    this.velocity.X = this.velocity.X + runSlowdown;
                }
                else
                {
                    this.velocity.X = 0f;
                }
            }
            else if ((double)this.velocity.X > (double)runSlowdown * 0.5)
            {
                this.velocity.X = this.velocity.X - runSlowdown * 0.5f;
            }
            else if ((double)this.velocity.X < (double)(-(double)runSlowdown) * 0.5)
            {
                this.velocity.X = this.velocity.X + runSlowdown * 0.5f;
            }
            else
            {
                this.velocity.X = 0f;
            }
            if (this.controlJump)
            {
                if (this.jump > 0)
                {
                    if (this.velocity.Y > -jumpSpeed + gravity * 2f)
                    {
                        this.jump = 0;
                    }
                    else
                    {
                        this.velocity.Y = -jumpSpeed;
                        this.jump--;
                    }
                }
                else if (this.velocity.Y == 0f && this.releaseJump)
                {
                    this.velocity.Y = -jumpSpeed;
                    this.jump = jumpHeight;
                }
            }
            else
            {

                this.jump = 0;
                this.releaseJump = true;
            }
            this.velocity.Y = this.velocity.Y + gravity;
            if (this.velocity.Y > maxFallSpeed)
            {
                this.velocity.Y = maxFallSpeed;
            }

            controlJump = false;
            float a = velocity.X;
            this.velocity = Colision.TileCollision(this,this.position, this.velocity, this.width, this.height,false);
            this.position += this.velocity;
            if (position.X < 0)
            {
                position.X = 0;
            }
            if (position.X > (SizeGeneratior.WorldWidth - 1) * ITile.TileMaxSize)
            {
                position.X = (SizeGeneratior.WorldWidth - 1) * ITile.TileMaxSize;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            if (position.Y > (SizeGeneratior.WorldHeight - 1) * ITile.TileMaxSize)
            {
                position.Y = (SizeGeneratior.WorldHeight - 1) * ITile.TileMaxSize;
            }
            if (a != velocity.X) this.controlJump = true;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }
    }
}
