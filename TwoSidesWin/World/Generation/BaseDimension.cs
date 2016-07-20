using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using System.Collections;
using TwoSides.World.Structures;
using TwoSides.Physics.Entity.NPC;
using TwoSides.World.Generation;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.Tiles;
using TwoSides.World.Tile;
using System.IO;
using TwoSides.GUI;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.Physics;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.Network;
using Lidgren.Network;
namespace TwoSides.World.Generation
{
    [Serializable]
    public class BaseDimension
    {
        public bool active { get;  set; }
        public float globallighting = 5;
        protected List<Point> UpdateTile = new List<Point>();
        public ArrayList Zombies = new ArrayList();
        public ArrayList civil = new ArrayList();
        List<Explosion>  explosions = new List<Explosion>();
        public int[] mapHeight = new int[SizeGeneratior.WorldWidth];
        public ITile[,] map = new ITile[SizeGeneratior.WorldWidth, SizeGeneratior.WorldHeight];
        public Biome[] mapB = new Biome[SizeGeneratior.WorldWidth];

        public Random rand = new Random();

        [NonSerialized]
        public bool isPortal;

        [NonSerialized]
        public ArrayList StructuresList = new ArrayList();

        public bool getEmptyOrNull(int x,int y){
            if (x < 0 || x >= SizeGeneratior.WorldWidth || y < 0 || y > SizeGeneratior.WorldHeight - 1) return true;

            if (!map[(int)x, (int)y].active) return true;
            return false;
        }
        public void addUpdate(int x, int y)
        {
            UpdateTile.Add(new Point(x, y));
        }
        public void addDoor(int id, int x, int y, bool isOpen)
        {
            if (!isOpen)
            {
                settexture(x, y, id, 2);
                settexture(x, y - 1, id, 1);
                settexture(x, y - 2, id, 0);
            }
            else
            {
                id = id + 1;
                settexture(x, y, id, 2);
                settexture(x, y - 1, id, 1);
                settexture(x, y - 2, id, 0);
                settexture(x+1, y, id, 2);
                settexture(x+1, y - 1, id, 1);
                settexture(x+1, y - 2, id, 0);
            }
        }
        public bool getEmptyOrGrass(int x, int y)
        {
            if (x < 0 || x >= SizeGeneratior.WorldWidth || y < 0 || y > SizeGeneratior.WorldHeight - 1) return true;

            if (y < SizeGeneratior.rockLayer) return true;
            return false;
        }

        public List<Cloud> cloud = new List<Cloud>();

        public void AddExplosion(Vector2 enemyPosition, Texture2D explosionTexture)
        {
            Animation explosionAnimation = new Animation(explosionTexture,
                enemyPosition,
                134,
                134,
                12,
                2000,
                Color.White,
                1.0f,
                true);

            Explosion explosion = new Explosion(explosionAnimation, enemyPosition);

            explosions.Add(explosion);
        }

