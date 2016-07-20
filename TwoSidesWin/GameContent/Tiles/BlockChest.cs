using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.World.Tile;
using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.World.Generation;

namespace TwoSides.GameContent.Tiles
{
    public class BlockChest : BaseTile
    {
        private DROP[] DropListChest = {  new DROP(5, 1, 60, new Item(0,11)),
                                                new DROP(5, 1, 60, new Item(0,10)), 
                                                new DROP(5, 1, 80, new Item(0,14)) };
        public BlockChest(int maxHP,int id)
            : base(maxHP,id)
        {
        }
        public override List<Item> destory(int x, int y, BaseDimension dimension, CEntity entity)
        {
            List<Item> a = new List<Item>();
            foreach (DROP dropingChest in DropListChest)
            {
                //if (Program.game.rand.Next(0, 100) <= dropingChest.Сhance*(Program.game.player.carma/100)) 
                a.Add(new Item(Program.game.dimension[Program.game.currentD].rand.Next(dropingChest.MinCount, dropingChest.MaxCount),
                    dropingChest.item.iditem));
            }
            return a;
        }
        public override bool hasShadow()
        {
            return false;
        }
    }
}
