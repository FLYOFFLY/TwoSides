using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Generation;
using TwoSides.World;
using Microsoft.Xna.Framework;
namespace TwoSides.GameContent.GenerationResources
{
    public class ArrayResource
    {
        public static List<Ore> ores = new List<Ore>() { new Ore(4, 5, 7,SizeGeneratior.rockLayer+500,SizeGeneratior.rockLayer-20), 
                                                         new Ore(1, 9, 2,SizeGeneratior.WorldHeight,SizeGeneratior.rockLayer),
                                                         new Ore(2, 9, 3,SizeGeneratior.WorldHeight-2,SizeGeneratior.rockLayer) };

        public static Biome grass =     new Biome(25, 0, SizeGeneratior.rockLayer - 50, SizeGeneratior.rockLayer - 48, 0, 1,Color.Green);
        public static Biome Hills =     new Biome(15, 1, SizeGeneratior.rockLayer - 100, SizeGeneratior.rockLayer - 50, 23, 1,Color.Brown);
        public static Biome snow = new Biome(-10, 2, SizeGeneratior.rockLayer - 50, SizeGeneratior.rockLayer - 10, 4, 1, Color.LightBlue);
        public static Biome Desrt = new Biome(35, 3, SizeGeneratior.rockLayer - 100, SizeGeneratior.rockLayer - 50, 24, 1, Color.LightYellow);
        public static Biome worldshow = new Biome(-50, 4, SizeGeneratior.rockLayer - 50, SizeGeneratior.rockLayer - 2, 4, 4, Color.DarkBlue);
        public static Biome hell =      new Biome(-50, 5, 1, 1, 10, 10,Color.DarkRed);
    }
}
