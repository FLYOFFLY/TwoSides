using System;

using TwoSides.GameContent.GenerationResources;
using TwoSides.GUI;
using TwoSides.World.Generation;

namespace TwoSides.GameContent.Dimensions
{
    [Serializable]
    public class Hell : BaseDimension
    {
        protected override void GenerationBiomes(ProgressBar progressBar)
        {
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                MapBiomes[i] = ArrayResource.Hell;
            }
        }
        protected override void GenerationTerrain(ProgressBar progressBar )
        {
            progressBar.Reset();
            progressBar.SetText("Generation Terrain");
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                if (MapHeight[i] > 0)
                {
                    for (int j = 0; j < MapHeight[i]; j++)
                    {
                        SetTexture(i, j + (SizeGeneratior.WorldHeight - SizeGeneratior.WorldHeight / 2) + 400, 10);
                    }
                }
                else SetTexture(i, SizeGeneratior.WorldHeight - 5, 10);
                for (int j = MapHeight[i] + (SizeGeneratior.WorldHeight - SizeGeneratior.WorldHeight / 2) + 400; j < SizeGeneratior.WorldHeight; j++)
                {
                    SetTexture(i, j, 11);
                }
                UpdateMaxY(i);
                progressBar.Add(1);
            }
        }

        protected override void GeneratorHeight(ProgressBar progressBar)
        {
            const int MAX_SIZE = 1;
            bool blocks = true;
            for (int i = 0; i < SizeGeneratior.WorldWidth; i+=MAX_SIZE)
            {
                for (int x = i; x < i + MAX_SIZE;x++ ) {
                    if (x >= SizeGeneratior.WorldWidth) break;
                    MapHeight[x] = blocks ? 1:0;
                } 
                blocks = !blocks;    
            }
        }

    }
}
