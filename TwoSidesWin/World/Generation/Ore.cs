using TwoSIdes.GameContent.GenerationResources;

namespace TwoSIdes.World.Generation
{
    public class Ore
    {
        public int Change {get; }
        public int Range { get; }
        public int Idblock { get;  }
        public int MaxY { get; }
        public int MinY { get; }

        public Ore(int change, int range,int idblock,int maxY,int minY)
        {
            Change = change;
            Range = range;
            Idblock = idblock;
            MaxY = maxY;
            if (MaxY >= SizeGeneratior.WorldHeight)
                MaxY = SizeGeneratior.WorldHeight - 1;
            MinY = minY;
        }
    }
}
