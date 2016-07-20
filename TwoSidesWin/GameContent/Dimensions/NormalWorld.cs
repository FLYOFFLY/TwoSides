using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World;
using TwoSides.Physics.Entity.NPC;
using TwoSides.World.Structures;
using Microsoft.Xna.Framework;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Generation;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.World.Tile;
using TwoSides.GUI;
using TwoSides.Physics.Entity;

namespace TwoSides.GameContent.Dimensions
{
    public class NormalWorld : BaseDimension
    {
        public Boss boss;
        public List<Wolf> wolfs = new List<Wolf>();
        Factory factory = null;
        ArrayList vectree = new ArrayList();
        public NormalWorld()
        {
            vectree.Clear();
        }
        public override void update(GameTime gameTime,Camera camera)
        {
            base.update(gameTime,camera);
            foreach (Wolf wolf in wolfs)
            {
                wolf.update();
            }
        }
        public void Cavinator(int i, int j, int steps)
        {
            double num = (double)Program.game.rand.Next(7, 15);
            int num2 = 1;
            if (Program.game.rand.Next(2) == 0)
            {
                num2 = -1;
            }
            Vector2 value;
            value.X = (float)i;
            value.Y = (float)j;
            int k = Program.game.rand.Next(20, 40);
            Vector2 value2;
            value2.Y = (float)Program.game.rand.Next(10, 20) * 0.01f;
            value2.X = (float)num2;
            while (k > 0)
            {
                k--;
                int num3 = (int)((double)value.X - num * 0.5);
                int num4 = (int)((double)value.X + num * 0.5);
                int num5 = (int)((double)value.Y - num * 0.5);
                int num6 = (int)((double)value.Y + num * 0.5);
                if (num3 < 0)
                {
                    num3 = 0;
                }
                if (num4 >= SizeGeneratior.WorldWidth - 1)
                {
                    num4 = SizeGeneratior.WorldWidth - 1;
                }
                if (num5 < 0)
                {
                    num5 = 0;
                }
                if (num6 >= SizeGeneratior.WorldHeight - 1)
                {
                    num6 = SizeGeneratior.WorldHeight - 1;
                };
                double num7 = num * (double)Program.game.rand.Next(80, 120) * 0.01;
                for (int l = num3; l < num4; l++)
                {
                    for (int m = num5; m < num6; m++)
                    {
                        float num8 = Math.Abs((float)l - value.X);
                        float num9 = Math.Abs((float)m - value.Y);
                        double num10 = Math.Sqrt((double)(num8 * num8 + num9 * num9));
                        if (num10 < num7 * 0.4)
                        {
                            Reset(l, m);
                        }
                    }
                }
                value += value2;
                value2.X += (float)Program.game.rand.Next(-10, 11) * 0.05f;
                value2.Y += (float)Program.game.rand.Next(-10, 11) * 0.05f;
                if (value2.X > (float)num2 + 0.5f)
                {
                    value2.X = (float)num2 + 0.5f;
                }
                if (value2.X < (float)num2 - 0.5f)
                {
                    value2.X = (float)num2 - 0.5f;
                }
                if (value2.Y > 2f)
                {
                    value2.Y = 2f;
                }
                if (value2.Y < 0f)
                {
                    value2.Y = 0f;
                }
            }
            if (steps > 0 && ((int)value.X < 0 || (int)value.X >= SizeGeneratior.WorldWidth || (double)((int)value.Y) >= mapHeight[(int)value.X]) && (double)((int)value.Y) > SizeGeneratior.rockLayer)
            {
                Cavinator((int)value.X, (int)value.Y, steps - 1);
            }
        }

