using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TwoSIdes.Utils;

namespace TwoSIdes.World
{
    [Serializable]
    public sealed class Item
    {
        public bool IsEmpty = true;
        public int Ammount;
        public int Id;
        public int Power;
        public float Hp { get; set; }
        public  enum Type
        {
            BLOCKICON = 0,
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
            HAMMER
        }


        public  const int ItemMax = 55;
        static readonly Texture2D[] Items = new Texture2D[ItemMax + Clothes.MaxArmor];
        public void Save(BinaryWriter writer)
        {
            writer.Write(IsEmpty);
            writer.Write(Ammount);
            writer.Write((double)Hp);
            writer.Write(Power);
        }
        public void Load(BinaryReader reader)
        {
            IsEmpty = reader.ReadBoolean();
            Ammount = reader.ReadInt32();
            Hp = (float)reader.ReadDouble();
            Power = reader.ReadInt32();
        }
        public int GetTypeItem()
        {
            if ( IsPickaxe )
                return (int) Type.PICKAXE;
            else if ( IsHammer )
                return (int) Type.HAMMER;
            else if ( IsSword )
                return (int) Type.SWORD;
            else if ( IsCraftitem )
                return (int) Type.CRAFTITEM;
            else if ( IsHeadArmor )
                return (int) Type.HEAD;
            else if ( IsBodyArmor )
                return (int) Type.BODY;
            else if ( IsLegsArmor )
                return (int) Type.LEGS;
            else if (IsGun)
                return (int) Type.WEAPONGUN;
            else if ( Id == 26 )
                return (int) Type.VERTICALBLOCKICON;
            else if ( IsWater )
                return (int) Type.WATER;
            else if ( IsEat)
                return (int) Type.EAT;
            else if ( Id == 14 )
                return (int) Type.BANDAGE;
            else
                return (int) Type.BLOCKICON;
        }

        bool IsEat => Id == 55 || Id == 53 ||  Id == 11;

        bool IsWater => Id == 48 || Id==10;

        bool IsLegsArmor => Id == 24 || Id == 33;

        bool IsBodyArmor => Id == 23 || Id == 32;

        bool IsHeadArmor => Id == 22 || Id == 31;

        bool IsCraftitem => Id == -1 || Id == 19 || Id == 20 || Id == 21 || Id == 29 || Id == 30 || Id == 37 || Id == 36 ||
                            Id == 38 || Id == 47 || Id == 49 || Id == 50 || Id == 51 || Id == 52 || Id == 54;

        bool IsBandage => Id == 14;
        bool IsSword => Id == 44 || Id == 45 || Id == 34;
        bool IsGun => Id == 25;
        bool IsHammer => Id == 42 || Id == 43;

        bool IsPickaxe => Id == 8 || Id == 41;

        public bool IsTool => IsPickaxe || IsSword || IsHammer;
        public bool IsArmor => IsBodyArmor || IsLegsArmor || IsHeadArmor;
        public bool IsHealth => IsEat || IsWater || IsBandage;

        public Item() => Hp = 100;

        public Item(int ammount,int id)
        {
            Ammount = ammount;
            Id = id;
            IsEmpty = false;
            Hp = 100;
        }
        public int GetHunger()
        {
            switch (Id)
            {
                case 11:
                    return 10;
                case 53:
                    return 4;
                case 55:
                    return 6;
                default:
                    return 0;
            }
        }
        public int GetArmorModel()
        {
            switch (Id)
            {
                case 22:
                    return 0;
                case 23:
                    return 1;
                case 24:
                    return 2;
                case 31:
                    return 3;
                case 32:
                    return 4;
                case 33:
                    return 5;
                default:
                    return 0;
            }
        }

        public int GetArmorSlot()
        {
            switch (Id)
            {
                case 22:
                case 31:
                    return 1;
                case 32:
                case 23:
                    return 2;
                case 33:
                case 24:
                    return 3;
                default:
                    return 0;
            }
        }

        public int GetDef()
        {
            switch ( Id ) {
                case 23:
                    return 2;
                case 31:
                    return 4;
                case 33:
                    return 4;
                case 32:
                case 24:
                case 22:
                    return 3;
                default:
                    return 0;
            }
        }
        public float GetMass()
        {
            switch ( Id ) {
                case 22:
                case 23:
                case 24:
                    return 30;
                case 31:
                case 32:
                case 33:
                    return 100;
                default:
                    return 0;
            }
        }
        public float GetMassFactor()
        {
            if (GetMass() >= 1) return GetMass() / 100;

            return 0.01f;
        }
        public float GetSubMassFactor()
        {
            if (GetMass() >= 1) return 1.0f-GetMassFactor();

            return 0.01f;
        }
        public float GetSpeedModdif()
        {
            switch (GetArmorSlot())
            {
                case 0:
                case 1:
                    return 1.0f - GetMass() * 0.5f / 100.0f;
                case 2:
                    return 1.0f - GetMass() * 0.8f / 100.0f;
                default:
                    return 1;
            }
        }

