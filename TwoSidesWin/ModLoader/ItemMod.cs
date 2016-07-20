using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.ModLoader
{
    public class ItemMod
    {
        public int ID;
        public int type;
        public int param;
        public string modPath;
        public string IMG;
        public Texture2D texture;
        public ItemMod(int id, string img,int type,int param,string ModPath)
        {
            System.Console.WriteLine(id);
            this.ID = id;
            this.IMG = img;
            this.type = type;
            this.param = param;
            this.modPath = ModPath;
        }
        
        public void setTexture(Texture2D tex){
            this.texture = tex;
        }

        public override bool Equals(object obj)
        {
            if (obj is ItemMod) { 
                ItemMod bm = (ItemMod)obj;
                if (bm.ID == ID) return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
