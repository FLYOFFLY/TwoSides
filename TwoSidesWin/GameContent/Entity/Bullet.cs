using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwoSides.Physics.Entity.NPC;
using TwoSides.World;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.Physics;
using TwoSides.Physics.Entity;
using TwoSides.World.Tile;
using TwoSides.Utils;
using TwoSides.GameContent.Tiles;
namespace TwoSides.GameContent.Entity
{
    public class Bullet : CEntity
    {

        public const float maxtime = 1;
        public const int BulletSizeX = 12;
        public const int BulletSizeY = 5;

        float dirx = 1;
        int countrotate = 0;
        float ase = 1;
        float diry = 1;
        float time = 0;
        static float mass = 3.4f;
        public int type = 1;

        public Vector2 oldposition;


        public static Texture2D BulletTexture;
        public static void LoadBullet(ContentManager Content)
        {
            BulletTexture = Content.Load<Texture2D>(Game1.ImageFolder + "Bullet");
        }

        public Bullet(Vector2 position, Vector2 angle)
        {
            
            this.position = position;
            this.position.Y += 16;
            if(angle != Vector2.Zero) angle.Normalize();
           // float angles = (float)Cordionats.angletomouse((int)position.X, (int)position.Y);
            dirx = angle.X;
            diry = angle.Y;
            this.oldposition = position;
            this.velocity.X +=((32) * (float)dirx);
            this.velocity.Y += this.velocity.Y + ((32) * (float)diry);
            this.velocity = Colision.TileCollision(this,position, velocity, BulletSizeX, BulletSizeY,true);
            this.position += this.velocity;
            time = 0;
            this.velocity = Vector2.Zero;
            this.velocity.Y = diry * 1;
            this.velocity.X = dirx * 1;
            type = 1;
            par = new Particle(Program.game.carma , position, velocity, 90, 0,Color.White.ToVector4(), 1024, 500, 0, 0);
        }
        public Particle par;
        public void destory()
        {
            if(type == 1)Program.game.dimension[Program.game.currentD].settexture((
                int)Math.Floor(oldposition.X / ITile.TileMaxSize), 
                (int)Math.Floor(oldposition.Y / ITile.TileMaxSize), 1);
        }

        public bool move()
        {
            par.update();
            this.oldposition = position;
            time += Program.game.seconds;
           // float velrev = (mass * 9.8f) - (ase);
            if (this.velocity.X <= 10 && this.velocity.X >= -10)
            {
                this.velocity.X +=((ase * Program.game.seconds) * (float)dirx);
                //this.velocity.X += ((velrev * Program.game.seconds) * (float)dirx );
            }
          // diry -= (float)((9.8f*mass) * Program.game.seconds);
            velocity.Y += 0.2f;
            this.velocity = Colision.TileCollision(this,position, velocity, BulletSizeX, BulletSizeY,true);
            this.position += this.velocity;
            Program.game.AddExplosion(position);
            if (velocity.X == 0)
            {
                 countrotate++;
                dirx *= -1;
            }
            if (velocity.Y == 0 && diry != 0)
            {
                countrotate++;
                diry *= -1;
            }
            foreach (Zombie npcs in Program.game.dimension[Program.game.currentD].Zombies)
            {
                if (Util.directional((int)Program.game.player.position.X / ITile.TileMaxSize, (int)npcs.position.X / ITile.TileMaxSize, 20) &&
                  Util.directional((int)Program.game.player.position.Y / ITile.TileMaxSize, (int)npcs.position.Y / ITile.TileMaxSize, 20))
                {
                    npcs.walkto(Program.game.player.position.X);
                }
            }
            foreach (Zombie npcs in  Program.game.dimension[Program.game.currentD].Zombies
               )
            {

                if (Util.InCube(new Rectangle((int)position.X, (int)position.Y, BulletSizeX, BulletSizeY),
                    new Rectangle( (int)npcs.position.X, (int)npcs.position.Y, npcs.width, 18) ) )
                {
                    npcs.hp -= 1;
                    if (this.velocity.X >=0)
                        npcs.velocity.X += mass/2;
                    else
                        npcs.velocity.X -= mass/2;
                    if ((int)npcs.hp <= 0)
                    {
                        if (npcs.type == 5) ((Boss)(npcs)).kill();
                        else npcs.kill();
                        Program.game.dimension[Program.game.currentD].Zombies.Remove(npcs);

                    } return false;
                }
            }
            if (Util.InCube(new Rectangle((int)position.X, (int)position.Y, BulletSizeX, BulletSizeY),
                    new Rectangle((int)Program.game.player.position.X, (int)(int)Program.game.player.position.Y
                        , (int)Program.game.player.width, 18))) //head
            {
                if (!Program.game.player.slot[Player.slotmax].IsEmpty)
                {
                    Program.game.player.slot[Player.slotmax].damageslot((float)Math.Max(1.0, 10 - Program.game.player.slot[Player.slotmax].getDef()));
                    if (Program.game.player.slot[Player.slotmax].HP <= 2) Program.game.player.slot[Player.slotmax] = new Item();
                  
                }
                else Program.game.player.typeKill = 0;
                return false;
            }
            if (Util.InCube(new Rectangle((int)position.X, (int)position.Y, BulletSizeX, BulletSizeY),
                    new Rectangle((int)Program.game.player.position.X, (int)(int)Program.game.player.position.Y+18
                        , (int)Program.game.player.width, 20)))//body
            {
                if (!Program.game.player.slot[Player.slotmax + 1].IsEmpty)
                {
                    Program.game.player.slot[Player.slotmax + 1].damageslot(Math.Max(1, 10 - Program.game.player.slot[Player.slotmax + 1].getDef()));
                    if (Program.game.player.slot[Player.slotmax + 1].HP < 2) Program.game.player.slot[Player.slotmax + 1] = new Item();
                    if (this.velocity.X >= 0)
                        Program.game.player.velocity.X += mass / 2;
                    else
                        Program.game.player.velocity.X -= mass / 2;
                }
                else 
                {
                    if (velocity.X <= 0) Program.game.player.bloods[3] = true;
                    else Program.game.player.bloods[2] = true;
                }
                return false;
            }
            if (Util.InCube(new Rectangle((int)position.X, (int)position.Y, BulletSizeX, BulletSizeY),
                    new Rectangle((int)Program.game.player.position.X, (int)(int)Program.game.player.position.Y + 18+20
                        , (int)Program.game.player.width, 24)))//legs
            {
                if (!Program.game.player.slot[Player.slotmax + 2].IsEmpty)
                {
                    Program.game.player.slot[Player.slotmax + 2].damageslot(Math.Max(1, 10 - Program.game.player.slot[Player.slotmax + 2].getDef()));
                    if (Program.game.player.slot[Player.slotmax + 2].HP < 2) Program.game.player.slot[Player.slotmax + 2] = new Item();
                }
                else
                {
                    if (velocity.X <= 0) Program.game.player.bloods[0] = true;
                    else Program.game.player.bloods[1] = true;
                }
                return false;
            }
            if (type == 0)
                return time < maxtime;
            else return countrotate < 1;
        }
    }
}