        public void AddTree(int x, int y)
        {
            int size = rand.Next(1, 4);
            if (mapB[x] == ArrayResource.grass)
            {
                for (int i = -size; i < size; i++)
                {
                    if (x + i < 0) continue;
                    if (x + i >= SizeGeneratior.WorldWidth) continue;
                    if (mapHeight[x + i] != y + 1) continue;
                    if (map[x + i, y].active) continue;
                    //Reset(x + i, y);
                    //map[x + i, y].water = 180;
                    if (rand.Next(0, 2) == 0)
                    {
                        settexture(x + i, y, 27, (byte)rand.Next(0, 2));
                        UpdateTile.Add(new Point(x + i, y));
                    }
                    else if (rand.Next(0, 5) == 0) settexture(x + i, y, 29, (byte)rand.Next(0, 3));
                    //else map[x + i, y].waterType = 2;
                }
            }
            byte subid = 0;
            if (mapB[x] == ArrayResource.snow || mapB[x] == ArrayResource.worldshow) subid = 1;
            settexture(x, y, 13,subid);
            int b = Program.game.rand.Next(2, 5);
            settexture(x, y - 1, 14, subid);
            map[x, y].blockheight = 1;
            map[x, y - 1].blockheight = b + 1;
            for (int i = 0; i < b; i++)
            {
                settexture(x, y - 2 - i, 15, subid);
                map[x, y - 2 - i].blockheight = (b - i) + 2;
            }
            settexture(x, y - 2 - b, 16, subid);
            // map[x, y].blockheight = 2;
            updateblock(x, y - 2 - b, 2, 1);
            //liquid.Add(new Liquid(new Vector2(x*16,(y-2-b-1)*16)));
        }
        protected override void Clearring(ProgressBar bar)
        {
            base.Clearring(bar);
            vectree.Clear();
        }
        public void CaveOpenater(int i, int j)
        {
            double num = (double)Program.game.rand.Next(7, 12);
            int num2 = 1;
            if (Program.game.rand.Next(2) == 0)
            {
                num2 = -1;
            }
            Vector2 value;
            value.X = (float)i;
            value.Y = (float)j;
            int k = 100;
            Vector2 value2;
            value2.Y = 0f;
            value2.X = (float)num2;
            while (k > 0)
            {
                if ((int)value2.Y < SizeGeneratior.rockLayer)
                {
                    k = 0;
                }
                k--;
                int num3 = (int)((double)value.X - num * 0.5);
                int num4 = (int)((double)value.X + num * 0.5);
                int num5 = (int)((double)value.Y - num * 0.5);
                int num6 = (int)((double)value.Y + num * 0.5);
                if (num3 < 0)
                {
                    num3 = 0;
                }
                if (num4 >= SizeGeneratior.WorldWidth)
                {
                    num4 = SizeGeneratior.WorldWidth - 1;
                }
                if (num5 < 0)
                {
                    num5 = 0;
                }
                if (num6 >= SizeGeneratior.WorldHeight)
                {
                    num6 = SizeGeneratior.WorldHeight - 1;
                }
                double num7 = num * (double)Program.game.rand.Next(80, 120) * 0.01;
                for (int l = num3; l < num4; l++)
                {
                    for (int m = num5; m < num6; m++)
                    {
                        float num8 = Math.Abs((float)l - value.X);
                        float num9 = Math.Abs((float)m - value.Y);
                        double num10 = Math.Sqrt((double)(num8 * num8 + num9 * num9));
                        if (num10 < num7 * 0.4)
                        {
                            Reset(l, m);
                        }
                    }
                }
                value += value2;
                value2.X += (float)rand.Next(-10, 11) * 0.05f;
                value2.Y += (float)rand.Next(-10, 11) * 0.05f;
                if (value2.X > (float)num2 + 0.5f)
                {
                    value2.X = (float)num2 + 0.5f;
                }
                if (value2.X < (float)num2 - 0.5f)
                {
                    value2.X = (float)num2 - 0.5f;
                }
                if (value2.Y > 0f)
                {
                    value2.Y = 0f;
                }
                if ((double)value2.Y < -0.5)
                {
                    value2.Y = -0.5f;
                }
            }
        }
        List<Point> biomesIDs = new List<Point>();
        protected override void GenerationBiomes(ProgressBar bar)
        {

            int changeBiome = 0;
            int currentBiome = 0;
            bar.Reset();
            bar.setText("Generation Biomes");
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                if (changeBiome == 0)
                {
                    if (rand.Next(0, 100) < 50) currentBiome = 0;
                    else if (rand.Next(0, 100) < 50) currentBiome = 1;
                    else if (rand.Next(0, 100) < 50) currentBiome = 2;
                    else
                    {
                        if(rand.Next(0,100) < 50)    currentBiome = 3;
                        else currentBiome = 3+rand.Next(ModLoader.ModLoader.BiomesMods.Count);
                    } changeBiome = rand.Next(100, 250);
                    currentBiome = 0;
                    biomesIDs.Add(new Point(i,currentBiome));
                }
                else changeBiome--;
                if (currentBiome == 0) mapB[i] = ArrayResource.grass;
                if (currentBiome == 1) mapB[i] = ArrayResource.Hills;
                if (currentBiome == 2) mapB[i] = ArrayResource.snow;
                if (currentBiome == 3) mapB[i] = ArrayResource.Desrt;
                if(currentBiome >= 4) mapB[i] = ModLoader.ModLoader.BiomesMods[currentBiome-4];
                bar.Add(1);
            }
           /* bool stop = false;

            do
            {
                stop = false;
                int count = biomesIDs.Count - 2;
                for (int i = 0; i < count; i++)
                {
                    int idBiome = biomesIDs[i].Y;
                    int idBiomeNext = biomesIDs[i + 1].Y;
                    if (idBiome == idBiomeNext)
                    {
                        biomesIDs.RemoveAt(i + 1);
                        stop = true;
                        break;
                    }
                }
            } while (stop); 
            stop = false;

            do
            {
                stop = false;
                int count = biomesIDs.Count - 2;
                for (int i = 0; i < count; i++)
                {
                    int idBiome = biomesIDs[i].Y;
                    int idBiomeNextNext = biomesIDs[i + 2].Y;
                    if (idBiome == idBiomeNextNext)
                    {
                        for (int x = biomesIDs[i + 1].X; x < biomesIDs[i + 2].X; x++)
                        {
                            currentBiome = idBiome;
                            if (currentBiome == 0) mapB[x] = ArrayResource.grass;
                            if (currentBiome == 1) mapB[x] = ArrayResource.Hills;
                            if (currentBiome == 2) mapB[x] = ArrayResource.snow;
                            if (currentBiome == 3) mapB[x] = ArrayResource.Desrt;

                        }
                        biomesIDs.RemoveAt(i + 1);
                        stop = true;
                        break;
                    }

                }
            } while (stop);*/
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Wolf wolf in wolfs)
            {
                wolf.DrawNPC(SpriteEffects.None,spriteBatch,Program.game.Font1,Program.game.tiles.textures[0]);
            }
        }
        protected override void GeneratorHeight(ProgressBar bar)
        {
            mapHeight[0] = rand.Next(mapB[0].minHeight,mapB[0].maxHeight);//=50
            rand = new Random((int)DateTime.Now.Ticks);
            int currenttemp = 0;
            int maxcurrent = rand.Next(5, 8);
            int nextcurrent = rand.Next(5, 7);
            bool[] min = new bool[2];
            bar.Reset();
            bar.setText("Generation HeightMap");
            for (int i = 1; i < SizeGeneratior.WorldWidth; i++)
            {
                min[1] = min[0];
                min[0] = false;
                int temp = 1;
                temp = rand.Next(0, 3);
                mapHeight[i] = mapHeight[i - 1];
                if (temp == 1 && mapHeight[i] > 0)
                {
                    if (mapHeight[i] > mapB[i].minHeight)
                        mapHeight[i]--;
                    min[0] = true;
                }
                else if ((temp == 2 && mapHeight[i] < SizeGeneratior.rockLayer - 2) || (mapHeight[i] <= 2))
                {
                    if (mapHeight[i] < mapB[i].maxHeight)
                    {
                        mapHeight[i]++;
                    }
                }
                currenttemp++;
                bar.Add(1);
            }

            if (Program.game.currentD != 1) smooth(bar);
        }

        public void smooth(ProgressBar bar)
        {
            bar.Reset();
            bar.setText("Smooth Map");
            for (int i = 0; i + 2 < SizeGeneratior.WorldWidth; i++)
            {
                for (int j = i + 5; j > i; j--)
                {
                    if (j + 1 < SizeGeneratior.WorldWidth)
                    {
                        if (mapHeight[i] == mapHeight[j + 1]) mapHeight[j] = mapHeight[i];
                    }
                }
                bar.Add(1);
            }
        }
        protected override void GenerationTerrain(ProgressBar bar)
        {
            bar.Reset();
            bar.setText("Generation Terrain");
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                for (int j = mapHeight[i]; j < SizeGeneratior.WorldHeight; j++)
                {
                    if (j > mapHeight[i]+6)
                    {
                        GeneratorOre(i, j);
                    }
                }
                bar.Add(1);
            }
            for (int i = 0; i < biomesIDs.Count-1; i++)
            {
                int BiomeX = biomesIDs[i].X;
                int BiomeNeX = biomesIDs[i + 1].X;
                int width = BiomeNeX - BiomeX;

                int idBiome = biomesIDs[i].Y;

                if (idBiome == 0) ArrayResource.grass.Place(BiomeX, this, width, mapHeight); ;
                if (idBiome == 1) ArrayResource.Hills.Place(BiomeX, this, width, mapHeight); ;
                if (idBiome == 2) ArrayResource.snow.Place(BiomeX, this, width, mapHeight); ;
                if (idBiome == 3) ArrayResource.Desrt.Place(BiomeX, this, width, mapHeight); ;
                if (idBiome >= 4) ModLoader.ModLoader.BiomesMods[idBiome-4].Place(BiomeX, this, width, mapHeight); ;
            }
            biomesIDs.Clear();
        }

        protected void GeneratorOre(int x, int y)
        {
            bool final = false;
            foreach (Ore ore in ArrayResource.ores)
            {
                if (y < ore.minY || y >= ore.maxY) continue;
                if (rand.Next(0, 100) < ore.change)
                {
                    spawnOre(x, y, ore);
                    final = true;
                    break;
                }
            }
            if (!final)
            {
                foreach (Ore ore in ModLoader.ModLoader.OreMods)
                {
                    if (y < ore.minY || y >= ore.maxY) continue;
                    if (rand.Next(0, 100) < ore.change)
                    {
                        spawnOre(x, y, ore);
                        final = true;
                        break;
                    }
                }
            }
        }
         
        protected void spawnOre(int x, int y,Ore ore)
        {
            int countOre = 0;
            while (countOre < ore.range)
            {
                for (int i = x - 3; i < x + 3; i++)
                {
                    if (i < 0 || i >= SizeGeneratior.WorldWidth) continue;
                    for (int j = y - 3; j < y + 3; j++)
                    {
                        if (j < ore.minY || j >= ore.maxY) continue;
                        if (countOre >= ore.range) break;
                        if (!map[i, j].active)
                        {
                            settexture(i, j, ore.idblock);
                            if (mapB[i] == ArrayResource.Hills) map[i, j].subTexture = 1;
                            else if (mapB[i] == ArrayResource.Desrt) map[i, j].subTexture = 2;
                        } countOre++;
                    }
                    if (countOre >= ore.range) break;
                }
            }
        }
        
        protected override void GenerationStructes(ProgressBar bar)
        {
            bar.Reset();
            bar.setText("Generation Structures");
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                if (i + 9 < SizeGeneratior.WorldWidth)
                {
                    if (mapHeight[i] == mapHeight[i + 9])
                    {
                        if ((rand.Next(0, 20) == 0 && mapB[i] == ArrayResource.grass) ||
                            (rand.Next(0, 60) == 0 && mapB[i] == ArrayResource.Desrt)
                            )
                        {
                            if (StructuresList.Count >= 1)
                            {
                                bool test = false;
                                for (int sID = StructuresList.Count - 1; sID > 0; sID--)
                                {
                                    if (StructuresList[sID] is Home)
                                    {
                                        test = true;
                                        if (i > ((Home)StructuresList[sID]).x + 15)
                                            StructuresList.Add(new Home(i, mapHeight[i]));
                                        break;
                                    }
                                }
                                if (!test) StructuresList.Add(new Home(i, mapHeight[i]));
                            }
                            else if (i > 15) StructuresList.Add(new Home(i, mapHeight[i]));
                        }
                        else if (Program.game.currentD == 0)
                        {
                            if (rand.Next(0, 1) <= 0.9f)
                            {
                                if (factory == null)
                                {
                                    if (i >= 40 && i < SizeGeneratior.WorldWidth - 40) factory = new Factory(i, mapHeight[i]);
                                }
                            }
                        }
                    }
                }

                int tree = rand.Next(0, 3);
                if (tree == 1 && (mapB[i] == ArrayResource.grass || mapB[i] == ArrayResource.snow )) vectree.Add(new Vector2(i, mapHeight[i] - 1));
                if (rand.Next(0, 10) == 0 && mapB[i] == ArrayResource.Desrt) vectree.Add(new Vector2(i, mapHeight[i] - 1));
                for (int j = SizeGeneratior.rockLayer; j < SizeGeneratior.WorldHeight; j++)
                {
                    int airspawn = rand.Next(0, 800);

                    if (airspawn == 0 && Program.game.currentD != 1)
                    {
                        StructuresList.Add(new Caves(i, j));
                    }
                }
                bar.Add(1);
            }
            bar.Reset();
            bar.setText("Spawn Tree and cactus");
            bar.setMaxValue(vectree.Count);
            foreach (Vector2 vec in vectree)
            {
                if (mapB[(int)vec.X] == ArrayResource.Desrt)
                {
                    int height = rand.Next(2, 5);
                    for (int i = 0; i < height; i++)
                    {
                        settexture((int)vec.X, (int)vec.Y - i, 28);
                    }
                    settexture((int)vec.X, (int)vec.Y - height, 28,1);
                }
                else AddTree((int)vec.X, (int)vec.Y);
                bar.Add(1);
            }
            if (Program.game.currentD != 1) StructuresList.Add(new Home(0, mapHeight[5], true));
            bar.Reset();
            bar.setText("Place Structure");
            bar.setMaxValue(StructuresList.Count);
            foreach (BaseStruct structure in StructuresList)
            {
                structure.spawn(this);
                bar.Add(1);
            }
            if (factory != null) factory.spawn(this);
        }
    }
}
