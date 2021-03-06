﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GameContent.Entity.NPC;
using TwoSides.GameContent.GenerationResources.Structures;
using TwoSides.GameContent.Tiles;
using TwoSides.GUI;
using TwoSides.Physics.Entity.NPC;
using TwoSides.World.Generation.Structures;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Generation;

namespace TwoSides.GameContent.Dimensions
{
    public class NormalWorld : BaseDimension
    {
        public Boss Boss;
        public List<BaseNpc> Npcs = new List<BaseNpc>();
        Factory _factory;
        readonly List<Vector2> _vectree = new List<Vector2>();
        public NormalWorld()
        {
            _vectree.Clear();
        }
        public override bool IsUpdate(int idTextutre) => idTextutre == 27 || idTextutre == 32;

        public override void Update(GameTime gameTime,Camera camera)
        {
            base.Update(gameTime,camera);
            foreach (BaseNpc npc in Npcs)
            {
                npc.Update();
                if ( !(npc.Hp <= 1) ) continue;

                npc.Kill();
                Npcs.Remove(npc);
                break;
            }
        }

        public void Cavinator(int i , int j , int steps)
        {
            while ( true )
            {
                double scaleHoles = Program.Game.Rand.Next(7 , 15);
                var offsetHolesX = 1;
                if ( Program.Game.Rand.Next(2) == 0 )
                {
                    offsetHolesX = -1;
                }
                Vector2 posHoles;
                posHoles.X = i;
                posHoles.Y = j;
                var lenghtHoles = Program.Game.Rand.Next(20 , 40);
                Vector2 offsetHoles;
                offsetHoles.Y = Program.Game.Rand.Next(10 , 20) * 0.01f;
                offsetHoles.X = offsetHolesX;
                while ( lenghtHoles > 0 )
                {
                    lenghtHoles--;
                    var tileLeft = (int) (posHoles.X - scaleHoles * 0.5);
                    var tileRight = (int) (posHoles.X + scaleHoles * 0.5);
                    var tileTop = (int) (posHoles.Y - scaleHoles * 0.5);
                    var tileBottom = (int) (posHoles.Y + scaleHoles * 0.5);
                    if ( tileLeft < 0 )
                    {
                        tileLeft = 0;
                    }
                    if ( tileRight >= SizeGeneratior.WorldWidth - 1 )
                    {
                        tileRight = SizeGeneratior.WorldWidth - 1;
                    }
                    if ( tileTop < 0 )
                    {
                        tileTop = 0;
                    }
                    if ( tileBottom >= SizeGeneratior.WorldHeight - 1 )
                    {
                        tileBottom = SizeGeneratior.WorldHeight - 1;
                    }
                    var radius = scaleHoles * Program.Game.Rand.Next(80 , 120) * 0.01;
                    for ( var l = tileLeft ; l < tileRight ; l++ )
                    {
                        for ( var m = tileTop ; m < tileBottom ; m++ )
                        {
                            var lenghtX = Math.Abs(l - posHoles.X);
                            var lenghtY = Math.Abs(m - posHoles.Y);
                            var lenghtSqrt = Math.Sqrt(lenghtX * lenghtX + lenghtY * lenghtY);
                            if ( lenghtSqrt < radius * 0.4 )
                            {
                                Reset(l , m);
                            }
                        }
                    }

                    posHoles += offsetHoles;
                    offsetHoles.X += Program.Game.Rand.Next(-10 , 11) * 0.05f;
                    offsetHoles.Y += Program.Game.Rand.Next(-10 , 11) * 0.05f;
                    if ( offsetHoles.X > offsetHolesX + 0.5f )
                    {
                        offsetHoles.X = offsetHolesX + 0.5f;
                    }
                    if ( offsetHoles.X < offsetHolesX - 0.5f )
                    {
                        offsetHoles.X = offsetHolesX - 0.5f;
                    }
                    if ( offsetHoles.Y > 2f )
                    {
                        offsetHoles.Y = 2f;
                    }
                    if ( offsetHoles.Y < 0f )
                    {
                        offsetHoles.Y = 0f;
                    }
                }

                if ( steps > 0 && ((int) posHoles.X < 0 || (int) posHoles.X >= SizeGeneratior.WorldWidth || (double) (int) posHoles.Y >= MapHeight[(int) posHoles.X]) && (double) (int) posHoles.Y > SizeGeneratior.RockLayer )
                {
                    i = (int) posHoles.X;
                    j = (int) posHoles.Y;
                    steps = steps - 1;
                    continue;
                }

                break;
            }
        }

