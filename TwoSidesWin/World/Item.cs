using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TwoSides.ModLoader;
using System.IO;

namespace TwoSides.World
{
    [Serializable]
    sealed public class Item
    {
        public bool IsEmpty = true;
        public int ammount = 0;
        public int iditem;
        public int power;
        public float HP { get; set; }
        public  enum Type
        {
            BLOCKICON = 0,
            HORISONTALBLOCKICON, // 1
            VERTICALBLOCKICON, //2
            EAT, //3
            WATER, //4
            BANDAGE,//5
            HEAD,//6
            BODY,//7
            LEGS,//8
            WEAPONGUN,//9
            MODDINGITEM,//10
            CRAFTITEM,//11
            PICKAXE,
            SWORD,
            HAMMER,
        };


        public  const int itemmax = 46;
        public static Texture2D[] items = new Texture2D[itemmax + Clothes.maxArmor];
        public void save(BinaryWriter writer)
        {
            writer.Write(IsEmpty);
            writer.Write(ammount);
            writer.Write((double)HP);
            writer.Write(power);
        }
        public void load(BinaryReader reader)
        {
            IsEmpty = reader.ReadBoolean();
            ammount = reader.ReadInt32();
            HP = (float)reader.ReadDouble();
            power = reader.ReadInt32();
        }
        public int getTypeItem()
        {
            if (iditem == -1) return (int)Item.Type.CRAFTITEM;
            if (iditem == 8) return (int)Item.Type.PICKAXE;
            if (iditem == 41) return (int)Item.Type.PICKAXE;
            if (iditem == 42) return (int)Item.Type.HAMMER;
            if (iditem == 43) return (int)Item.Type.HAMMER;
            if (iditem == 44) return (int)Item.Type.SWORD;
            if (iditem == 45) return (int)Item.Type.SWORD;


            if (iditem == 19) return (int)Item.Type.CRAFTITEM;
            if (iditem == 20) return (int)Item.Type.CRAFTITEM;
            if (iditem == 21) return (int)Item.Type.CRAFTITEM;
            if (iditem == 29) return (int)Item.Type.CRAFTITEM;
            if (iditem == 30) return (int)Item.Type.CRAFTITEM;
            if (iditem == 37) return (int)Item.Type.CRAFTITEM;
            if (iditem == 36) return (int)Item.Type.CRAFTITEM;
            if (iditem == 34) return (int)Item.Type.SWORD;
            if (iditem == 38) return (int)Item.Type.CRAFTITEM;

            if (iditem == 47) return (int)Item.Type.CRAFTITEM;
            if (iditem == 49) return (int)Item.Type.CRAFTITEM;
            if (iditem == 50) return (int)Item.Type.CRAFTITEM;
            if (iditem == 51) return (int)Item.Type.CRAFTITEM;

            if (iditem == 22 || iditem == 31) return (int)Item.Type.HEAD;
            if (iditem == 23 || iditem == 32) return (int)Item.Type.BODY;
            if (iditem == 24 || iditem == 33) return (int)Item.Type.LEGS;
            if (iditem == 25) return (int)Item.Type.WEAPONGUN;
            if (iditem == 26) return (int)Item.Type.VERTICALBLOCKICON;
            if (iditem == 27) return (int)Item.Type.HORISONTALBLOCKICON;

            if (iditem == 10) return (int)Item.Type.WATER;
            if (iditem == 48) return (int)Item.Type.WATER;
            if (iditem == 11) return (int)Item.Type.EAT;
            if (iditem == 14) return (int)Item.Type.BANDAGE;

            if (iditem >= ModLoader.ModLoader.StartIdItem)
            {
                Console.WriteLine(iditem);
                return (int)((ItemMod)ModLoader.ModLoader.ItemsMods[ModLoader.ModLoader.getIndexItem(iditem)]).type;
            } return (int)Item.Type.BLOCKICON;
        }

        public Item()
        {
            HP = 100;
        }

        public Item(int ammount,int iditem)
        {
            this.ammount = ammount;
            this.iditem = iditem;
            this.IsEmpty = false;
            HP = 100;
        }
        public int getHunger()
        {
            return 0;
        }
        public int getArmorModel()
        {
            if (iditem == 22) return 0;
            if (iditem == 23) return 1;
            if (iditem == 24) return 2;
            if (iditem == 31) return 3;
            if (iditem == 32) return 4;
            if (iditem == 33) return 5;
            return 0;
        }