        public void DamageSlot(float dmg)
        {
            if ( IsEmpty ) return;

            if (Ammount == 1)
            {
                Hp -= dmg;
                if (Hp <= 0.01f) Hp = 0.01f;
            }
            else
            {
                Item num = new Item
                           {
                               Ammount = Ammount - 1 ,
                               Id = Id ,
                               Power = Power ,
                               IsEmpty = false
                           };
                Ammount = 1;
                DamageSlot(dmg);
                int a = Program.Game.Player.GetSlotItem(Id, num.Hp, false);
                if (a == -1)
                {
                    Program.Game.Player.Slot[Program.Game.Player.GetSlotFull()] = num;
                }
                else { Program.Game.Player.Slot[a].Ammount += num.Ammount; }
            }
        }

        public int GetBlockId()
        {
            switch ( Id ) {
                case 9:
                    return 8;
                case 15:
                    return 9;
                case 16:
                    return 10;
                case 17:
                    return 11;
                case 18:
                    return 12;
                case 26:
                    return 19;
                case 27:
                    return 17;
                case 28:
                    return 22;
                case 39:
                    return 26;
                case 40:
                    return 25;
                case 48:
                    return 28;
                default:

                    if (Id >= 56 && Id <= 60) return Id - 23;
                    return Id;
            }
        }
        public string GetName()
        {
            if ( Id < 0 || Id >= ItemMax ) return "???";

            string name = $"Item_{Id}";
            return Localisation.GetName(name);
        }
        
        public int GetStack()
        {
            switch ( Id ) {
                case 8:
                    return 1;
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    return 5;
                case 22:
                case 23:
                case 24:
                case 25:
                case 31:
                case 32:
                case 33:
                case 34:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                    return 1;
                default:
                    return 64;
            }
        }

        public float GetPower()
        {
            switch ( Id ) {
                case 8:
                case 41:
                case 42:
                case 43:
                    return 2f;
                default:
                    return 0;
            }
        }

        public float GetDamage()
        {
            switch ( Id ) {
                case 5:
                    return 0.55f;
                case 1:
                case 19:
                    return 0.6f;
                case 8:
                case 43:
                    return 2f;
                case 15:
                case 28:
                    return 1f;
                case 34:
                    return 5f;
                case 41:
                case 45:
                    return 3f;
                case 42:
                case 44:
                    return 4f;
                default:
                    return 0;
            }
        }

        public Color GetColor()
        {

            switch ( Id ) {
                case 0:
                case 1:
                case 4:
                case 5:
                case 6:
                case 7:
                case 16:
                case 19:
                case 26:
                case 27:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                    return Rate.Normal;
                case 8:
                case 28:
                case 43:
                case 45:
                    return Rate.Stone;
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    return Rate.Drop;
                case 2:
                case 3:
                case 30:
                case 29:
                case 44:
                    return Rate.Ore;
                case 15:
                case 22:
                case 23:
                case 24:
                case 31:
                case 32:
                case 33:
                case 34:
                case 41:
                    return Rate.Iron;
                case 25:
                case 42:
                    return Rate.Legend;
                default:
                    return Color.Black;
            }
        }
        public static void Render(SpriteBatch spriteBatch, int id, Rectangle desc, SpriteEffects effect, float angle, Vector2 center)
        {
            Texture2D texture = GetTexture(id);
            center.X *= texture.Width;
            center.Y *= texture.Height;
            spriteBatch.Draw(texture,
                           desc,
                           new Rectangle(0, 0, texture.Width, texture.Height),
                           Color.White, angle, center, effect, 0);
        }
        public static void Render(SpriteBatch spriteBatch,int Id,int x,int y,SpriteEffects effect,float angle,Vector2 center)
        {
            Render(spriteBatch, Id, new Rectangle(x, y, 16, 16), effect, angle, center);
        }
        public static void Render(SpriteBatch spriteBatch, int Id, int x,int y)
        {
            Render(spriteBatch, Id, new Rectangle(x, y, 16, 16));
        }

        static Texture2D GetTexture(int id) => Items[id];

        public static void Render(SpriteBatch spriteBatch,int Id,Rectangle dest)
        {
            Render(spriteBatch, Id, dest, SpriteEffects.None, 0, Vector2.Zero);
        }
        public void Render(SpriteBatch spriteBatch, int x, int y)
        {
            Render(spriteBatch, Id, x, y);

        }
        public void Render(SpriteBatch spriteBatch, Rectangle dest)
        {
            Render(spriteBatch, Id, dest);

        }
        public static void LoadedItems(ContentManager content)
        {

            Program.StartSw();
            for (int i = 0; i < ItemMax + Clothes.MaxArmor; i++)
            {
                Items[i] = content.Load<Texture2D>(Game1.ImageFolder + "items\\" + i);
            }
            Program.StopSw("Loaded Sprite Items");
        }

    }
}
