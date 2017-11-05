using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TwoSides.GameContent.Entity.NPC;
using TwoSides.GUI;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Generation.Structures;

namespace TwoSides.GameContent.GenerationResources.Structures
{
    internal class Home : BaseStruct
    {

        public Home(int x, int y)
            : base(x, y)
        {
        }
        public Home(int x, int y, bool isplayer)
            : base(x, y) => Isplaying = isplayer;

        void SpawnInForest(BaseDimension dimension)
        {
            Clear(dimension);
            short tile = PlaceBackground(dimension);
            PlaceWall(dimension , tile);
            GeneratorPersonOrZombie(dimension);
            PlaceFurniture(dimension);
            dimension.MapTile[X + 3, Y - 3].IdWall = 21;
            dimension.MapTile[X + 4, Y - 3].IdWall = 21;
            dimension.MapTile[X + 3, Y - 4].IdWall = 21;
            dimension.MapTile[X + 4, Y - 4].IdWall = 21;
            dimension.AddDoor(30, X + 9, Y - 1, !Isplaying);
        }

        void PlaceFurniture(BaseDimension dimension)
        {
            dimension.SetTexture(X + 6 , Y - 1 , 8);
            dimension.SetTexture(X + 8 , Y - 4 , 6);
            dimension.SetTexture(X + 1 , Y - 4 , 6);
            dimension.SetTexture(X + 1 , Y - 1 , 17);
            dimension.SetTexture(X + 1 , Y - 2 , 17 , 1);
            dimension.SetTexture(X + 3 , Y - 1 , 20);
        }

        void GeneratorPersonOrZombie(BaseDimension dimension)
        {
            if ( !Isplaying )
            {
                int a = dimension.Rand.Next(1 , 4);
                for ( int i = 0 ; i < a ; i++ )
                {
                    Clothes[] cl = new Clothes[6];
                    Color[] color = new Color[6];
                    for ( int c = 0 ; c < 6 ; c++ )
                    {
                        int[] maxInt =
                        {
                            Clothes.MaxHair , Clothes.MaxShirt , Clothes.MaxPants , Clothes.MaxShoes , Clothes.MaxBelt ,
                            Clothes.MaxGlove 
                        };
                        int b = Program.Game.Rand.Next(-1 , maxInt[c]);
                        if ( b == -1 ) cl[c] = new Clothes();
                        else
                        {
                            cl[c] = new Clothes(b);
                            color[c] = new Color(Program.Game.Rand.Next(0 , 256) , Program.Game.Rand.Next(0 , 256) ,
                                                 Program.Game.Rand.Next(0 , 256));
                        }
                    }

                    dimension.Zombies.Add(
                                          new Zombie(X + 4 + i * 5 ,
                                                     Race.Racelist[Program.Game.Rand.Next(Race.Racelist.Count)] , cl , color));
                }
            }
            else if ( Program.Game.CurrentDimension == 0 )
            {
                InitDialog();
            }
        }

        void PlaceWall(BaseDimension dimension , short tile)
        {
            for ( int i = 0 ; i < 10 ; i++ )
            {
                dimension.SetTexture(X + i , Y , tile);
                dimension.SetTexture(X + i , Y - 5 , tile);
            }
            for ( int i = 4 ; i < 6 ; i++ )
            {
                if ( !Isplaying ) dimension.SetTexture(X , Y - i , tile);
                else dimension.SetTexture(X + 9 , Y - i , tile);
            }
            for ( int i = 0 ; i < 6 ; i++ )
            {
                if ( !Isplaying ) dimension.SetTexture(X + 9 , Y - i , tile);
                else dimension.SetTexture(X , Y - i , tile);
            }
        }

