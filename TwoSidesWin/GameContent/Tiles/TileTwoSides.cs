using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class TileTwoSides : ITileList
    {
        public const int TILE_MAX = 39;
        public const int ADDMAXT = 13;
        public const int MAX_POSTERS = 1;

        public Texture2D[] Textures { get; } = new Texture2D[TILE_MAX];
        public Texture2D[] Posters { get; } = new Texture2D[TILE_MAX];
        public Texture2D[] Addtexture { get; } = new Texture2D[ADDMAXT];

        readonly BaseTile[] _tileList;
        public TileTwoSides() => _tileList = new[] {
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
                                                       //RUINS Update
                                                       new BlockPlatform(1,56),//33;
                                                       new BlockStairs(1,57),//34;
                                                       new StoneBlock(1,58),//35;
                                                       new StoneBlock(1,59),//36;
                                                       new StoneBlock(1,60),//37;
                                                       new BlockTree(1,5),//38;
                                                       //04.07.16
                                                       new BlockSit(39,1),//39
                                                       new BlockTable(40,1),//40
                                                       new BlockNull(41)
                                                   };

        /// <exception cref="OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
        public void Loadtiles(ContentManager content)
        {
            Program.StartSw();
            for (var i = 0; i < TILE_MAX; i++)
            {
                Textures[i] = content.Load<Texture2D>($"{Game1.IMAGE_FOLDER}tiles\\{i}");
            }
            Addtexture[0] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\0_1");
            Addtexture[1] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\0_2");
            Addtexture[2] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\0_3");
            Addtexture[3] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\0_4");

            Addtexture[4] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\4_1");
            Addtexture[5] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\4_2");
            Addtexture[6] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\4_3");
            Addtexture[7] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\4_4");

            Addtexture[8] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\10_1");
            Addtexture[9] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\10_2");
            Addtexture[10] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\10_3");
            Addtexture[11] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\10_4");
            Addtexture[12] = content.Load<Texture2D>(Game1.IMAGE_FOLDER + "tiles\\11_1");
            Program.StopSw("load tiles");
            Program.StartSw();
            for (var i = 0; i < MAX_POSTERS; i++)
            {
                Posters[i] = content.Load<Texture2D>($"{Game1.IMAGE_FOLDER}posters\\{i}");
            }
            Program.StopSw("Loaded Posters");
            Tile.SetTileList(_tileList);
        }

        public void RenderTiles(BaseDimension dimension, Rectangle rect, Render render)
        {
            if (rect.Y <= 0) rect.Y = 0;
            if (rect.X <= 0) rect.X = 0;
            for (var i = rect.X; i < rect.Width + rect.X; i++)
            {

                if (i < SizeGeneratior.WorldWidth) dimension.UpdateMaxY(i);
                if (i > SizeGeneratior.WorldWidth - 1) { break; }
                /*
                 
                    Parallel.For(rect.Y, rect.Height + rect.Y, (j) => {

                        if (j > SizeGeneratior.WorldHeight - 1 || j < 0) return;
                        //dimension[CurrentDimension].globallighting = 1;
                        Color color = Color.White;
                        //рисование
                        Tile tile = dimension.MapTile[i, j];
                        Vector2 postile = new Vector2(Tile.TileMaxSize * i, Tile.TileMaxSize * j);
                        if (dimension.MapTile[i, j].Active)
                        {
                            tile.Render(i, j, spriteBatch, dimension.MapHeight[i] == j, Textures, Addtexture, postile, color);

                        }
                        else
                        {
                            tile.Render(i, j, postile, spriteBatch, Textures[1]);
                        }
                    });
                 */
                
                for (var j = rect.Y; j < rect.Height + rect.Y; j++)
                {
                    if (j > SizeGeneratior.WorldHeight - 1) { break; }
                    //dimension[CurrentDimension].globallighting = 1;
                    //рисование
                    Tile tile = dimension.MapTile[i, j];
                    Vector2 postile = new Vector2(Tile.TILE_MAX_SIZE * i, Tile.TILE_MAX_SIZE * j);
                    if (tile.Light <= 0) continue;
                    if (dimension.MapTile[i, j].Active)
                    {
                        tile.Render(i, j, render, dimension.MapHeight[i] == j, Textures, Addtexture, postile, ColorScheme.BaseColor);

                    }
                    else
                    {
                        tile.Render(i,j,postile, render, Textures[1]);
                    }

                }
            }
        }

        public void RenderPlasters(BaseDimension dimension, Rectangle rect, Render render)
        {
            for (var i = rect.X; i < rect.Width+rect.X; i++)
            {

                if (i < SizeGeneratior.WorldWidth && i >= 0) dimension.UpdateMaxY(i);
                if (i > SizeGeneratior.WorldWidth - 1 || i < 0) continue;
                for (var j = rect.Y; j < rect.Y+rect.Height; j++)
                {
                    if (j > SizeGeneratior.WorldHeight - 1 || j < 0) continue;
                    //dimension[CurrentDimension].globallighting = 1;
                    //рисование
                    if (dimension.MapTile[i, j].Light <= 0) continue;
                    if (dimension.MapTile[i, j].IdPoster >= 0)
                        render.Draw(Posters[dimension.MapTile[i, j].IdPoster], new Vector2(Tile.TILE_MAX_SIZE * i, Tile.TILE_MAX_SIZE * j), ColorScheme.GenColorGradient(0.5f));
                }
            }
        }

        public void RenderWall(BaseDimension dimension, Rectangle rect, Render render)
        {
            for (var i = rect.X; i < rect.Width + rect.X; i++)
            {

                if (i < SizeGeneratior.WorldWidth && i >= 0) dimension.UpdateMaxY(i);
                if (i > SizeGeneratior.WorldWidth - 1 || i < 0) continue;
                for (var j = rect.Y; j < rect.Y + rect.Height; j++)
                {
                    if (j > SizeGeneratior.WorldHeight - 1 || j < 0) continue;

                    if ( dimension.MapTile[i , j].IdWall < 0 ) continue;

                    if (dimension.MapTile[i, j].Light <= 0) continue;
                    render.Draw(Textures[dimension.MapTile[i, j].IdWall], new Vector2(Tile.TILE_MAX_SIZE * i, Tile.TILE_MAX_SIZE * j), ColorScheme.GenColorGradient(0.5f));

                    var lighting = 255 - dimension.MapTile[i, j].Light * 5;
                    render.Draw(Program.Game.Black, new Vector2(Tile.TILE_MAX_SIZE * i, Tile.TILE_MAX_SIZE * j), ColorScheme.GenColorGradient(lighting/255.0f));
                }
            }
        }
    }
}
