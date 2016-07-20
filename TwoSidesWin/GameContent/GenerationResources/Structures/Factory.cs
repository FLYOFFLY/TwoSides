using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using TwoSides.Physics.Entity.NPC;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.World.Tile;
using TwoSides.Utils;
using TwoSides.World.Generation;
namespace TwoSides.World.Structures
{
    sealed public class Factory : BaseStruct
    {
        [NonSerialized]
        const int sizex = 25;

        [NonSerialized]
        const int sizey = 25;

        public Factory(int x, int y) : base(x,y + sizey + 5)
        {
        }

        public override void spawn(BaseDimension dimension)
        {
            for (int i = 0; i < sizex; i++)
            {
                for (int j = 0; j < sizey; j++)
                {
                    if (x + i + 1 < SizeGeneratior.WorldWidth)
                    {
                        if (j != 0 || (Math.Abs((y - j) - dimension.mapHeight[x + i + 1]) < 1))
                            dimension.Reset(x + i, y - j);
                    }
                }
            }
            for (int i = -5; i < 5; i++)
            {
                for (int j = -5; j < 5; j++)
                {
                    if (Util.inradious(x + 5 + i, y + 5 + j, x + 5, y + 5, 5 - 1, true))
                    {
                        dimension.settexture(x + 5 + i, y - 5 + j, 12);
                    }
                }
            }
            dimension.map[x + sizex / 2, y - sizey / 2].posterid = 0;
            for (int i = 0; i < sizex - 1; i++)
            {
                for (int j = 1; j < sizey - 1; j++)
                {
                    dimension.map[x + i, y - j].wallid = 9;
                }
            }
            for (int i = 0; i < sizex; i++)
            {
                dimension.settexture(x + i, y, 9);
            }
            for (int i = 0; i < sizex; i++)
            {
               //dimension.settexture(x + i, y - (sizey - 1), 9);
            }
            for (int j = 4; j < sizey; j++)
            {
                dimension.settexture(x, y - j, 9);
            }
            for (int j = 4; j < sizey; j++)
            {
                dimension.settexture(x + (sizex - 1), y - j, 9);
            }
            if (!isplaying)
            {
                dimension.Zombies.Add(new Boss(new Vector2((x + 2) * ITile.TileMaxSize, (y - 10) * ITile.TileMaxSize)));
            }
            dimension.settexture(x + 1, y - (sizey - 2), 6);
            dimension.settexture(x + (sizex - 2), y - (sizey - 2), 6);
        }
    }
}
