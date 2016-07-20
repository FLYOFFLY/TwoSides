using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World;
using TwoSides.World.Tile;
using TwoSides.World.Generation;
using TwoSides.GUI;

namespace TwoSides.GameContent.Dimensions
{
    [Serializable]
    public class Hell : BaseDimension
    {
        protected override void GenerationBiomes(ProgressBar bar)
        {
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                mapB[i] = ArrayResource.hell;
            }
        }
        protected override void GenerationTerrain(ProgressBar bar )
        {
            bar.Reset();
            bar.setText("Generation Terrain");
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                if (mapHeight[i] > 0)
                {
                    for (int j = 0; j < mapHeight[i]; j++)
                    {
                        settexture(i, j + (SizeGeneratior.WorldHeight - SizeGeneratior.WorldHeight / 2) + 400, 10);
                    }
                }
                else settexture(i, SizeGeneratior.WorldHeight - 5, 10);
                for (int j = mapHeight[i] + (SizeGeneratior.WorldHeight - SizeGeneratior.WorldHeight / 2) + 400; j < SizeGeneratior.WorldHeight; j++)
                {
                    settexture(i, j, 11);
                }
                maxy(i);
                bar.Add(1);
            }
        }

        protected override void GeneratorHeight(ProgressBar bar)
        {
            int maxSize = 1;
            bool blocks = true;
            for (int i = 0; i < SizeGeneratior.WorldWidth; i+=maxSize)
            {
                for (int x = i; x < i + maxSize;x++ ) {
                    if (x >= SizeGeneratior.WorldWidth) break;
                    mapHeight[x] = blocks ? 1:0;
                } 
                if (i >= SizeGeneratior.WorldWidth) break;
                blocks = !blocks;    
            }
        }

    }
}
