using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GameContent.Entity.NPC;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.Tiles;
using TwoSides.GUI;
using TwoSides.Physics;
using TwoSides.World.Generation.Structures;

using Console = System.Console;

namespace TwoSides.World.Generation
{
    [Serializable]
    public class BaseDimension
    {
        public bool Active { get;  set; }
        public float Globallighting = 5;
        protected List<Point> UpdateTile = new List<Point>();
        public List<Zombie> Zombies = new List<Zombie>();
        public List<Civilian> Civil = new List<Civilian>();
        List<Explosion>  _explosions = new List<Explosion>();
        public int[] MapHeight = new int[SizeGeneratior.WorldWidth];
        public Tile.Tile[,] MapTile = new Tile.Tile[SizeGeneratior.WorldWidth, SizeGeneratior.WorldHeight];
        public Biome[] MapBiomes = new Biome[SizeGeneratior.WorldWidth];
        public List<Dust> Dusts = new List<Dust>();

        public Random Rand = new Random();

        [NonSerialized]
        public bool IsPortal;

        [NonSerialized]
        public List<BaseStruct> StructuresList = new List<BaseStruct>();
        public virtual bool IsUpdate(int idTextutre) => false;

        public bool IsNullOrEmpty(int x,int y){
            if (x < 0 || x >= SizeGeneratior.WorldWidth || y < 0 || y > SizeGeneratior.WorldHeight - 1) return true;           
            return !MapTile[x, y].Active;
        }
        public void AddUpdateTile(int x, int y) => UpdateTile.Add(new Point(x, y));

        public void AddDoor(int id, int x, int y, bool isOpen)
        {
            if (!isOpen)
            {
                SetTexture(x, y, id, 2);
                SetTexture(x, y - 1, id, 1);
                SetTexture(x, y - 2, id);
            }
            else
            {
                id = id + 1;
                SetTexture(x, y, id, 2);
                SetTexture(x, y - 1, id, 1);
                SetTexture(x, y - 2, id);
                SetTexture(x+1, y, id, 2);
                SetTexture(x+1, y - 1, id, 1);
                SetTexture(x+1, y - 2, id);
            }
        }
        public static bool IsEmptyOrGround(int x, int y)
        {
            if (x < 0 || x >= SizeGeneratior.WorldWidth || y < 0 || y > SizeGeneratior.WorldHeight - 1) return true;

            return y < SizeGeneratior.RockLayer;
        }

        public List<Cloud> Cloud = new List<Cloud>();

        public void AddExplosion(Vector2 enemyPosition, Texture2D explosionTexture)
        {
            Animation explosionAnimation = new Animation(explosionTexture,
                enemyPosition,
                134,
                134,
                12,
                2000,
                ColorScheme.BaseColor, 
                1.0f,
                true);

            Explosion explosion = new Explosion(explosionAnimation, enemyPosition);

            _explosions.Add(explosion);
        }

        public virtual void Clear()
        {
            for (var i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                for (var j = 0; j < SizeGeneratior.WorldHeight; j++)
                {
                    MapTile[i, j] = new Tile.Tile(this);
                }
            }
            Active = false;
            StructuresList.Clear();
            Zombies.Clear();
            IsPortal = false;
        }

        /// <param name="reader">todo: describe reader parameter on Load</param>
        /// <param name="bar">todo: describe bar parameter on Load</param>
        /// <param name="version">todo: describe version parameter on Load</param>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ObjectDisposedException">The stream is closed. </exception>
        public virtual void Load(BinaryReader reader, ProgressBar bar,int version)
        {
           bar.Reset();
           bar.SetText("Load World");
            reader.ReadString();

            for (var i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                MapBiomes[i] = new Biome();
                MapBiomes[i].Read(reader);
                MapHeight[i] = reader.ReadInt32();
                for (var j = 0; j < SizeGeneratior.WorldHeight; j++)
                {
                    MapTile[i, j].Read(reader,version);
                }
                bar.Add(1);
            }
           /* Zombies.Clear();
            int countBlocks = reader.ReadInt32();
            for (int i = 0; i < countBlocks; i++)
            {
                Zombie zombie = new Zombie();
                zombie.load(reader);
                Zombies.Add(zombie);
            }
            civil.Clear();
            countBlocks = reader.ReadInt32();
            for (int i = 0; i < countBlocks; i++)
            {
                Civilian civ = new Civilian(Vector2.Zero);
                civ.load(reader);
                civil.Add(civ);
            }*/
        }