        public void AddTree(int x, int y)
        {
            var size = Rand.Next(1, 4);
            if (Equals(MapBiomes[x] , ArrayResource.Grass))
            {
                for (var i = -size; i < size; i++)
                {
                    if (x + i < 0) continue;
                    if (x + i >= SizeGeneratior.WorldWidth) continue;
                    if (MapHeight[x + i] != y + 1) continue;
                    if (MapTile[x + i, y].Active) continue;
                    //Reset(x + i, y);
                    //map[x + i, y].water = 180;
                    if (Rand.Next(0, 2) == 0)
                    {
                        SetTexture(x + i, y, 27, (byte)Rand.Next(0, 2));
                        UpdateTile.Add(new Point(x + i, y));
                    }
                    else if (Rand.Next(0, 5) == 0) SetTexture(x + i, y, 29, (byte)Rand.Next(0, 3));
                    //else map[x + i, y].waterType = 2;
                }
            }
            //if (MapBiomes[x] == ArrayResource.snow || MapBiomes[x] == ArrayResource.worldshow) subId = 1;
            SetTexture(x, y, 13);
            var b = Program.Game.Rand.Next(2, 5);
            SetTexture(x, y - 1, 14);
            MapTile[x, y].Blockheight = 1;
            MapTile[x, y - 1].Blockheight = (short)(b + 1);
            for (var i = 0; i < b; i++)
            {
                SetTexture(x, y - 2 - i, 15);
                MapTile[x, y - 2 - i].Blockheight = (short)(b - i + 2);
                if ( i < b / 2 || i >= b - 1 || Rand.Next(100) > 40 ) continue;

                SetTexture(x+1, y - 2 - i, 38);
                if(x+1<MapTile.GetUpperBound(0))
                    ((TreeDate)MapTile[x+1, y - 2 - i].TileDate).TypeTree = (short)Rand.Next(0, 2);
            }
            SetTexture(x, y - 2 - b, 16);
            ((TreeDate)MapTile[x, y - 2 - b].TileDate).TypeTree = (short)Rand.Next(0, 3);
            // map[x, y].blockheight = 2;
            UpdateBlock(x, y - 2 - b, 2, 1);
            //liquId.Add(new LiquId(new Vector2(x*16,(y-2-b-1)*16)));
        }
        protected override void Clearring(ProgressBar bar)
        {
            base.Clearring(bar);
            _vectree.Clear();
        }
        public void CaveOpenater(int i, int j)
        {
            double scaleHoles = Program.Game.Rand.Next(7, 12);
            var num2 = 1;
            if (Program.Game.Rand.Next(2) == 0)
            {
                num2 = -1;
            }
            Vector2 posCaves;
            posCaves.X = i;
            posCaves.Y = j;
            var k = 100;
            Vector2 directionCaves;
            directionCaves.Y = 0f;
            directionCaves.X = num2;
            while (k > 0)
            {
                if ((int)directionCaves.Y < SizeGeneratior.RockLayer)
                {
                    k = 0;
                }
                k--;
                var tileLeft = (int)(posCaves.X - scaleHoles * 0.5);
                var tileRight = (int)(posCaves.X + scaleHoles * 0.5);
                var tileTop = (int)(posCaves.Y - scaleHoles * 0.5);
                var tileBottom = (int)(posCaves.Y + scaleHoles * 0.5);
                if (tileLeft < 0)
                {
                    tileLeft = 0;
                }
                if (tileRight >= SizeGeneratior.WorldWidth)
                {
                    tileRight = SizeGeneratior.WorldWidth - 1;
                }
                if (tileTop < 0)
                {
                    tileTop = 0;
                }
                if (tileBottom >= SizeGeneratior.WorldHeight)
                {
                    tileBottom = SizeGeneratior.WorldHeight - 1;
                }
                var radius = scaleHoles * Program.Game.Rand.Next(80, 120) * 0.01;
                for (var l = tileLeft; l < tileRight; l++)
                {
                    for (var m = tileTop; m < tileBottom; m++)
                    {
                        var lenX = Math.Abs(l - posCaves.X);
                        var lenY = Math.Abs(m - posCaves.Y);
                        var lenSqrt = Math.Sqrt(lenX * lenX + lenY * lenY);
                        if (lenSqrt < radius * 0.4)
                        {
                            Reset(l, m);
                        }
                    }
                }
                posCaves += directionCaves;
                directionCaves.X += Rand.Next(-10, 11) * 0.05f;
                directionCaves.Y += Rand.Next(-10, 11) * 0.05f;
                if (directionCaves.X > num2 + 0.5f)
                {
                    directionCaves.X = num2 + 0.5f;
                }
                if (directionCaves.X < num2 - 0.5f)
                {
                    directionCaves.X = num2 - 0.5f;
                }
                if (directionCaves.Y > 0f)
                {
                    directionCaves.Y = 0f;
                }
                if (directionCaves.Y < -0.5)
                {
                    directionCaves.Y = -0.5f;
                }
            }
        }
        readonly List<Point> _biomesIDs = new List<Point>();
        protected override void GenerationBiomes(ProgressBar progressBar)
        {

            var changeBiome = 0;
            var currentBiome = 0;
            progressBar.Reset();
            progressBar.SetText("Generation Biomes");
            for (var i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                if (changeBiome == 0)
                {
                    if (Rand.Next(0, 100) < 50) currentBiome = 0; //-V3003
                    else if (Rand.Next(0, 100) < 50) currentBiome = 1; //-V3003
                    else if (Rand.Next(0, 100) < 50) currentBiome = 2;
                    else
                        currentBiome = 3; //-V3030
                    changeBiome = Rand.Next(100, 250);
                    _biomesIDs.Add(new Point(i,currentBiome));
                }
                else changeBiome--;
                switch ( currentBiome ) {
                    case 0:
                        MapBiomes[i] = ArrayResource.Grass;
                        break;
                    case 1:
                        MapBiomes[i] = ArrayResource.Hills;
                        break;
                    case 2:
                        MapBiomes[i] = ArrayResource.Snow;
                        break;
                    case 3:
                        MapBiomes[i] = ArrayResource.Desrt;
                        break;
                }

                progressBar.Add(1);
            }
           /* bool stop = false;

            do
            {
                stop = false;
                int count = biomesIDs.Count - 2;
                for (int i = 0; i < count; i++)
                {
                    int IdBiome = biomesIDs[i].Y;
                    int IdBiomeNext = biomesIDs[i + 1].Y;
                    if (IdBiome == IdBiomeNext)
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
                    int IdBiome = biomesIDs[i].Y;
                    int IdBiomeNextNext = biomesIDs[i + 2].Y;
                    if (IdBiome == IdBiomeNextNext)
                    {
                        for (int x = biomesIDs[i + 1].X; x < biomesIDs[i + 2].X; x++)
                        {
                            currentBiome = IdBiome;
                            if (currentBiome == 0) MapBiomes[x] = ArrayResource.grass;
                            if (currentBiome == 1) MapBiomes[x] = ArrayResource.Hills;
                            if (currentBiome == 2) MapBiomes[x] = ArrayResource.snow;
                            if (currentBiome == 3) MapBiomes[x] = ArrayResource.Desrt;

                        }
                        biomesIDs.RemoveAt(i + 1);
                        stop = true;
                        break;
                    }

                }
            } while (stop);*/
        }
        public override void Draw(Render render, ITileList tileList, Rectangle radious)
        {
            base.Draw(render,tileList,radious);

            foreach (BaseNpc npc in Npcs)
            {
                npc.RenderNpc(render,Program.Game.Font1,Program.Game.Tiles.Textures[0]);
            }
        }
        protected override void GeneratorHeight(ProgressBar bar)
        {
            MapHeight[0] = Rand.Next(MapBiomes[0].MinHeight,MapBiomes[0].MaxHeight);//=50
            Rand = new Random((int)DateTime.Now.Ticks);
            bool[] min = new bool[2];
            bar.Reset();
            bar.SetText("Generation HeightMap");
            for (var i = 1; i < SizeGeneratior.WorldWidth; i++)
            {
                min[1] = min[0];
                min[0] = false;
                var temp  = Rand.Next(0, 3);
                MapHeight[i] = MapHeight[i - 1];
                if (temp == 1 && MapHeight[i] > 0)
                {
                    if (MapHeight[i] > MapBiomes[i].MinHeight)
                        MapHeight[i]--;
                    min[0] = true;
                }
                else if (temp == 2 && MapHeight[i] < SizeGeneratior.RockLayer - 2 || MapHeight[i] <= 2)
                {
                    if (MapHeight[i] < MapBiomes[i].MaxHeight)
                    {
                        MapHeight[i]++;
                    }
                }
                bar.Add(1);
            }

            if (Program.Game.CurrentDimension != 1) Smooth(bar);
        }

