using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Generation;

namespace TwoSides.World.Structures
{
    public class BaseStruct
    {
        [NonSerialized]
        public bool isplaying;

        [NonSerialized]
        public int x, y;
        public BaseStruct(int x, int y)
        {
            this.x = x;
            this.y = y;
            isplaying = false;
        }
        public virtual void spawn(BaseDimension dimension) { }
    }
}
