using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GUI;

namespace TwoSides.GameContent.Dimensions
{
    [Serializable]
    public class SnowWorld : NormalWorld
    {
        public SnowWorld():base(){}
        protected override void GenerationBiomes(ProgressBar bar)
        {
            for (int i = 0; i < SizeGeneratior.WorldWidth; i++)
            {
                mapB[i] = ArrayResource.worldshow;
            }
        }
    }
}