        public int getArmorSlot()
        {
            if (iditem == 22) return 1;
            if (iditem == 23) return 2;
            if (iditem == 24) return 3;
            if (iditem == 31) return 1;
            if (iditem == 32) return 2;
            if (iditem == 33) return 3;
            return 0;
        }

        public int getDef()
        {
            if (iditem == 22) return 3;
            if (iditem == 23) return 2;
            if (iditem == 24) return 3;
            if (iditem == 31) return 4;
            if (iditem == 32) return 3;
            if (iditem == 33) return 4;
            return 0;
        }
        public float getMass()
        {
            if (iditem == 22) return 30;
            if (iditem == 23) return 30;
            if (iditem == 24) return 30;
            if (iditem == 31) return 100;
            if (iditem == 32) return 100;
            if (iditem == 33) return 100;
            return 0;
        }
        public float getMassFactor()
        {
            if (getMass() >= 1) return (getMass() / 100);
            else return 0.01f;
        }
        public float getSubMassFactor()
        {
            if (getMass() >= 1) return 1.0f-getMassFactor();
            else return 0.01f;
        }
        public float getSpeedModdif()
        {
            if (getArmorSlot() == 0) return 1.0f-((getMass() * 0.5f)/100.0f);
            if (getArmorSlot() == 1) return 1.0f-((getMass() * 0.5f)/100.0f);
            if (getArmorSlot() == 2) return 1.0f - ((getMass() * 0.8f) / 100.0f);
            
            return 1;
        }

        public void damageslot(float dmg)
        {
            if (!IsEmpty)
            {
                if (ammount == 1)
                {
                    HP -= dmg;
                    if (HP <= 0.01f) HP = 0.01f;
                }
                else
                {
                    Item num = new Item();
                    num.ammount = ammount - 1;
                    num.iditem = iditem;
                    num.power = power;
                    num.IsEmpty = false;
                    ammount = 1;
                    damageslot(dmg);
                    int a = Program.game.player.getslotitem(iditem, num.HP, false);
                    if (a == -1)
                    {
                        Program.game.player.slot[Program.game.player.getslotfull()] = num;
                    }
                    else { Program.game.player.slot[a].ammount += num.ammount; }
                }
            }
        }

        public int getBlockId()
        {
            if (iditem == 9) return 8;
            if (iditem == 15) return 9;
            if (iditem == 16) return 10;
            if (iditem == 17) return 11;
            if (iditem == 18) return 12;
            if (iditem == 26) return 19;
            if (iditem == 27) return 17;
            if (iditem == 28) return 22;
            if (iditem == 39) return 26;
            if (iditem == 40) return 25;
            if (iditem == 48) return 28;
            if (iditem >= ModLoader.ModLoader.StartIdItem)
            {
                Console.WriteLine(iditem);
                if (((ItemMod)ModLoader.ModLoader.ItemsMods[ModLoader.ModLoader.getIndexItem(iditem)]).type == 0)
                    return ((ItemMod)ModLoader.ModLoader.ItemsMods[ModLoader.ModLoader.getIndexItem(iditem)]).param;
                else
                    return iditem;
            } return iditem;
        }

        public string GetName()
        {
            if (iditem == 0) return "dirt";
            if (iditem == 1) return "stone";
            if (iditem == 2) return "Gold Ore";
            if (iditem == 3) return "Iron Ore";
            if (iditem == 4) return "SnowDirt";
            if (iditem == 5) return "Wood";
            if (iditem == 6) return "Torch";
            if (iditem == 7) return "Coal";
            if (iditem == 8) return "Stone Pickaxe";
            if (iditem == 9) return "Chest";
            if (iditem == 10) return "Water";
            if (iditem == 11) return "Burger";
            if (iditem == 12) return "???";
            if (iditem == 13) return "String";
            if (iditem == 14) return "plaster";
            if (iditem == 15) return "Iron Block";
            if (iditem == 16) return "HellDirt";
            if (iditem == 18) return "Portal";
            if (iditem == 19) return "Stick";
            if (iditem == 20) return "Stick Stone";
            if (iditem == 21) return "Stick Iron";
            if (iditem == 22) return "Stone helmet  ";
            if (iditem == 23) return "Stone Breakplast";
            if (iditem == 24) return "stone Shoes";
            if (iditem == 25) return "stone GUN";
            if (iditem == 26) return "Stul";
            if (iditem == 27) return "TABLE";
            //
            if (iditem == 28) return "Furnace";
            if (iditem == 29) return "Iron Bar";
            if (iditem == 30) return "Gold Bar";
            if (iditem == 31) return "Chain Helmet";
            if (iditem == 32) return "Chain Mail";
            if (iditem == 33) return "Chain boots";
            if (iditem == 34) return "Iron Sword";
            if (iditem == 35) return "Iron bar(?)";
            if (iditem == 36) return "Clay";
            if (iditem == 37) return "Sand";
            if (iditem == 38) return "Brick bar";
            if (iditem == 39) return "Brick";
            if (iditem == 40) return "Sandstone";
            if (iditem == 41) return "Iron PickAxe";
            if (iditem == 42) return "Snow Hammer";
            if (iditem == 43) return "Stone Hammer";
            if (iditem == 44) return "Copper Sword";
            if (iditem == 45) return "Stone Sword";
            if (iditem == 46) return "Cactus";
            if (iditem == 47) return "Cactus Thorn";
            if (iditem == 48) return "Cactus juice";
            if (iditem == 49) return "Wooden tubule";
            return "???";
        }

