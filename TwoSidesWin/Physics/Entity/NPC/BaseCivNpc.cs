
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GameContent.GenerationResources;
using TwoSides.Utils;
using TwoSides.World;
using TwoSides.World.Tile;

namespace TwoSides.Physics.Entity.NPC
{
    public class BaseCivNpc : BaseNpc
    {

        public int ItemAnimation;
       
        public Color[] Colors = new Color[5];
        public Clothes[] ClothesSkin = new Clothes[6];

        protected Race Race;

        public BaseCivNpc() 
        {
            for (int i = 0; i < 6; i++)
            {
                ClothesSkin[i] = new Clothes();
            }
            Race = Race.Racelist[0];
        }
       
        public override void Load(BinaryReader reader)
        {
            base.Load(reader);
            for (int i = 0; i < 6; i++)
            {
                ClothesSkin[i].Load(reader);
            }
            for (int i = 0; i < 5; i++)
            {
                Colors[i] = Tools.ReadColor(reader);
            }
            Race = new Race(new Color(0, 0, 0, 0), null);
            if (reader.ReadBoolean())
            {
                Race.Load(reader);
            }
        }
        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            for (int i = 0; i < 6; i++)
            {
                ClothesSkin[i].Save(writer);
            }
            for (int i = 0; i < 5; i++)
            {
                Tools.SaveColor(Colors[i],writer);
            }
            writer.Write(Race != null);
            Race?.Save(writer);
        }
        public BaseCivNpc(int blockx)
        {
            if (blockx <= 0) blockx = 0;
            else if (blockx > SizeGeneratior.WorldWidth) blockx = SizeGeneratior.WorldWidth - 1;
            Position.X = blockx * Tile.TileMaxSize;
            Position.Y = Program.Game.Dimension[Program.Game.CurrentDimension].MapHeight[blockx] * Tile.TileMaxSize - Tile.TileMaxSize * 3;
            Type = Program.Game.Rand.Next(2);
            Rect.Width = Width;

            WayPoint = Position.X;
            WayPoint += Program.Game.Rand.Next(-1, 1) * Tile.TileMaxSize;
            Rect.Height = Height;
            Drop.Clear();
            Drop.Add(new Item(1, 7));
            for (int i = 0; i < 6; i++)
            {
                ClothesSkin[i] = new Clothes();
            }
            Race = Race.Racelist[0];
        }

        public BaseCivNpc(Vector2 positions)
        {
            Position = positions;
            WayPoint = Position.X;
            WayPoint += Program.Game.Rand.Next(-Tile.TileMaxSize, Tile.TileMaxSize);
            Rect.Width = Width;
            Rect.Height = Height;
            Drop.Clear();
            Drop.Add(new Item(1, 7));
            for (int i = 0; i < 6; i++)
            {
                ClothesSkin[i] = new Clothes();
            }
            Race = Race.Racelist[0];
        }

        public BaseCivNpc(int blockx, Race race, IReadOnlyList<Clothes> clslot, Color[] color)
        {
            Position.X = blockx * Tile.TileMaxSize;
            if (blockx <= 0) blockx = 0;
            else if (blockx >= SizeGeneratior.WorldWidth) blockx = SizeGeneratior.WorldWidth - 2;
            Position.Y = Program.Game.Dimension[Program.Game.CurrentDimension].MapHeight[blockx] * Tile.TileMaxSize - Tile.TileMaxSize * 3;
            Type = Program.Game.Rand.Next(2);
            Rect.Width = Width;

            WayPoint = Position.X;
            for (int i = 0; i < 6; i++)
            {
                ClothesSkin[i] = clslot[i];
            }
            Colors = color;
            WayPoint += Program.Game.Rand.Next(-1, 1) * Tile.TileMaxSize;
            Rect.Height = Height;
            Drop.Clear();
            Drop.Add(new Item(1, 7));
            Race = race;
        }

        public BaseCivNpc(Vector2 positions, Race race, IReadOnlyList<Clothes> clslot, Color[] color)
        {
            Position = positions;
            WayPoint = Position.X;
            Colors = color;
            Race = race;
            WayPoint += Program.Game.Rand.Next(-Tile.TileMaxSize, Tile.TileMaxSize);
            Rect.Width = Width;
            Rect.Height = Height;
            Drop.Clear();
            Drop.Add(new Item(1, 7));
            for (int i = 0; i < 6; i++)
            {
                ClothesSkin[i] = clslot[i];
            }
        }
        public virtual void RenderNpc(SpriteBatch spriteBatch, SpriteFont font1, Texture2D head, Texture2D head2,
            Texture2D body, Texture2D legs, Texture2D eye,Texture2D hand, Texture2D shadow)
        {

            SpriteEffects effect = SpriteEffects.None;
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.DrawString(font1, ((int)Hp).ToString(CultureInfo.CurrentCulture), new Vector2((int)(Position.X + (Width - head.Width)), (int)Position.Y - 30), Color.Black);
            Rect = new Rectangle((int)(Position.X + (Width - head.Width)),
                    (int)Position.Y, head.Width, head.Height);
            Rectangle src = new Rectangle(0, 0, head.Width, head.Height);
            RenderLeft(hand, spriteBatch);
            spriteBatch.Draw(head, Rect, src, Race.GetZombieColor(),
                    0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(eye, Rect, src, Color.White,
                    0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(body, Rect, src,
               Race.GetColor(), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(legs,
               Rect, src,
                    Race.GetColor(), 0, Vector2.Zero, effect, 0);
            ClothesRender(effect, spriteBatch, Rect, src);
            DrawShadow(shadow, spriteBatch);
        }
        public void RenderLeft(Texture2D hand, SpriteBatch sb, float scale = 1.0f)
        {
            SpriteEffects effect = SpriteEffects.None;
            Rectangle dest = new Rectangle(
                (int)(Position.X + (Width - hand.Width)),
                (int)Position.Y, (int)(hand.Width * scale), (int)(hand.Height * scale));
            Rectangle src = new Rectangle(0, 0,
                24, 42);
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            sb.Draw(hand, dest, src, Race.GetColor(), 0, Vector2.Zero, effect, 0);
            ClothesSkin[1].RenderLeft(sb, dest, src, Colors[0], 1, effect);
            ClothesSkin[5].RenderLeft(sb, dest, src, Colors[4], 5, effect);
        }

        protected void ClothesRender(SpriteEffects effect, SpriteBatch spriteBatch, Rectangle rect, Rectangle src)
        {
            ClothesSkin[0].Render(spriteBatch, rect, src, Color.Black, 0, effect);
            ClothesSkin[1].Render(spriteBatch, rect, src, Colors[0], 1, effect);
            ClothesSkin[2].Render(spriteBatch, rect, src, Colors[1], 2, effect);
            ClothesSkin[3].Render(spriteBatch, rect, src, Colors[2], 3, effect);
            ClothesSkin[4].Render(spriteBatch, rect, src, Colors[3], 4, effect);
            ClothesSkin[5].Render(spriteBatch, rect, src, Colors[4], 5, effect);
        }


    }
}
