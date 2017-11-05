using System;

using TwoSides.GameContent.Dimensions;
using TwoSides.World.Generation;
using TwoSides.World.Generation.Structures;

namespace TwoSides.GameContent.GenerationResources.Structures
{
    internal class Caves:BaseStruct
    {
        [NonSerialized]
        public static Random Rand ;

        public Caves(int x, int y):base(x,y+5) => Rand = new Random((int)DateTime.Now.Ticks);

        public override void Spawn(BaseDimension dimension)
        {
            NormalWorld world = (NormalWorld)dimension;
            world.CaveOpenater(X, Y);
            world.Cavinator(X, Y,Rand.Next(2,3));
        }
    }
}