        public void Smooth(ProgressBar bar)
        {
            bar.Reset();
            bar.SetText("Smooth Map");
            for (var i = 0; i + 2 < SizeGeneratior.WorldWidth; i++)
            {
                for (var j = i + 5; j > i; j--)
                {
                    if ( j + 1 >= SizeGeneratior.WorldWidth ) continue;

                    if (MapHeight[i] == MapHeight[j + 1]) MapHeight[j] = MapHeight[i];
                }
                bar.Add(1);
            }
        }

        protected override void GenerationTerrain(ProgressBar bar)
        {
            bar.Reset();
            bar.SetText("Generation Terrain");
            for (var i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                for (var j = MapHeight[i]; j < SizeGeneratior.WorldHeight; j++)
                {
                    if (j > MapHeight[i] + 6)
                    {
                        GeneratorOre(i, j);
                    }
                }
                bar.Add(1);
            }
            for (var i = 0; i < _biomesIDs.Count - 1; i++)
            {
                var biomeCurrentX = _biomesIDs[i].X;
                var biomeNextX = _biomesIDs[i + 1].X;
                var wIdth = biomeNextX - biomeCurrentX;

                var idBiome = _biomesIDs[i].Y;

                switch ( idBiome ) {
                    case 0:
                        ArrayResource.Grass.Place(biomeCurrentX, this, wIdth, MapHeight);
                        break;
                    case 1:
                        ArrayResource.Hills.Place(biomeCurrentX, this, wIdth, MapHeight);
                        break;
                    case 2:
                        ArrayResource.Snow.Place(biomeCurrentX, this, wIdth, MapHeight);
                        break;
                    case 3:
                        ArrayResource.Desrt.Place(biomeCurrentX, this, wIdth, MapHeight);
                        break;
                }
            }
            _biomesIDs.Clear();
        }

