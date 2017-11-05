using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using static System.Console;

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
    public sealed class Clothes
    {
        public const int MaxShirt = 2;
        public const int MaxCostume = 3;
        public const int MaxShoes = 1;
        public const int MaxPants = 1;
        public const int MaxBelt = 1;
        public const int MaxHair = 1;
        public const int MaxGlove = 1;

        int _idClothes;
        public int Temperature;
        bool _active;

        public const int MaxArmor = 6;
        public static Texture2D[] Shirt  = new Texture2D[MaxShirt];
        public static Texture2D[] ShirtLeft = new Texture2D[MaxShirt];
        public static Texture2D[] Shoes   = new Texture2D[MaxShoes];
        public static Texture2D[] Pants  = new Texture2D[MaxPants];
        public static Texture2D[] Suit  = new Texture2D[MaxCostume];
        public static Texture2D[] Gloves = new Texture2D[MaxGlove];
        public static Texture2D[] GlovesLeft = new Texture2D[MaxGlove];
        public static Texture2D[] Belt  = new Texture2D[MaxBelt];
        public static Texture2D[] Hair = new Texture2D[MaxHair];
        public static Texture2D[] Armor = new Texture2D[MaxArmor];

        public Clothes()
        {
            _active = false;
            Temperature = 0;
        }

        public Clothes(int id,int temperature = 0)
        {
            _idClothes = id;
            _active = true;
            Temperature = temperature;
        }
        public void Load(BinaryReader reader)
        {
            try
            {
                _idClothes = reader.ReadInt32();
                _active = reader.ReadBoolean();
                Temperature = reader.ReadInt32();
            }
            catch ( EndOfStreamException endOfStreamException )
            {
                WriteLine(endOfStreamException.Message);
            }
            catch ( IOException ioException ) {
                WriteLine(ioException.Message);
            }
        }
        public void Save(BinaryWriter writer){
            writer.Write(_idClothes);
            writer.Write(_active);
            writer.Write(Temperature);
        }
        public int GetId() => _active ? _idClothes : -1;

        public static void Loadclothes(ContentManager content )
        {
            Program.StartSw();
            for (int i = 0; i < Pants.Length; i++)
                Pants[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\pants_{i}");

            for (int i = 0; i < Suit.Length; i++)
                Suit[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\suit_{i}");

            for (int i = 0; i < Shirt.Length; i++)
                Shirt[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\shirt_{i}");

            for (int i = 0; i < Belt.Length; i++)
                Belt[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\belt_{i}");

            for (int i = 0; i < ShirtLeft.Length; i++)
                ShirtLeft[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\shirtl_{i}");
        
            for (int i = 0; i < Shoes.Length; i++)
                Shoes[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\shoes_{i}");

            for (int i = 0; i < Gloves.Length; i++)
                Gloves[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\glove_{i}");

            for (int i = 0; i < GlovesLeft.Length; i++)
                GlovesLeft[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\glovel_{i}");

            for (int i = 0; i < Armor.Length; i++)
                Armor[i] = content.Load<Texture2D>($"{Game1.ImageFolder}armor\\{i}");

            for (int i = 0; i < Hair.Length; i++)
                Hair[i] = content.Load<Texture2D>($"{Game1.ImageFolder}clothes\\hair_{i}");

            Program.StopSw("Loaded clothes");
        }
        public void RenderLeft(SpriteBatch spriteBatch, Rectangle rect, Rectangle src, Color colors, int type, SpriteEffects effect)
        {
            if (GetId() <= -1) return;

            switch ( type ) {
                case 1:
                    spriteBatch.Draw(ShirtLeft[_idClothes], rect, src, colors, 0, Vector2.Zero, effect, 0);
                    break;
                case 5:
                    spriteBatch.Draw(GlovesLeft[_idClothes], rect, src, colors, 0, Vector2.Zero, effect, 0);
                    break;
            }
        }
        public void Render(SpriteBatch spriteBatch,Rectangle rect,Rectangle src,Color colors,int type,SpriteEffects effect)
        {
            if (GetId() <= -1) return;

            switch ( type ) {
                case 0:
                    if (_idClothes < MaxHair) spriteBatch.Draw(Hair[_idClothes], rect, src, colors, 0, Vector2.Zero, effect, 0);
                    break;
                case 1:
                    if (_idClothes < MaxShirt) spriteBatch.Draw(Shirt[_idClothes], rect, src, colors, 0, Vector2.Zero, effect, 0);
                    break;
                case 2:
                    if (_idClothes < MaxPants) spriteBatch.Draw(Pants[_idClothes], rect, src, colors, 0, Vector2.Zero, effect, 0);
                    break;
                case 3:
                    if (_idClothes < MaxShoes) spriteBatch.Draw(Shoes[_idClothes], rect, src, colors, 0, Vector2.Zero, effect, 0);
                    break;
                case 4:
                    if (_idClothes < MaxBelt) spriteBatch.Draw(Belt[_idClothes], rect, src, colors, 0, Vector2.Zero, effect, 0);
                    break;
                case 5:
                    if (_idClothes < MaxGlove) spriteBatch.Draw(Gloves[_idClothes], rect, src, colors, 0, Vector2.Zero, effect, 0);
                    break;
            }
        }
    }
}
