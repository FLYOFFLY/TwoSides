using System;

using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    [Serializable]
    public class TreeDate : ITileDatecs
    {
        public short TypeTree;

        public void Init()
        {
            TypeTree = 0;
        }
    }
}
