using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World;
using Microsoft.Xna.Framework.Graphics;
using TwoSides;
using Microsoft.Xna.Framework.Content;
using TwoSides.GameContent.GenerationResources;
using Microsoft.Xna.Framework;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class TileTwoSides
    {
        public const int TileMax = 33;
        public const int addmaxt = 13;
        public const int maxPosters = 1;

        public Texture2D[] textures = new Texture2D[TileMax];
        public Texture2D[] posters = new Texture2D[TileMax];
        public Texture2D[] addtexture = new Texture2D[addmaxt];

        public BaseTile[] tileList = new BaseTile[]{
            new GrassBlock(1,0,0), //0
            new StoneBlock(5,1), //1 
            new StoneBlock(6,2), //2
            new StoneBlock(8,3), //3
            new GrassBlock(1,4,4), //4
            new BaseTile(3,5), //5 
            new LightBlock(3,6), //6
            new StoneBlock(4,7), //7
            new BlockChest(3,9), //8
            new StoneBlock(9,15), //9
            new GrassBlock(1,9,16), //10
            new BaseTile(-1,17), //11
            new BlockPortal(-1,-18), //12
            new BlockTree(1,5), //13 
            new BlockTree(1,5), //14
            new BlockTree(1,5), //15
            new BlockTree(1,5), //16
            new BlockSit(17,1), //17
            new BlockSit(18,1), //18
            new BlockTable(1,19), //19
            new BlockTable(1,20), //20
            new BaseTile(-1,-1), //21
            //new Update
            new StoneBlock(1,28), //22
            new BaseTile(1,36), //23
            new BaseTile(1,37), //24
            new StoneBlock(1,40), //25
            new StoneBlock(1,39), //26
            new BlockNull(27),//27;
            new BlockTree(1,46),//28;
            new BlockNull(29),//29;
            new CloseDoor(1,5),//30;
            new OpenDoor(1,5),//31;
            new BlockNull(32),//32;
        };

        public void Loadtiles(ContentManager Content)
        {
            Program.StartSw();
            for (int i = 0; i < TileMax; i++)
            {
                textures[i] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\" + i);
            }
            addtexture[0] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\0_1");
            addtexture[1] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\0_2");
            addtexture[2] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\0_3");
            addtexture[3] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\0_4");

            addtexture[4] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\4_1");
            addtexture[5] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\4_2");
            addtexture[6] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\4_3");
            addtexture[7] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\4_4");

            addtexture[8] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\10_1");
            addtexture[9] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\10_2");
            addtexture[10] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\10_3");
            addtexture[11] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\10_4");
            addtexture[12] = Content.Load<Texture2D>(Game1.ImageFolder + "tiles\\11_1");
            Program.StopSw("load tiles");
            Program.StartSw();
            for (int i = 0; i < maxPosters; i++)
            {
                posters[i] = Content.Load<Texture2D>(Game1.ImageFolder + "posters\\" + i);
            }
            Program.StopSw("Loaded Posters");
            ITile.setTileList(this.tileList,TileMax);
        }

        public void renderTiles(int i0, int i1, int j0, int j1,SpriteBatch spriteBatch)
        {
            for (int i = i0; i < i1; i++)
            {

                if (i < SizeGeneratior.WorldWidth && i >= 0) Program.game.dimension[Program.game.currentD].maxy(i);
                for (int j = j0; j < j1; j++)
                {
                    if (i > SizeGeneratior.WorldWidth - 1 || i < 0) continue;
                    if (j > SizeGeneratior.WorldHeight - 1 || j < 0) continue;
                    //dimension[currentD].globallighting = 1;
                    Color color = Color.White;
                    //рисование
                    ITile tile = Program.game.dimension[Program.game.currentD].map[i, j];
                    Vector2 postile = new Vector2(ITile.TileMaxSize * i, ITile.TileMaxSize * j);
                    if (Program.game.dimension[Program.game.currentD].map[i, j].active)
                    {
                        tile.Render(i, j, spriteBatch, Program.game.dimension[Program.game.currentD].mapHeight[i] == j, textures, addtexture, postile, color);

                    }
                    else
                    {
                        tile.Render(i,j,postile, spriteBatch, textures[1]);
                    }

                }
            }
        }

        public void renderPlasters(int i0, int i1, int j0, int j1, SpriteBatch spriteBatch)
        {
            for (int i = i0; i < i1; i++)
            {

                if (i < SizeGeneratior.WorldWidth && i >= 0) Program.game.dimension[Program.game.currentD].maxy(i);
                for (int j = j0; j < j1; j++)
                {
                    if (i > SizeGeneratior.WorldWidth - 1 || i < 0) continue;
                    if (j > SizeGeneratior.WorldHeight - 1 || j < 0) continue;
                    //dimension[currentD].globallighting = 1;
                    Color color = Color.White;
                    //рисование
                    if (Program.game.dimension[Program.game.currentD].map[i, j].posterid >= 0)
                        spriteBatch.Draw(posters[Program.game.dimension[Program.game.currentD].map[i, j].posterid], new Vector2(ITile.TileMaxSize * i, ITile.TileMaxSize * j), Color.Gray);
                }
            }
        }

        public void renderWall(int i0, int i1, int j0, int j1, SpriteBatch spriteBatch)
        {
            for (int i = i0; i < i1; i++)
            {

                if (i < SizeGeneratior.WorldWidth && i >= 0) Program.game.dimension[Program.game.currentD].maxy(i);
                for (int j = j0; j < j1; j++)
                {
                    if (i > SizeGeneratior.WorldWidth - 1 || i < 0) continue;
                    if (j > SizeGeneratior.WorldHeight - 1 || j < 0) continue;
                    //dimension[currentD].globallighting = 1;
                    Color color = Color.White;
                    //рисование
                    if (Program.game.dimension[Program.game.currentD].map[i, j].wallid >= 0)
                    {
                        spriteBatch.Draw(textures[Program.game.dimension[Program.game.currentD].map[i, j].wallid], new Vector2(ITile.TileMaxSize * i, ITile.TileMaxSize * j), Color.Gray);

                        int lighting = 255 - Program.game.dimension[Program.game.currentD].map[i, j].light * 5;
                        spriteBatch.Draw(Program.game.black, new Vector2(ITile.TileMaxSize * i, ITile.TileMaxSize * j), new Color(0, 0, 0, lighting));
                
                    }
                }
            }
        }
    }
}