        /// <param name="writer">todo: describe writer parameter on Save</param>
        /// <param name="bar">todo: describe bar parameter on Save</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ArgumentNullException">
        ///         <paramref name="writer" /> is null. </exception>
        /// <exception cref="ObjectDisposedException">The stream is closed. </exception>
        public virtual void Save(BinaryWriter writer, ProgressBar bar)
        {
            bar.Reset();
            bar.SetText("SAVE world");
            writer.Write(@"World");
            for (var i = 0; i < SizeGeneratior.WorldWidth; i++) {
                MapBiomes[i].Save(writer);
                writer.Write(MapHeight[i]);
                for (var j = 0; j < SizeGeneratior.WorldHeight; j++)
                {
                    MapTile[i, j].Save(writer);
                }
                bar.Add(1);
            }

         /*   bar.setText("SAVE NPC");
            writer.Write(Zombies.Count);
            foreach (Zombie zombie in Zombies)
            {
                zombie.save(writer);
            }
            writer.Write(civil.Count);
            foreach (Civilian civ in civil)
            {
                civ.save(writer);
            }*/
        }

        public int GetTemperature(int x, int y)
        {
            if (x < 0) x = 0;
            if (x >= SizeGeneratior.WorldWidth) x = SizeGeneratior.WorldWidth - 1;
            if (y < 0) y = 0;
            if (y >= SizeGeneratior.WorldHeight) y = SizeGeneratior.WorldHeight - 1;
            return MapBiomes[x].Temperature + (int)(
                                                  MapTile[x, y].Light * 0.05f);
        }
        public void SetWallId(int x, int y,short id)
        {
            if ( x < 0 || y < 0 || x > SizeGeneratior.WorldWidth - 1 || y > SizeGeneratior.WorldHeight - 1 ) return;

            Reset(x, y);
            MapTile[x, y].IdWall = id;
        }
        public void Reset(int x, int y)
        {
            if ( x < 0 || y < 0 || x > SizeGeneratior.WorldWidth - 1 || y > SizeGeneratior.WorldHeight - 1 ) return;

            if (!MapTile[x, y].Active) return;
            RemoveFromUpdate(new Point(x, y));
                
            if (MapTile[x, y].IsLightBlock())
            {
                MapTile[x, y].Light = 0;
                for (var i = -20; i < 20; i++)
                {
                    for (var j = -20; j < 20; j++)
                    {
                        if ( x + i < 0 || y + j < 0 || x + i > SizeGeneratior.WorldWidth - 1 ||
                             y + j > SizeGeneratior.WorldHeight - 1 ) continue;

                        if (!MapTile[x + i, y + j].IsLightBlock()) MapTile[x + i, y + j].Light = 0;
                    }
                }
            }
            MapTile[x, y].Blockheight = 1;
            UpdateBlock(x, y, 0, 1);
            MapTile[x, y].Active = false;
            MapTile[x, y].IdTexture = 0;
        }

