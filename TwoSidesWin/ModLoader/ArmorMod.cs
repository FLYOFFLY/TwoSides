using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.ModLoader
{
    public class ArmorMod
    {
        Texture2D armor ;
        public ArmorMod(Texture2D armor)
        {
            this.armor = armor;
        }
        public Texture2D renderArmor()
        {
            return armor;
        }
    }
}
