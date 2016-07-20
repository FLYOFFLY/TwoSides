using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoSides
{
    public class Perlin
    {
        public static float QunticCurve(float t)
        {
            return t*t*t*(t*(t*6-15)+10);
        }

    }
}
