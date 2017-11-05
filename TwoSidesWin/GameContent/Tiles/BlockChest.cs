using System.Collections.Generic;
using System.Linq;

using TwoSIdes.Physics.Entity;
using TwoSIdes.World;
using TwoSIdes.World.Generation;
using TwoSIdes.World.Tile;

using Drop = TwoSIdes.World.Drop;

namespace TwoSIdes.GameContent.Tiles
{
    public class BlockChest : BaseTile
    {
        readonly Drop[] _dropListChest = {  new Drop(5, 1, 60, new Item(0,11)),
                                                new Drop(5, 1, 60, new Item(0,10)), 
                                                new Drop(5, 1, 80, new Item(0,14)) };
        public BlockChest(int maxHp,int Id)
            : base(maxHp,Id)
        {
        }
        public override List<Item> Destory(int x, int y, BaseDimension dimension, DynamicEntity entity) =>
            _dropListChest.Select(dropingChest =>
                                    new Item(Program.Game.Dimension[Program.Game.CurrentDimension].Rand.Next(dropingChest.MinCount ,
                                                                                                    dropingChest.MaxCount) ,
                                                dropingChest.Item.Id)).ToList();
        public override bool HasShadow() => false;
    }
}