        public virtual void cleardimension()
        {
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                for (int j = 0; j < SizeGeneratior.WorldHeight; j++)
                {
                    map[i, j] = new ITile(this);
                }
            }
            for (int j = 0; j < SizeGeneratior.WorldWidth; j++)
            {
                mapB[j] = new Biome();
            }
            active = false;
            StructuresList.Clear();
            Zombies.Clear();
            isPortal = false;
        }
        public virtual void load(BinaryReader reader, ProgressBar bar)
        {
           bar.Reset();
           bar.setText("Load World");
            reader.ReadString();

            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                mapB[i].read(reader);
                mapHeight[i] = reader.ReadInt32();
                for (int j = 0; j < SizeGeneratior.WorldHeight; j++)
                {
                    map[i, j].read(reader);
                }
                bar.Add(1);
            }
            Zombies.Clear();
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
            }

        }
        public virtual void save(BinaryWriter writer, ProgressBar bar)
        {
            bar.Reset();
            bar.setText("SAVE world");
            writer.Write("World");
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++) {
                mapB[i].save(writer);
                writer.Write(mapHeight[i]);
                for (int j = 0; j < SizeGeneratior.WorldHeight; j++)
                {
                    map[i, j].save(writer);
                }
                bar.Add(1);
            }

            bar.setText("SAVE NPC");
            writer.Write(Zombies.Count);
            foreach (Zombie zombie in Zombies)
            {
                zombie.save(writer);
            }
            writer.Write(civil.Count);
            foreach (Civilian civ in civil)
            {
                civ.save(writer);
            }
        }

        public int GetTemperature(int x, int y)
        {
            if (x < 0) x = 0;
            if (x >= SizeGeneratior.WorldWidth) x = SizeGeneratior.WorldWidth - 1;
            if (y < 0) y = 0;
            if (y >= SizeGeneratior.WorldHeight) y = SizeGeneratior.WorldHeight - 1;
            return (mapB[(int)x].Temperature + (int)(
                    map[(int)x, (int)y].light * 0.05f));
        }
        public void SetWallID(int x, int y,short id)
        {
            if (x >= 0 && y >= 0 && x <= SizeGeneratior.WorldWidth - 1 && y <= SizeGeneratior.WorldHeight - 1)
            {
                Reset(x, y);
                map[x, y].wallid = id;
            }
        }
        public void Reset(int x, int y)
        {
            if (x >= 0 && y >= 0 && x <= SizeGeneratior.WorldWidth - 1 && y <= SizeGeneratior.WorldHeight - 1)
            {
                if (map[x, y].active) removeFromUpdate(new Point(x, y));
                
                if (map[x, y].isLightBlock())
                {

                    map[x, y].light = 0;
                    for (int i = -20; i < 20; i++)
                    {
                        for (int j = -20; j < 20; j++)
                        {
                            if (x + i >= 0 && y + j >= 0 && x + i <= SizeGeneratior.WorldWidth - 1 && y + j <= SizeGeneratior.WorldHeight - 1)
                            {
                                if (!map[x + i, y + j].isLightBlock()) map[x + i, y + j].light = 0;
                            }
                        }
                    }
                }
                map[x, y].blockheight = 1;
                updateblock(x, y, 0, 1);
                map[x, y].active = false;
                map[x, y].idtexture = (short)0;
            }
        }
        
        public void updateblock(int x, int y, int type, int step)
        {
            if (type == 0)
            {
                if (map[x, y].idtexture == 16 || map[x, y].idtexture == 15 || map[x, y].idtexture == 14)
                {
                    updateblock(x, y + 1, 1, 1);
                }
            }
            else if (type == 1)
            {
                if (map[x, y].idtexture == 15 || map[x, y].idtexture == 14)
                {
                    settextureonly(x, y, 16);
                }
                else if (map[x, y].idtexture == 13)
                {
                    updateblock(x, y, 2, 1);
                }
                map[x, y].blockheight = 1;
                updateblock(x, y + 1, 2, step + 1);
            }
            else if (type == 2)
            {
                map[x, y].blockheight = step;
                if (map[x, y].idtexture == 15 || map[x, y].idtexture == 14 || map[x, y].idtexture == 16)
                {
                    updateblock(x, y + 1, 2, (step) + 1);
                }
            }
        }
        
        public void maxy(int x)
        {
            mapHeight[x] = 0;
            for (int i = 0; i < SizeGeneratior.WorldHeight; i++)
            {
                if (map[x, i].issolid()) { mapHeight[x] = i; break; }
            }
        }
        public void settextureonly(int x, int y, int id)
        {
            map[x, y].idtexture = (short)id;
        }
        public void removeFromUpdate(Point point){
            if(UpdateTile.Contains(point))
            {
                    UpdateTile.Remove(point);
            }
        }
        public void settexture(int x, int y, int id,byte subID = 0, bool inf = false)
        {
            if (x >= 0 && y >= 0 && x <= SizeGeneratior.WorldWidth - 1 && y <= SizeGeneratior.WorldHeight - 1)
            {
                if(map[x,y].active)removeFromUpdate(new Point(x, y));
                map[x, y].idtexture = (short)id;
                if (map[x,y].isLightBlock()) { 
                    map[x, y].light = 15; 
                    for (int i = -20; i < 20; i++)
                    {
                        for (int j = -20; j < 20; j++)
                        {
                            if (x + i >= 0 && y + j >= 0 && x + i <= SizeGeneratior.WorldWidth - 1 && y + j <= SizeGeneratior.WorldHeight - 1)
                            {
                                if (!map[x + i, y + j].isLightBlock()) map[x + i, y + j].light = 0;
                            }
                        }
                    }
                }
                map[x, y].infected = inf;
                map[x, y].active = true;
                map[x, y].idtexture = (short)id;
                map[x, y].updateHP();
                map[x, y].subTexture = subID;
                map[x, y].frame = 0;
            }
        }

        
        public int GetMax(int w)
        {
            return mapHeight[w];
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            foreach (Explosion exp in explosions)
            {
                exp.Draw(spriteBatch);
            }
        }
        public virtual void update(GameTime gameTime,Camera camera)
        {
            for (int i = 0; i < cloud.Count; i++)
            {
                cloud[i].update();
            }
            foreach (Explosion exp in explosions)
            {
                exp.Update(gameTime);
                if (!exp.Active)
                {
                    explosions.Remove(exp);
                    break;
                }
            }
            foreach (Point point in this.UpdateTile)
            {
                map[point.X, point.Y].Update(point.X, point.Y, camera);
            }
        }
        public virtual void start(ProgressBar bar)
        {
            bar.setMaxValue(SizeGeneratior.WorldWidth);
            GenerationBiomes(bar);
            GeneratorHeight(bar);
            GenerationTerrain(bar);
            GenerationStructes(bar);
            Clearring(bar);
            active = true;
            for (int i = 0; i < 1; i++)
            {
                cloud.Add(new Cloud(new Vector2(i*10,SizeGeneratior.rockLayer-120)));
            }
        }

        protected virtual void Clearring(ProgressBar bar) { StructuresList.Clear(); }
        protected virtual void GenerationBiomes(ProgressBar bar) { }
        protected virtual void GenerationStructes(ProgressBar bar) { }
        protected virtual void GeneratorHeight(ProgressBar bar)
        {
            System.Console.WriteLine("Base");
            System.Console.ReadLine();
        }
        protected virtual void GenerationTerrain(ProgressBar bar) { }

       
        
        public void updateblock(int frame)
        {
        }
    }//концовка класса=
}
