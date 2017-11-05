using System;

namespace TwoSides.World.Generation.Structures
{
    public class BaseStruct
    {
        [NonSerialized]
        public bool Isplaying;

        [NonSerialized]
        public int X, Y;
        public BaseStruct(int x, int y)
        {
            X = x;
            Y = y;
            Isplaying = false;
        }
        public virtual void Spawn(BaseDimension dimension) { }
    }
}
