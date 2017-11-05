using System;

using TwoSIdes.GameContent.GenerationResources;
using TwoSIdes.GUI;

namespace TwoSIdes.GameContent.Dimensions
{
    [Serializable]
    public class SnowWorld : NormalWorld
    {
        protected override void GenerationBiomes(ProgressBar bar)
        {
            for (int i = 0; i < MapBiomes.Length; i++)
            {
                MapBiomes[i] = ArrayResource.WorldShow;
            }
        }
    }
}
