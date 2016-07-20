using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.Dimensions;
using TwoSides.World.Generation;

namespace TwoSides.World.Structures
{
    class Caves:BaseStruct
    {

        [NonSerialized]
        int radious;
        [NonSerialized]
        public static Random rand = new Random();

        public Caves(int x, int y):base(x,y+5)
        {
            rand = new Random((int)DateTime.Now.Ticks);
                radious = rand.Next(3, 8);
        }
        public override void spawn(BaseDimension dimension)
        {
            NormalWorld world = (NormalWorld)dimension;
            world.CaveOpenater(x, y);
            world.Cavinator(x, y,rand.Next(2,3));
            
            return;
        }
    }
}
