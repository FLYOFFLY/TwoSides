using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GameContent.GenerationResources;

namespace TwoSides.World.Generation
{
    public class Ore
    {
        public int change {get; private set; }
        public int range { get; private set; }
        public int idblock { get; private set; }
        public int maxY { get; private set; }
        public int minY { get; private set; }

        public Ore(int change, int range,int idblock,int maxY,int MinY)
        {
            this.change = change;
            this.range = range;
            this.idblock = idblock;
            this.maxY = maxY;
            if (this.maxY >= SizeGeneratior.WorldHeight)
                maxY = SizeGeneratior.WorldHeight - 1;
            this.minY = minY;
        }
    }
}
