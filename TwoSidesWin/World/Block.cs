using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using System.Collections;
namespace TwoSides.World
{
    public class DROP
    {
        public int MaxCount;
        public int MinCount;
        public int Сhance;
        public Item item;
        public DROP(int maxCount,int minCount,int chance,Item item)
        {
            this.MinCount   = minCount;
            this.MaxCount   = maxCount;
            this.Сhance     = chance;
            this.item = item;
        }
    }

}
