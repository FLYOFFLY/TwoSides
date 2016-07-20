using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Generation;

namespace TwoSides.World
{
    public class Sector
    {
        const int SectorWidth = 10;
        const int SectorHeight = 10;
        BaseDimension[,] world = new BaseDimension[SectorWidth, SectorHeight];
    }
}