        protected void GeneratorOre(int x, int y)
        {
            foreach (Ore ore in ArrayResource.Ores)
            {
                if (y < ore.MinY || y >= ore.MaxY) continue;
                if ( Rand.Next(0 , 100) >= ore.Change ) continue;

                SpawnOre(x, y, ore);
                break;
            }
        }
         
        protected void SpawnOre(int x, int y,Ore ore)
        {
            var countOre = 0;
            while (countOre < ore.Range)
            {
                for (var i = x - 3; i < x + 3; i++)
                {
                    if (i < 0 || i >= SizeGeneratior.WorldWidth) continue;
                    for (var j = y - 3; j < y + 3; j++)
                    {
                        if (j < ore.MinY || j >= ore.MaxY) continue;
                        if (countOre >= ore.Range) break;
                        if (!MapTile[i, j].Active)
                        {
                            SetTexture(i, j, ore.Idblock);
                            if (ReferenceEquals(MapBiomes[i] , ArrayResource.Hills))
                                MapTile[i, j].IdSubTexture = 1;
                            else if (ReferenceEquals(MapBiomes[i] , ArrayResource.Desrt))
                                MapTile[i, j].IdSubTexture = 2;
                        } countOre++;
                    }
                    if (countOre >= ore.Range) break;
                }
            }
        }

