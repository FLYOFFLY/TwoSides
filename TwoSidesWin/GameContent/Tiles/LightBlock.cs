using TwoSIdes.World.Tile;

namespace TwoSIdes.GameContent.Tiles
{
    public class LightBlock : BaseTile
    {
        
        public LightBlock(float maxHp,int id) : base(maxHp,id)
        {
        }
        public override bool IsSolId() => false;

        public override bool IsLightBlock() => true;

        public override int GetAnimFrame() => 2;

        public override int GetTickFrame() => 5;
    }
}
