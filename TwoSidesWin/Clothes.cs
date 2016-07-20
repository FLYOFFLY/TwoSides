using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace TwoSides
{
    /*
            int hairid = clslot[0].getid();
            int shirtid = clslot[1].getid();
            int pantsid = clslot[2].getid();
            int shoesid = clslot[3].getid();
            int beltid = clslot[4].getid();
            int glovesid = clslot[5].getid();*/
    [Serializable]
    sealed public class Clothes
    {
                                               
        public const int maxShirt = 3;
        public const int maxCostume = 3;
        public const int maxShoes = 1;
        public const int maxPants = 1;
        public const int maxBelt = 1;
        public const int maxHair = 1;
        public const int maxGlove = 1;

        int idclothes;
        public int temperature = 0;
        bool isactive = false;

        public const int maxArmor = 6;
        public static ArrayList beltMods = new ArrayList();
        public static ArrayList shirtMods = new ArrayList();
        public static ArrayList pantsMods = new ArrayList();
        public static ArrayList specialMods = new ArrayList();
        public static Texture2D[] shirt = new Texture2D[maxShirt];
        public static Texture2D[] shoes = new Texture2D[maxShoes];
        public static Texture2D[] pants = new Texture2D[maxPants];
        public static Texture2D[] suit = new Texture2D[maxCostume];
        public static Texture2D[] gloves = new Texture2D[maxGlove];
        public static Texture2D[] belt = new Texture2D[maxBelt];
        public static Texture2D[] hair = new Texture2D[maxHair];
        public static Texture2D[] armor = new Texture2D[maxArmor];

        public Clothes()
        {
            isactive = false;
            temperature = 0;
        }

        public Clothes(int id,int temperature = 0)
        {
            idclothes = id;
            isactive = true;
            this.temperature = temperature;
        }
        public void load(BinaryReader reader)
        {
            idclothes = reader.ReadInt32();
            isactive = reader.ReadBoolean();
            temperature = reader.ReadInt32();
        }
        public void save(BinaryWriter writer){
            writer.Write(idclothes);
            writer.Write(isactive);
            writer.Write(temperature);
        }
        public int getid()
        {
            if (isactive)
            {
                return idclothes;
            }
            return -1;
        }

        public static void Loadclothes(ContentManager content )
        {
            Program.StartSw();
            for (int i = 0; i < Clothes.maxPants; i++)
            {
                pants[i] = content.Load<Texture2D>(Game1.ImageFolder + "clothes\\pants_" + i);
            }
            for (int i = 0; i < Clothes.maxCostume; i++)
            {
                suit[i] = content.Load<Texture2D>(Game1.ImageFolder +"clothes\\suit_" + i);
            }
            for (int i = 0; i < Clothes.maxShirt; i++)
            {
                shirt[i] = content.Load<Texture2D>(Game1.ImageFolder +"clothes\\shirt_" + i);
            }

            for (int i = 0; i < Clothes.maxBelt; i++)
            {
                belt[i] = content.Load<Texture2D>(Game1.ImageFolder + "clothes\\belt_" + i);
            }
            for (int i = 0; i < Clothes.maxShoes; i++)
            {
                shoes[i] = content.Load<Texture2D>(Game1.ImageFolder + "clothes\\shoes_" + i);
            }
            for (int i = 0; i < Clothes.maxGlove; i++)
            {
                gloves[i] = content.Load<Texture2D>(Game1.ImageFolder + "clothes\\glove_" + i);
            }

            for (int i = 0; i < Clothes.maxArmor; i++)
            {
                armor[i] = content.Load<Texture2D>(Game1.ImageFolder + "armor\\" + i);
            }
            for (int i = 0; i < Clothes.maxHair; i++)
            {
                hair[i] = content.Load<Texture2D>(Game1.ImageFolder + "clothes\\hair_" + i);
            }
            Program.StopSw("Loaded clothes");
        }
    }
}