        /// <param name="x">todo: describe x parameter on UpdateBlock</param>
        /// <param name="y">todo: describe y parameter on UpdateBlock</param>
        /// <param name="type">todo: describe type parameter on UpdateBlock</param>
        /// <param name="step">todo: describe step parameter on UpdateBlock</param>
        /// <exception cref="ArgumentOutOfRangeException">Condition.</exception>
        public void UpdateBlock(int x , int y , int type , short step)
        {
            while ( true )
            {
                switch ( type )
                {
                    case 0:
                        if ( MapTile[x , y].IdTexture == 16 || MapTile[x , y].IdTexture == 15 || MapTile[x , y].IdTexture == 14 )
                        {
                            y = y + 1;
                            type = 1;
                            step = 1;
                            continue;
                        }

                        break;
                    case 1:
                        if ( MapTile[x , y].IdTexture == 15 || MapTile[x , y].IdTexture == 14 )
                        {
                            SetTextureOnly(x , y , 16);
                        }
                        else if ( MapTile[x , y].IdTexture == 13 )
                        {
                            UpdateBlock(x , y , 2 , 1);
                        }
                        MapTile[x , y].Blockheight = 1;
                        y = y + 1;
                        type = 2;
                        step = (short) (step + 1);
                        continue;
                    case 2:
                        MapTile[x , y].Blockheight = step;
                        if ( MapTile[x , y].IdTexture == 15 || MapTile[x , y].IdTexture == 14 || MapTile[x , y].IdTexture == 16 )
                        {
                            y = y + 1;
                            type = 2;
                            step = (short) (step + 1);
                            continue;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            }
        }

        public void UpdateMaxY(int x)
        {
            MapHeight[x] = 0;
            for (var i = 0; i < SizeGeneratior.WorldHeight; i++)
            {
                if ( !MapTile[x , i].IsSolid() ) continue;

                MapHeight[x] = i; break;
            }
        }
        public void SetTextureOnly(int x, int y, int id)
        {
            MapTile[x, y].IdTexture = (short)id;
        }
        public void RemoveFromUpdate(Point point){
            if (MapTile[point.X, point.Y].Active && IsUpdate(MapTile[point.X, point.Y].IdTexture))
            {
                    UpdateTile.Remove(point);
            }
        }
        public void SetTexture(int x, int y, int id,byte subId = 0)
        {
            if ( x < 0 || y < 0 || x > SizeGeneratior.WorldWidth - 1 || y > SizeGeneratior.WorldHeight - 1 ) return;

            RemoveFromUpdate(new Point(x, y));

            MapTile[x, y].IdTexture = (short)id;
            if (MapTile[x,y].IsLightBlock()) { 
                MapTile[x, y].Light = 15; 
                for (var i = -20; i < 20; i++)
                {
                    for (var j = -20; j < 20; j++)
                    {
                        if ( x + i < 0 || y + j < 0 || x + i > SizeGeneratior.WorldWidth - 1 ||
                             y + j > SizeGeneratior.WorldHeight - 1 ) continue;

                        if (!MapTile[x + i, y + j].IsLightBlock()) MapTile[x + i, y + j].Light = 0;
                    }
                }
            }
            MapTile[x, y].Active = true;
            MapTile[x, y].IdTexture = (short)id;
            MapTile[x, y].UpdateHp();
            MapTile[x, y].IdSubTexture = subId;
            MapTile[x, y].Frame = 0;
            MapTile[x, y].UpdateTileDate();
        }

        
        public int GetHeight(int w) => MapHeight[w];

        public virtual void Draw(Render render,ITileList tileList,Rectangle radious)
        {
            /*   foreach (Cloud cloud in cloud)
               {

                   spriteBatch.Draw(tiles.textures[1],
                       new Rectangle((int)(cloud.position.X), (int)(cloud.position.Y),
                           16,
                           16),
                           Color.White);
               }*/
            tileList.RenderPlasters(this, radious, render);
            tileList.RenderWall(this, radious, render);
            tileList.RenderTiles(this, radious, render);
            foreach (Explosion exp in _explosions)
            {
                exp.Draw(render);
            }
            foreach(Dust dust in Dusts)
            {
                dust.Render(render);
            }
        }
        
        public virtual void Update(GameTime gameTime,Camera camera)
        {

            foreach (Zombie npc in Zombies)
            {
                npc.Update();
                if ( !(npc.Hp <= 1) ) continue;

                npc.Kill();
                Zombies.Remove(npc);
                break;
            }
            foreach (Civilian npc in Civil) npc.Update();
            foreach ( Cloud cloud in Cloud )cloud.Update();

            _explosions = _explosions?.Where(exp => {exp.Update(gameTime);return exp.Active;}).ToList();
            foreach (Point point in UpdateTile)
                MapTile[point.X, point.Y].Update(point.X, point.Y, camera);

            foreach (Dust dust in Dusts)
                dust.Update();
        }
        public virtual void Start(ProgressBar bar)
        {
            bar.SetMaxValue(SizeGeneratior.WorldWidth);
            GenerationBiomes(bar);
            GeneratorHeight(bar);
            GenerationTerrain(bar);
            GenerationStructes(bar);
            Clearring(bar);
            Active = true;
            for (var i = 0; i < 1; i++)
            {
                Cloud.Add(new Cloud(new Vector2(i*10,SizeGeneratior.RockLayer-120)));
            }
        }

        protected virtual void Clearring(ProgressBar bar) { StructuresList.Clear(); }
        protected virtual void GenerationBiomes(ProgressBar progressBar) { }
        protected virtual void GenerationStructes(ProgressBar bar) { }

        /// <param name="bar">todo: describe bar parameter on GeneratorHeight</param>
        /// <exception cref="IOException">An I/O error occurred. </exception>
        protected virtual void GeneratorHeight(ProgressBar bar)
        {
            Console.WriteLine("Base");
        }
        protected virtual void GenerationTerrain(ProgressBar bar) { }
    }//концовка класса=
}