        short PlaceBackground(BaseDimension dimension)
        {
            const short TILE = 26;
            for ( int j = 1 ; j < 5 ; j++ )
            {
                if ( Isplaying )
                {
                    for ( int i = 1 ; i < 10 ; i++ )
                    {
                        dimension.SetWallId(X + i , Y - j , TILE);
                    }
                }
                else
                {
                    for ( int i = 0 ; i < 9 ; i++ )
                    {
                        dimension.SetWallId(X + i , Y - j , TILE);
                    }
                }
            }

            return TILE;
        }

        void Clear(BaseDimension dimension)
        {
            for ( int i = 0 ; i < 10 ; i++ )
            {
                for ( int j = 0 ; j < 10 ; j++ )
                {
                    if ( X + i + 1 >= SizeGeneratior.WorldWidth ) continue;

                    if ( j != 0 || Math.Abs(Y - j - dimension.MapHeight[X + i + 1]) < 1 )
                        dimension.Reset(X + i , Y - j);
                }
            }
        }

        static void InitDialog()
        {
            List<Recipe> recip = new List<Recipe> {new Recipe(new Item(1 , 0) , 100)};
            recip[0].AddIngridents(1 , 1);

            // ReSharper disable once UnusedVariable
            List<Dialog> dialogs = new List<Dialog>
                                   {
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) ,
                                                  "Get out, i can handle that!" ,
                                                  "Okay, stupId bot." ,
                                                  "Do not want to distract you!" ,
                                                  "Of course" ,
                                                  "I'm your tour guIde, you are ready to explore the science of survival, here!" ,
                                                  "monster,I'm your tour guIde, you are ready to explore the science of survival, here!" ,
                                                  "Hero,I'm your tour guIde, you are ready to explore the science of survival, here!" ,
                                                  Program.Game.Button , Program.Game.Font1 ,
                                                  Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) , //первое задание
                                                  "Why the hell?" ,
                                                  "Ready, bitch, i am killed them" ,
                                                  "How?" ,
                                                  "killed them, they accIdents!" ,
                                                  "The first task, kill 1 zombie! To do this you need to take any weapon and aim at his head and hit the(LKM)" ,
                                                  "monster,I'm your tour guIde, you are ready to explore the science of survival, here!" ,
                                                  "Hero,I'm your tour guIde, you are ready to explore the science of survival, here!" ,
                                                  Program.Game.Button , Program.Game.Font1 ,
                                                  Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) ,
                                                  "Get out, i am not a slave!" ,
                                                  "Okay, stupId bot." ,
                                                  "I'm busy Sori.!" ,
                                                  "Of course" ,
                                                  "Good job, are you ready to take the next job. you need to extract resources!" ,
                                                  "monster,are you ready to take the next job. you need to extract resources!" ,
                                                  "Hero,are you ready to take the next job. you need to extract resources!" ,
                                                  Program.Game.Button , Program.Game.Font1 ,
                                                  Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) , // второе задание
                                                  "I dId not try!" ,
                                                  "Ready, bitch, I got resources" ,
                                                  "Not yet!" ,
                                                  "I got resources!" ,
                                                  "I got the right resources?Burger and Water Bottle in chest, Destory Him" ,
                                                  "monster,I got the right resources? Burger and Water Bottle in chest, Destory Him" ,
                                                  "Hero,I got the right resources?Burger and Water Bottle in chest, Destory Him" ,
                                                  Program.Game.Button , Program.Game.Font1 ,
                                                  Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) ,
                                                  "Get out, I'm not your slave" ,
                                                  "Okay, bitch" ,
                                                  "I cannot begin to do it" ,
                                                  "Okay" ,
                                                  "you items, sooner or later break down, so learn crafting new(to crafting items, click J)" ,
                                                  "monster,you items, sooner or later break down, so learn crafting new(to crafting items, click J)" ,
                                                  "Hero,you items, sooner or later break down, so learn crafting new(to crafting items, click J)!" ,
                                                  Program.Game.Button , Program.Game.Font1 ,
                                                  Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) , // 3 задание
                                                  "Get out, I'm not your slave" ,
                                                  "Ready, bitch, i am crafting pickaxe, which I will kill you" ,
                                                  "Not yet" ,
                                                  "easy it was" ,
                                                  "You Crafting PickAxe(to crafting items, click J)?" ,
                                                  "monster,You Crafting PickAxe(to crafting items, click J)?" ,
                                                  "Hero,You Crafting PickAxe(to crafting items, click J)?" ,
                                                  Program.Game.Button , Program.Game.Font1 ,
                                                  Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) ,
                                                  "Get out, I'm not your slave" ,
                                                  "Okay, bitch" ,
                                                  "I cannot begin to do it" ,
                                                  "Okay" ,
                                                  "To defeat the boss, you need to crafting armor" ,
                                                  "monster,To defeat the boss, you need to crafting armor" ,
                                                  "Hero,To defeat the boss, you need to crafting armor" ,
                                                  Program.Game.Button , Program.Game.Font1 ,
                                                  Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) , // 4 задание
                                                  "Get out, I'm not your slave" ,
                                                  "Ready, bitch," ,
                                                  "Not yet" ,
                                                  "easy it was" ,
                                                  "Crafting Armor?" ,
                                                  "monster,Crafting Armor?" ,
                                                  "Hero,Crafting Armor?" , Program.Game.Button ,
                                                  Program.Game.Font1 , Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) , // 5 заданий
                                                  "Get out, I'm not your slave" ,
                                                  "Okay, bitch" ,
                                                  "I cannot begin to do it" ,
                                                  "Okay" ,
                                                  "Search iron factory and Kill Ultra Zombie and send the show world" ,
                                                  "monster,Search iron factory and Kill Ultra Zombie and send the show world!" ,
                                                  "Hero,Search iron factory and Kill Ultra Zombie and send the show world!" ,
                                                  Program.Game.Button , Program.Game.Font1 ,
                                                  Program.Game.Dialogtex) ,
                                       new Dialog(new Rectangle(2 , 200 , 800 , 200) ,
                                                  "" ,
                                                  "clear, he и lazy ass" ,
                                                  "" ,
                                                  "" ,
                                                  "Create our world, no create task, он he is lazy" ,
                                                  "monster,Create our world, no create task, он he is lazy" ,
                                                  "Hero,Create our world, no create task, он he is lazy!" ,
                                                  Program.Game.Button , Program.Game.Font1 , Program.Game.Dialogtex)
                                   };
        }

        void SpawnPyr(BaseDimension dimension)
        {
            int wallStone = dimension.Rand.Next(10,15);
            int size = wallStone*2+dimension.Rand.Next(wallStone / 3, wallStone / 2);
            
            Y += wallStone;
            if (X + size >= SizeGeneratior.WorldWidth) return;
            if (Y - wallStone >= SizeGeneratior.WorldHeight) return;
            for (int i = 0; i < wallStone; i++)
                dimension.SetTexture(X + i, Y - i, 25);
            for (int i = 0; i < wallStone; i++)
                dimension.SetTexture(X + (size - wallStone) + i, Y - (wallStone - 1) + i, 25);
            for (int i = 0; i < size - wallStone * 2; i++)
                dimension.SetTexture(X + i + wallStone, Y - (wallStone - 1), 25);
            for (int i = 0; i < size; i++)
                dimension.SetTexture(X + i, Y, 25);
            for (int i = 1; i < wallStone - 1; i++)
            {
                for (int j = i+1; j < size - i-1; j++)
                {
                    dimension.SetWallId(X + j, Y - i, 25);
                }
            }
            int direction = 0;
            int stepsToDirection = dimension.Rand.Next(5, 10);
            int stepX = X + (size-4);
            int stepY = Y-1;
            int countPath = dimension.Rand.Next(5, 7);
            for (int i = 0; i < countPath; i++)
            {
                _countAddPath = 1;
                CreatePath(dimension, direction, stepsToDirection, ref stepX, ref stepY);
                direction = direction == 0 ? dimension.Rand.Next(0, 2) : dimension.Rand.Next((direction+1)%2, 3);
                if (stepX - 5 <= 0 || stepX + 5 >= SizeGeneratior.WorldWidth) break;
                if (stepY - 5 <= 0 || stepY + 5 >= SizeGeneratior.WorldHeight) break;
                stepsToDirection = dimension.Rand.Next(5, 10); 
            }
        }

        void CreatePath(BaseDimension dimension, int direction, int stepsToDirection, ref int stepX, ref int stepY)
        {
            if ( GeneratorHoles(dimension , direction , stepsToDirection , ref stepX , ref stepY) ) return;

            if ( dimension.Rand.Next(100) > 30 - 10 * _countAddPath ) return;

            int countPath = dimension.Rand.Next(5, 7);
            int stepXNew = stepX;
            int stepYNew = stepY;
            _countAddPath = 2;
            for (int i = 0; i < countPath; i++)
            {
                switch ( direction )
                {

                    case 0:
                        direction = dimension.Rand.Next(0, 2);
                        break;
                    case 1:
                        direction = dimension.Rand.Next(0, 3);
                        break;
                    case 2:
                        direction = dimension.Rand.Next(1, 3);
                        break;
                }
                stepsToDirection = dimension.Rand.Next(5, 10);
                CreatePath(dimension, direction, stepsToDirection, ref stepXNew, ref stepYNew);
                if (stepXNew - 5 <= 0 || stepXNew + 5 >= SizeGeneratior.WorldWidth) break; 
                if (stepYNew - 5 <= 0 || stepYNew + 5 >= SizeGeneratior.WorldHeight) break;
            }
        }

        static bool GeneratorHoles(BaseDimension dimension , int direction , int stepsToDirection , ref int stepX ,
                                   ref int stepY)
        {
            for ( int j = 0 ; j < stepsToDirection ; j++ )
            {
                if ( stepX - 5 <= 0 || stepX + 5 >= SizeGeneratior.WorldWidth ) return true;
                if ( stepY - 5 <= 0 || stepY + 5 >= SizeGeneratior.WorldHeight ) return true;

                switch ( direction )
                {
                    case 0:
                        if ( dimension.MapTile[stepX , stepY - 3].IdWall != 25 ) dimension.SetTexture(stepX , stepY - 3 , 25);
                        if ( dimension.MapTile[stepX , stepY + 1].IdWall != 25 ) dimension.SetTexture(stepX , stepY + 1 , 25);
                        stepX = stepX + 1;
                        break;
                    case 1:
                        if ( dimension.MapTile[stepX - 3 , stepY].IdWall != 25 ) dimension.SetTexture(stepX - 3 , stepY , 25);
                        if ( dimension.MapTile[stepX + 1 , stepY].IdWall != 25 ) dimension.SetTexture(stepX + 1 , stepY , 25);
                        stepY = stepY + 1;
                        break;
                    case 2:
                        if ( dimension.MapTile[stepX , stepY - 3].IdWall != 25 ) dimension.SetTexture(stepX , stepY - 3 , 25);
                        if ( dimension.MapTile[stepX , stepY - 1].IdWall != 25 ) dimension.SetTexture(stepX , stepY + 1 , 25);
                        stepX = stepX - 1;
                        break;
                }

                dimension.SetWallId(stepX - 2 , stepY , 25);
                dimension.SetWallId(stepX - 1 , stepY , 25);
                dimension.SetWallId(stepX , stepY , 25);
            }

            return false;
        }

        int _countAddPath;
        public override void Spawn(BaseDimension dimension)
        {
            if (ReferenceEquals(dimension.MapBiomes[X] , ArrayResource.Grass)) SpawnInForest(dimension);
            else if (ReferenceEquals(dimension.MapBiomes[X] , ArrayResource.Desrt)) SpawnPyr(dimension);
        }
    }
}