        protected override void GenerationStructes(ProgressBar bar)
        {
            bar.Reset();
            bar.SetText("Generation Structures");
            GeneratorStructurePosition(bar);
            PlaceTree(bar);
            PlaceStructures(bar);

            _factory?.Spawn(this);
        }

        void PlaceStructures(ProgressBar bar)
        {
            bar.Reset();
            bar.SetText("Place Structure");
            bar.SetMaxValue(StructuresList.Count);
            foreach ( BaseStruct structure in StructuresList )
            {
                structure.Spawn(this);
                bar.Add(1);
            }
        }

        void PlaceTree(ProgressBar bar)
        {
            bar.Reset();
            bar.SetText("Spawn Tree and cactus");
            bar.SetMaxValue(_vectree.Count);
            foreach ( Vector2 vec in _vectree )
            {
                if ( ReferenceEquals(MapBiomes[(int) vec.X] , ArrayResource.Desrt) )
                {
                    var height = Rand.Next(2 , 5);
                    for ( var i = 0 ; i < height ; i++ )
                    {
                        SetTexture((int) vec.X , (int) vec.Y - i , 28);
                    }

                    SetTexture((int) vec.X , (int) vec.Y - height , 28 , 1);
                }
                else AddTree((int) vec.X , (int) vec.Y);

                bar.Add(1);
            }

            if ( Program.Game.CurrentDimension != 1 ) StructuresList.Add(new Home(0 , MapHeight[5] , true));
        }

        void GeneratorStructurePosition(ProgressBar bar)
        {
            for ( var i = 0 ; i < SizeGeneratior.WorldWidth ; i++ )
            {
                GeneratorHomeAndFactory(bar,i);

                GeneratorTree(i);
                GeneratorCaves(i);

                bar.Add(1);
            }
        }

        void GeneratorHomeAndFactory(ProgressBar bar , int i)
        {
            if ( i + 9 >= SizeGeneratior.WorldWidth || MapHeight[i] != MapHeight[i + 9]) return;

            if ( Rand.Next(0 , 20) == 0 && ReferenceEquals(MapBiomes[i] , ArrayResource.Grass) ||
                 Rand.Next(0 , 60) == 0 && ReferenceEquals(MapBiomes[i] , ArrayResource.Desrt)
                    )
            {
                if ( StructuresList.Count >= 1 )
                {
                    var test = false;
                    for ( var structureId = StructuresList.Count - 1 ; structureId > 0 ; structureId-- )
                    {
                        if ( !(StructuresList[structureId] is Home home)  ) continue;

                        test = true;
                        if ( i > home.X + 15 )
                            StructuresList.Add(new Home(i , MapHeight[i]));
                        break;
                    }

                    if ( !test ) StructuresList.Add(new Home(i , MapHeight[i]));
                }
                else if ( i > 15 ) StructuresList.Add(new Home(i , MapHeight[i]));
            }
            else if ( Program.Game.CurrentDimension == 0 && Rand.NextDouble() <= 0.5f )
            {
                if ( _factory == null && i >= 40 && i < SizeGeneratior.WorldWidth - 40 )
                    _factory = new Factory(i , MapHeight[i]);
            }
        }

        void GeneratorCaves(int i)
        {
            for ( var j = SizeGeneratior.RockLayer ; j < SizeGeneratior.WorldHeight ; j++ )
            {
                var airspawn = Rand.Next(0 , 800);

                if ( airspawn == 0 && Program.Game.CurrentDimension != 1 )
                {
                    StructuresList.Add(new Caves(i , j));
                }
            }
        }

        void GeneratorTree(int i)
        {
            var tree = Rand.Next(0 , 3);
            if ( tree == 1 &&
                 (ReferenceEquals(MapBiomes[i] , ArrayResource.Grass) ||
                  !ReferenceEquals(MapBiomes[i] , ArrayResource.Snow)) )
                _vectree.Add(new Vector2(i , MapHeight[i] - 1));
            if ( Rand.Next(0 , 10) == 0 && ReferenceEquals(MapBiomes[i] , ArrayResource.Desrt) )
                _vectree.Add(new Vector2(i , MapHeight[i] - 1));
        }
    }
}
