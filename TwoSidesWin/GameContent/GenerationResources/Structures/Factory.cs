using System;

using Microsoft.Xna.Framework;

using TwoSides.GameContent.Entity.NPC;
using TwoSides.Utils;
using TwoSides.World.Generation;
using TwoSides.World.Generation.Structures;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.GenerationResources.Structures
{
    public sealed class Factory : BaseStruct
    {
        [NonSerialized]
        const int SIZEX = 25;

        [NonSerialized]
        const int SIZEY = 25;

        public Factory(int x, int y) : base(x,y + SIZEY + 5)
        {
        }

        public override void Spawn(BaseDimension dimension)
        {
            for (int i = 0; i < SIZEX; i++)
            {
                if (X + i + 1 >= SizeGeneratior.WorldWidth) continue;
                for (int j = 0; j < SIZEY; j++)
                {

                    if (j != 0 || Math.Abs(Y - j - dimension.MapHeight[X + i + 1]) < 1)
                        dimension.Reset(X + i, Y - j);
                }
            }
            for (int i = -5; i < 5; i++)
            {
                for (int j = -5; j < 5; j++)
                {
                    if (Tools.InRadious(X + 5 + i, Y + 5 + j, X + 5, Y + 5, 5 - 1, true))
                    {
                        dimension.SetTexture(X + 5 + i, Y - 5 + j, 12);
                    }
                }
            }
            dimension.MapTile[X + SIZEX / 2, Y - SIZEY / 2].IdPoster = 0;
            for (int i = 0; i < SIZEX - 1; i++)
            {
                for (int j = 1; j < SIZEY - 1; j++)
                {
                    dimension.MapTile[X + i, Y - j].IdWall = 9;
                }
            }
            for (int i = 0; i < SIZEX; i++)
            {
                dimension.SetTexture(X + i, Y, 9);
               dimension.SetTexture(X + i, Y - (SIZEY - 1), 9);
            }
            for (int j = 4; j < SIZEY; j++)
            {
                dimension.SetTexture(X, Y - j, 9);
                dimension.SetTexture(X + (SIZEX - 1), Y - j, 9);
            }
            if (!Isplaying)
            {
                dimension.Zombies.Add(new Boss(new Vector2((X + 2) * Tile.TileMaxSize, (Y - 10) * Tile.TileMaxSize)));
            }
            dimension.SetTexture(X + 1, Y - (SIZEY - 2), 6);
            dimension.SetTexture(X + (SIZEX - 2), Y - (SIZEY - 2), 6);
        }
    }
}