        public int GetStack()
        {
            if (iditem == 8) return 1;
            if (iditem == 10) return 5;
            if (iditem == 11) return 5;
            if (iditem == 12) return 5;
            if (iditem == 13) return 5;
            if (iditem == 14) return 5;
            if (iditem == 22 || iditem == 23 || iditem == 24 || iditem == 25 || iditem == 31 || iditem == 32 || iditem == 33 || iditem == 34 ||
                iditem == 41 || iditem == 42 || iditem == 43 || iditem == 44 || iditem == 45) return 1;
            return 64;
        }

        public float GetPower()
        {
            if (iditem == 8) return 2f;
            if (iditem == 41) return 2f;
            if (iditem == 42) return 2f;
            if (iditem == 43) return 2f;
            return 0;
        }

        public float Getdamage()
        {
            if (iditem == 1) return 0.6f;
            if (iditem == 5) return 0.55f;
            if (iditem == 8) return 2f;
            if (iditem == 15) return 1f;
            if (iditem == 19) return 0.60f;
            if (iditem == 28) return 1f;
            if (iditem == 34) return 5f;
            if (iditem == 41) return 3f;
            if (iditem == 42) return 4f;
            if (iditem == 43) return 2f;
            if (iditem == 44) return 4f;
            if (iditem == 45) return 3f;
            return 0;
        }

        public Color GetColor()
        {

            if (iditem == 0 || iditem == 1 || iditem == 4 || iditem == 5 || iditem == 6 || iditem == 7 || iditem == 16 || iditem == 19||
                iditem == 26 || iditem == 27 || iditem == 36 || iditem == 37 || iditem == 38 || iditem == 39|| iditem == 40) return Rate.NORMAL_RATE;
            if (iditem == 8 || iditem == 28 || iditem == 43 || iditem == 45) return Rate.STONE_RATE;
            if (iditem == 10 || iditem == 11 || iditem == 12 || iditem == 13 || iditem == 14) return Rate.DROP_RATE;
            if (iditem == 2 || iditem == 3 || iditem == 30 || iditem == 29 || iditem == 44) return Rate.ORE_RATE;
            if (iditem == 15 || iditem == 22 || iditem == 23 || iditem == 24 || iditem == 31 || iditem == 32|| iditem==33 || iditem == 34||
                iditem == 41) return Rate.IRON_RATE;
            if (iditem == 25 || iditem == 42) return Rate.LEGEND_RATE;
            return Color.Black;
        }

        public static void Render(SpriteBatch spriteBatch,int id, int x, int y)
        {
            if (id >= ModLoader.ModLoader.StartIdItem)
            {
                id = ModLoader.ModLoader.getIndexItem(id);
                spriteBatch.Draw(((ItemMod)ModLoader.ModLoader.ItemsMods[id]).texture, new Rectangle(x, y, 16, 16), Color.White);
            }
            else spriteBatch.Draw(items[id], new Rectangle(x, y, 16, 16), Color.White);

        }
        public void Render(SpriteBatch spriteBatch, int x, int y)
        {
            Render(spriteBatch, iditem, x, y);
            
        }
        public static void LoadedItems(ContentManager Content)
        {

            Program.StartSw();
            for (int i = 0; i < itemmax + Clothes.maxArmor; i++)
            {
                items[i] = Content.Load<Texture2D>(Game1.ImageFolder + "items\\" + i);
            }
            Program.StopSw("Loaded Sprite Items");
        }

    }
}
