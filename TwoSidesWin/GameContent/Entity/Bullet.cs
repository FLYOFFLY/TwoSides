using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using TwoSides.GameContent.Entity.NPC;
using TwoSides.Physics;
using TwoSides.Physics.Entity;
using TwoSides.Physics.Entity.NPC;
using TwoSides.Utils;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Entity
{
    public class Bullet : DynamicEntity
    {

        public const float MaxTime = 1;
        public const int BulletSizeX = 12;
        public const int BulletSizeY = 5;

        float _dirX;
        int _countRotate;
        const float ASE = 1;
        float _dirY;
        float _time;
        const float MASS = 3.4f;

        public Vector2 OldPosition;

        public static void LoadBullet(ContentManager content)
        {
        }

        public Bullet(Vector2 position, Vector2 angle)
        {
            
            Position = position;
            Position.Y += 16;
            if(angle != Vector2.Zero) angle.Normalize();
           // float angles = (float)Cordionats.AngleMouse((int)position.X, (int)position.Y);
            _dirX = angle.X;
            _dirY = angle.Y;
            Velocity.X +=32 * _dirX;
            Velocity.Y += Velocity.Y + 32 * _dirY;
            Velocity = Colision.TileCollision(this,position, Velocity, BulletSizeX, BulletSizeY,true);
            Position += Velocity;
            _time = 0;
            Velocity = Vector2.Zero;
            Velocity.Y = _dirY * 1;
            Velocity.X = _dirX * 1;
            TypeBullet = 1;
            SoundPulse();
        }

        public int TypeBullet { get; }

        public void Destory(BaseDimension dimension)
        {
            Program.Game.AddExplosion(Position);
            if (TypeBullet == 1)
            {
                dimension.SetTexture((
                                         int)Math.Floor(OldPosition.X / Tile.TileMaxSize), 
                                     (int)Math.Floor(OldPosition.Y / Tile.TileMaxSize), 1);
            }
        }

        public bool Move()
        {
            OldPosition = Position;
            _time += Program.Game.Seconds;
            // float velrev = (mass * 9.8f) - (ase);
            if (Velocity.X <= 10 && Velocity.X >= -10)
            {
                Velocity.X += ASE * Program.Game.Seconds * _dirX;
                //this.velocity.X += ((velrev * Program.game.seconds) * (float)dirx );
            }
            // _dirY -= (float)((9.8f*mass) * Program.game.seconds);
            Velocity.Y += 0.2f;
            Velocity = Colision.TileCollision(this, Position, Velocity, BulletSizeX, BulletSizeY, true);
            Position += Velocity;
            if (Math.Abs(Velocity.X) <
            float.Epsilon)//Y равен
            {
                _countRotate++;
                _dirX *= -1;
            }
            if (Math.Abs(Velocity.Y) <
            float.Epsilon && Math.Abs(_dirY) >
            float.Epsilon)
            {
                _countRotate++;
                _dirY *= -1;
            }
            if ( Program.Game.Dimension[Program.Game.CurrentDimension].Zombies.Any(TryDmg) )
            {
                return false;
            }
            if (Tools.InCube(new Rectangle((int)Position.X, (int)Position.Y, BulletSizeX, BulletSizeY),
                    new Rectangle((int)Program.Game.Player.Position.X, (int)Program.Game.Player.Position.Y
                        , Program.Game.Player.Width, 18))) //head
            {
                Program.Game.Player.DamageHead();
                return false;
            }
            if (Tools.InCube(new Rectangle((int)Position.X, (int)Position.Y, BulletSizeX, BulletSizeY),
                    new Rectangle((int)Program.Game.Player.Position.X, (int)Program.Game.Player.Position.Y + 18
                        , Program.Game.Player.Width, 20)))//body
            {
                if (!Program.Game.Player.Slot[Player.Slotmax + 1].IsEmpty)
                {
                    Program.Game.Player.DamageArmor(1);
                    Program.Game.Player.AddForce(Velocity, MASS);
                }
                else
                {
                    if (Velocity.X <= 0) Program.Game.Player.Bloods[3] = true;
                    else Program.Game.Player.Bloods[2] = true;
                }
                return false;
            }
            if (Tools.InCube(new Rectangle((int)Position.X, (int)Position.Y, BulletSizeX, BulletSizeY),
                    new Rectangle((int)Program.Game.Player.Position.X, (int)Program.Game.Player.Position.Y + 18 + 20
                        , Program.Game.Player.Width, 24)))//legs
            {
                if (!Program.Game.Player.Slot[Player.Slotmax + 2].IsEmpty)
                {
                    Program.Game.Player.DamageArmor(2);
                }
                else
                {
                    if (Velocity.X <= 0) Program.Game.Player.Bloods[0] = true;
                    else Program.Game.Player.Bloods[1] = true;
                }
                return false;
            }
            if (TypeBullet == 0)
                return _time < MaxTime;

            return _countRotate < 1;
        }

        bool TryDmg(BaseNpc npcs)
        {
            if ( !Tools.InCube(new Rectangle((int) Position.X , (int) Position.Y , BulletSizeX , BulletSizeY) ,
                               new Rectangle((int) npcs.Position.X , (int) npcs.Position.Y , npcs.Width , 18)) )
                return false;

            npcs.Hp -= 1;
            if (Velocity.X >= 0)
                npcs.Velocity.X += MASS / 2;
            else
                npcs.Velocity.X -= MASS / 2;

            if ( (int) npcs.Hp > 0 ) return true;

            if (npcs.Type == 5) ((Boss)npcs).Kill();
            else npcs.Kill();
            return true;
        }

        void SoundPulse()
        {
            foreach (Zombie npcs in Program.Game.Dimension[Program.Game.CurrentDimension].Zombies)
            {
                if (Tools.Distance((int)Position.X , (int)npcs.Position.X, 320) &&
                  Tools.Distance((int)Position.Y, (int)npcs.Position.Y, 320))
                {
                    npcs.Move(Position.X);
                }
            }
        }
    }
}
