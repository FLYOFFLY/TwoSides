using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoSides.GUI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using TwoSides.World;
using TwoSides.GameContent.GenerationResources;
using TwoSides.Physics.Entity.NPC;
using TwoSides.Physics;
using TwoSides.World.Tile;
using System.IO;

namespace TwoSides.GameContent.Entity.NPC
{
    [Serializable]
    sealed public class Civilian :BaseCivNpc
    {
        bool isEnable = false;
        public int activedialog;

        public ArrayList dialog = new ArrayList();
        public override void load(BinaryReader reader)
        {
            base.load(reader);
            activedialog = reader.ReadInt32();
        }
        public override void save(BinaryWriter writer)
        {
            base.save(writer);
            writer.Write(activedialog);
        }
        public Civilian(Vector2 positions)
        {
            position = positions;
            to = position.X;

            to += Program.game.rand.Next(-ITile.TileMaxSize, ITile.TileMaxSize);
            rect.Width = width;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1, 7));
            activedialog = 0;
            this.dialog = null;
        }

        public Civilian(Vector2 positions, ArrayList dialog)
        {
            position = positions;
            to = position.X;

            to += Program.game.rand.Next(-ITile.TileMaxSize, ITile.TileMaxSize);
            rect.Width = width;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1, 7));
            this.dialog = dialog;
            activedialog = 0;
        }

        public void changeenable(){
            isEnable = !isEnable;
        }

        public void offenable()
        {
            isEnable = false;
        }

        public void DrawNPC(SpriteEffects effect, SpriteBatch spriteBatch, SpriteFont Font1, Texture2D head, Texture2D head2,
          Texture2D body, Texture2D legs,Texture2D shadow)
        {
            effect = SpriteEffects.None;
            if (direction < 0)
                effect = SpriteEffects.FlipHorizontally;
           if(activedialog ==0) spriteBatch.DrawString(Font1, "Press RKM on NPC", new Vector2((int)(position.X + (width - head.Width)), (int)(position.Y) - 30), Color.White);

            spriteBatch.Draw(head, new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(body, new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(legs, new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(Clothes.shirt[1], new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(Clothes.pants[0], new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(Clothes.shoes[0], new Rectangle((int)(position.X + (width - head.Width)), (int)(position.Y), head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red, 0, Vector2.Zero, effect, 0);
            DrawShadow(shadow, spriteBatch);
        }

        public void renderDialog(SpriteBatch spriteBatch)
        {
            if (isEnable && dialog != null)
            {
                spriteBatch.End();
                ((Dialog)(dialog[activedialog])).render(spriteBatch);
                spriteBatch.Begin();
            }
        }
        public void changevisible(int btn)
        {
            ((Dialog)(dialog[activedialog])).ChangeVisible(btn);
        }

        public void changevisible(int btn,bool newChange)
        {
            if ( ( (Dialog)(dialog[activedialog]) ).IsVisible(btn)!=newChange) ((Dialog)(dialog[activedialog])).ChangeVisible(btn, newChange);
        }

        public bool isbtnclicked(int btnid)
        {

            if (isEnable && dialog != null)
            {
                return ((Dialog)(dialog[activedialog])).IsBtnClicked(btnid);
            }
            else return false;
        }

        public override void update()
        {
            if (isEnable && dialog != null)
            {
                
                ((Dialog)(dialog[activedialog])).Update();
            }
            this.controlLeft = false;
            this.controlRight = false;
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
                if (this.itemAnimation == 0)
                {
                    this.direction = -1;
                }
            }
            else if (this.controlRight && this.velocity.X < maxRunSpeed)
            {
                if (this.velocity.X < -runSlowdown)
                {
                    this.velocity.X = this.velocity.X + runSlowdown;
                }
                this.velocity.X = this.velocity.X + runAcceleration;
                if (this.itemAnimation == 0)
                {
                    this.direction = 1;
                }
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
