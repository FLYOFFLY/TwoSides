using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.Physics.Entity.NPC;
using TwoSides;
using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.Physics;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Tile;
using TwoSides.Utils;

namespace TwoSides.GameContent.Entity.NPC
{
    public class Triangle : BaseNpc
    {
        private float to2;
        protected override void aiupdate()
        {
            if (Util.directional((int)Program.game.player.position.X / ITile.TileMaxSize, (int)position.X / ITile.TileMaxSize, 5) &&
                Util.directional((int)Program.game.player.position.Y / ITile.TileMaxSize, (int)position.Y / ITile.TileMaxSize, 5))
            {
                to = Program.game.player.position.X;
                to2 = Program.game.player.position.Y;
            }
        }

        public override void update()
        {
            this.controlLeft = false;
            this.controlRight = false;
            aiupdate();
            if (Program.game.player.rect.Intersects(rect) )
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
            if (this.position.Y > to2)
            {
                this.velocity.Y -= gravity;

                if (this.velocity.Y > -maxFallSpeed)
                {
                    this.velocity.Y = -maxFallSpeed;
                }
            }
            else if (this.position.Y < to2)
            {
                this.velocity.Y = this.velocity.Y + gravity;
                if (this.velocity.Y > maxFallSpeed)
                {
                    this.velocity.Y = maxFallSpeed;
                }
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
            } if (a != velocity.X) this.controlJump = true;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
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
