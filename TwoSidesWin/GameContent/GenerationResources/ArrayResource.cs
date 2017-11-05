using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TwoSides.World.Generation;

namespace TwoSides.GameContent.GenerationResources
{
    public static class ArrayResource
    {
        static ArrayResource()
        {
            Grass = new Biome(25, 0, SizeGeneratior.RockLayer - 50, SizeGeneratior.RockLayer - 48, 0, 1, Color.Green);
            Ores = new List<Ore>
                   { new Ore(4, 5, 7,SizeGeneratior.RockLayer+500,SizeGeneratior.RockLayer-20),
                       new Ore(1, 9, 2,SizeGeneratior.WorldHeight,SizeGeneratior.RockLayer),
                       new Ore(2, 9, 3,SizeGeneratior.WorldHeight-2,SizeGeneratior.RockLayer) };
            Hills = new Biome(15, 1, SizeGeneratior.RockLayer - 100, SizeGeneratior.RockLayer - 50, 23, 1,Color.Brown);
            Snow = new Biome(-10, 2, SizeGeneratior.RockLayer - 50, SizeGeneratior.RockLayer - 10, 4, 1, Color.LightBlue);
            Desrt = new Biome(35, 3, SizeGeneratior.RockLayer - 100, SizeGeneratior.RockLayer - 50, 24, 1, Color.LightYellow);
            WorldShow = new Biome(-50, 4, SizeGeneratior.RockLayer - 50, SizeGeneratior.RockLayer - 2, 4, 4, Color.DarkBlue);
            Hell = new Biome(-50, 5, 1, 1, 10, 10,Color.DarkRed);
        }
        public static List<Ore> Ores { get; }

        public static Biome Grass { get;}
        public static Biome Hills { get; }
        public static Biome Snow { get; }
        public static Biome Desrt { get; }
        public static Biome WorldShow { get; }
        public static Biome Hell { get; }
        
    }
}
