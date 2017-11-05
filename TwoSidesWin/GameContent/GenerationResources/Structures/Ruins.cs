using TwoSides.World.Generation;
using TwoSides.World.Generation.Structures;

namespace TwoSides.GameContent.GenerationResources.Structures
{
    internal class Ruins : BaseStruct
    {
        public Ruins(int x, int y) : base(x, y)
        {
        }
        public override void Spawn(BaseDimension dimension)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                    dimension.SetTexture(X + i, j + Y, 35);
            }
        }
    }
}
