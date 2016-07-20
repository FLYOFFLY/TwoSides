using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class LightBlock : BaseTile
    {
        
        public LightBlock(float MaxHP,int id) : base(MaxHP,id)
        {
        }
        public override bool issolid()
        {
            return false;
        }
        public override bool isLightBlock()
        {
            return true;
        }

        public override int getAnimFrame()
        {
            return 2;
        }

        public override int getTickFrame()
        {
            return 5;
        }
    }
}
