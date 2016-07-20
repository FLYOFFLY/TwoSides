using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using System.Collections;
using TwoSides.Physics.Entity.NPC;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.World.Tile;
using TwoSides.World.Generation;
namespace TwoSides.World.Structures
{
    class Home : BaseStruct
    {

        public Home(int x, int y)
            : base(x, y)
        {
        }
        public Home(int x, int y, bool isplayer)
            : base(x, y)
        {
            isplaying = isplayer;
        }
        private void spawnInForest(BaseDimension dimension)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (x + i + 1 < SizeGeneratior.WorldWidth)
                    {
                        if (j != 0 || (Math.Abs((y - j) - dimension.mapHeight[x + i + 1]) < 1))
                            dimension.Reset(x + i, y - j);
                    }
                }
            }
            short tile = 26;
            for (int j = 1; j < 5; j++)
            {
                if (isplaying)
                {
                    for (int i = 1; i < 10; i++)
                    {
                        if (x + i + 1 < SizeGeneratior.WorldWidth)
                        {
                            dimension.map[x + i, y - j].wallid = tile;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (x + i + 1 < SizeGeneratior.WorldWidth)
                        {
                            dimension.map[x + i, y - j].wallid = tile;
                        }
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                dimension.settexture(x + i, y, tile);
            }
            for (int i = 0; i < 10; i++)
            {
                dimension.settexture(x + i, y - 5, tile);
            }
            for (int i = 4; i < 6; i++)
            {
                if (!isplaying) dimension.settexture(x, y - i, tile);
                else dimension.settexture(x + 9, y - i, tile);
            }
            for (int i = 0; i < 6; i++)
            {
                if (!isplaying) dimension.settexture(x + 9, y - i, tile);
                else dimension.settexture(x, y - i, tile);
            } 
            if (!isplaying)
            {
                int a = dimension.rand.Next(1, 4);
                for (int i = 0; i < a; i++)
                {
                    Clothes[] cl = new Clothes[6];
                    for (int c = 0; c < 6; c++)
                    {
                        cl[c] = new Clothes();
                    }
                    for (int c = 0; c < 6; c++)
                    {
                        int b = 0;
                        if (c == 0) b = Program.game.rand.Next(-1, Clothes.maxHair);
                        if (c == 1) b = Program.game.rand.Next(-1, Clothes.maxShirt);
                        if (c == 2) b = Program.game.rand.Next(-1, Clothes.maxPants);
                        if (c == 3) b = Program.game.rand.Next(-1, Clothes.maxShoes);
                        if (c == 4) b = Program.game.rand.Next(-1, Clothes.maxBelt);
                        if (c == 5) b = Program.game.rand.Next(-1, Clothes.maxGlove);
                        if (b == -1) cl[c] = new Clothes();
                        else cl[c] = new Clothes(b);
                    }
                    Color[] color = new Color[5];
                    for (int c = 0; c < 5; c++)
                    {
                        color[c] = new Color(Program.game.rand.Next(0, 256), Program.game.rand.Next(0, 256), Program.game.rand.Next(0, 256));
                    }
                    dimension.Zombies.Add(
                        new Zombie(x + 4 + i * 5, (Race)Race.racelist[Program.game.rand.Next(Race.racelist.Count)], cl, color));
                }
            }
            else if (Program.game.currentD == 0)
            {
                ArrayList dialogs = new ArrayList();
                ArrayList recip = new ArrayList();
                recip.Add(new Recipe(new Item(1, 0), 100));
                ((Recipe)(recip[0])).addigr(1, 1);
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200), "Get out, i can handle that!",
                      "Okay, stupid bot.",
                         "Do not want to distract you!",
                      "Of course",
                        "I'm your tour guide, you are ready to explore the science of survival, here!",
                        "monster,I'm your tour guide, you are ready to explore the science of survival, here!",
                        "Hero,I'm your tour guide, you are ready to explore the science of survival, here!", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200), //первое задание
                        "Why the hell?",
                        "Ready, bitch, i am killed them",
                        "How?",
                        "killed them, they accidents!",
                        "The first task, kill 1 zombie! To do this you need to take any weapon and aim at his head and hit the(LKM)",
                        "monster,I'm your tour guide, you are ready to explore the science of survival, here!",
                        "Hero,I'm your tour guide, you are ready to explore the science of survival, here!", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200), "Get out, i am not a slave!",
                   "Okay, stupid bot.",
                      "I'm busy Sori.!",
                   "Of course",
                     "Good job, are you ready to take the next job. you need to extract resources!",
                        "monster,are you ready to take the next job. you need to extract resources!",
                        "Hero,are you ready to take the next job. you need to extract resources!", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200),// второе задание
                        "I did not try!",
                        "Ready, bitch, I got resources",
                        "Not yet!",
                        "I got resources!",
                        "I got the right resources?Burger and Water Bottle in chest, Destory Him",
                        "monster,I got the right resources? Burger and Water Bottle in chest, Destory Him",
                        "Hero,I got the right resources?Burger and Water Bottle in chest, Destory Him", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200),
                      "Get out, I'm not your slave",
                      "Okay, bitch",
                      "I cannot begin to do it",
                      "Okay",
                      "you items, sooner or later break down, so learn crafting new(to crafting items, click J)",
                        "monster,you items, sooner or later break down, so learn crafting new(to crafting items, click J)",
                        "Hero,you items, sooner or later break down, so learn crafting new(to crafting items, click J)!", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200), // 3 задание
                      "Get out, I'm not your slave",
                      "Ready, bitch, i am crafting pickaxe, which I will kill you",
                      "Not yet",
                      "easy it was",
                      "You Crafting PickAxe(to crafting items, click J)?",
                        "monster,You Crafting PickAxe(to crafting items, click J)?",
                        "Hero,You Crafting PickAxe(to crafting items, click J)?", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200),
                      "Get out, I'm not your slave",
                      "Okay, bitch",
                      "I cannot begin to do it",
                      "Okay",
                      "To defeat the boss, you need to crafting armor",
                        "monster,To defeat the boss, you need to crafting armor",
                        "Hero,To defeat the boss, you need to crafting armor", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200), // 4 задание
                      "Get out, I'm not your slave",
                      "Ready, bitch,",
                      "Not yet",
                      "easy it was",
                      "Crafting Armor?",
                        "monster,Crafting Armor?",
                        "Hero,Crafting Armor?", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200), // 5 заданий
                     "Get out, I'm not your slave",
                     "Okay, bitch",
                     "I cannot begin to do it",
                     "Okay",
                     "Search iron factory and Kill Ultra Zombie and send the show world",
                        "monster,Search iron factory and Kill Ultra Zombie and send the show world!",
                        "Hero,Search iron factory and Kill Ultra Zombie and send the show world!", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                dialogs.Add(new GUI.Dialog(new Rectangle(2, 200, 800, 200),
                      "",
                      "clear, he и lazy ass",
                      "",
                      "",
                      "Create our world, no create task, он he is lazy",
                     "monster,Create our world, no create task, он he is lazy",
                     "Hero,Create our world, no create task, он he is lazy!", Program.game.button, Program.game.Font1, Program.game.dialogtex));
                //dimension.civil.Add(new Civilian(
                 // new Vector2((x + 4) * ITile.TileMaxSize, (y - 3) * ITile.TileMaxSize),
                  //  dialogs));
            }
            dimension.settexture(x + 6, y - 1, 8);
            dimension.settexture(x + 8, y - 4, 6);
            dimension.settexture(x + 1, y - 4, 6);
            dimension.settexture(x + 1, y - 1, 17);
            dimension.settexture(x + 1, y - 2, 18);
            dimension.settexture(x + 4, y - 1, 19);
            dimension.settexture(x + 3, y - 1, 20);
            dimension.map[x + 3, y - 3].wallid = 21;
            dimension.map[x + 4, y - 3].wallid = 21;
            dimension.map[x + 3, y - 4].wallid = 21;
            dimension.map[x + 4, y - 4].wallid = 21;
            dimension.addDoor(30, x + 9, y - 1, !isplaying);
        }
        private void spawnPyr(BaseDimension dimension)
        {
            int wallStone = dimension.rand.Next(10,15);
            int size = wallStone*2+dimension.rand.Next(wallStone / 3, wallStone / 2);
            
            y += wallStone;
            if (x + size >= SizeGeneratior.WorldWidth) return;
            if (y - wallStone >= SizeGeneratior.WorldHeight) return;
            for (int i = 0; i < wallStone; i++)
                dimension.settexture(x + i, y - i, 25);
            for (int i = 0; i < wallStone; i++)
                dimension.settexture(x + (size - wallStone) + i, y - (wallStone - 1) + i, 25);
            for (int i = 0; i < (size - wallStone * 2); i++)
                dimension.settexture(x + i + wallStone, y - (wallStone - 1), 25);
            for (int i = 0; i < size; i++)
                dimension.settexture(x + i, y, 25);
            for (int i = 1; i < (wallStone - 1); i++)
            {
                for (int j = i+1; j < size - i-1; j++)
                {
                    dimension.SetWallID(x + j, y - i, 25);
                }
            }
            int direction = 0;
            int stepsToDirection = dimension.rand.Next(5, 10);
            int stepX = x + (size-4);
            int stepY = y-1;
            int countPath = dimension.rand.Next(5, 7);
            for (int i = 0; i < countPath; i++)
            {
                countAddPath = 1;
                createPath(dimension, direction, stepsToDirection, ref stepX, ref stepY);
                if (direction == 0)
                    direction = dimension.rand.Next(0, 2);
                else if (direction == 1)
                    direction = dimension.rand.Next(0, 3);
                else if (direction == 2)
                    direction = dimension.rand.Next(1, 3);

                if (stepX - 5 <= 0 || stepX + 5 >= SizeGeneratior.WorldWidth) break; ;
                if (stepY - 5 <= 0 || stepY + 5 >= SizeGeneratior.WorldHeight) break;
                stepsToDirection = dimension.rand.Next(5, 10); 
            }
        }

        private void createPath(BaseDimension dimension, int direction, int stepsToDirection, ref int stepX, ref int stepY)
        {
            for (int j = 0; j < stepsToDirection; j++)
            {
                if (stepX - 5 <= 0 || stepX + 5 >= SizeGeneratior.WorldWidth) return;
                if (stepY - 5 <= 0 || stepY + 5 >= SizeGeneratior.WorldHeight) return;
                if (direction == 0)
                {
                    if (dimension.map[stepX, stepY - 3].wallid != 25) dimension.settexture(stepX, stepY - 3, 25);
                    if (dimension.map[stepX, stepY + 1].wallid != 25) dimension.settexture(stepX, stepY + 1, 25);
                    dimension.SetWallID(stepX, stepY - 2, 25);
                    dimension.SetWallID(stepX, stepY - 1, 25);
                    dimension.SetWallID(stepX, stepY, 25);
                    stepX = stepX + 1;
                }
                else if (direction == 1)
                {

                    dimension.SetWallID(stepX - 2, stepY, 25);
                    dimension.SetWallID(stepX - 1, stepY, 25);
                    dimension.SetWallID(stepX, stepY, 25);
                    if (dimension.map[stepX - 3, stepY].wallid != 25) dimension.settexture(stepX - 3, stepY, 25);
                    if (dimension.map[stepX + 1, stepY].wallid != 25) dimension.settexture(stepX + 1, stepY, 25);
                    stepY = stepY + 1;
                }
                else if (direction == 2)
                {
                    if (dimension.map[stepX, stepY - 3].wallid != 25) dimension.settexture(stepX, stepY - 3, 25);
                    if (dimension.map[stepX, stepY - 1].wallid != 25) dimension.settexture(stepX, stepY + 1, 25);
                    dimension.SetWallID(stepX, stepY - 2, 25);
                    dimension.SetWallID(stepX, stepY - 1, 25);
                    dimension.SetWallID(stepX, stepY, 25);
                    stepX = stepX - 1;
                }
            }
            if (dimension.rand.Next(100) <= 30 - 10 * countAddPath)
            {
                int countPath = dimension.rand.Next(5, 7);
                int stepXNew = stepX;
                int stepYNew = stepY;
                countAddPath = 2;
                for (int i = 0; i < countPath; i++)
                {
                    if (direction == 0)
                        direction = dimension.rand.Next(0, 2);
                    else if (direction == 1)
                        direction = dimension.rand.Next(0, 3);
                    else if (direction == 2)
                        direction = dimension.rand.Next(1, 3);

                    stepsToDirection = dimension.rand.Next(5, 10);
                    createPath(dimension, direction, stepsToDirection, ref stepXNew, ref stepYNew);
                    if (stepXNew - 5 <= 0 || stepXNew + 5 >= SizeGeneratior.WorldWidth) break; ;
                    if (stepYNew - 5 <= 0 || stepYNew + 5 >= SizeGeneratior.WorldHeight) break;
                }
            } 
        }
        int countAddPath = 0;
        public override void spawn(BaseDimension dimension)
        {
            if (dimension.mapB[x] != ArrayResource.grass && dimension.mapB[x] != ArrayResource.Desrt) return;
            if (dimension.mapB[x] == ArrayResource.grass) spawnInForest(dimension);
            else if (dimension.mapB[x] == ArrayResource.Desrt) spawnPyr(dimension);
        }
    }
}
