
        using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.World;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Tile;
using System.IO;
using TwoSides.Utils;
namespace TwoSides.Physics.Entity.NPC
{
    public class BaseCivNpc : BaseNpc
    {

        public int itemAnimation;
       
        public Color[] colors = new Color[5];
        public Clothes[] cl = new Clothes[6];

        protected Race race;

        public BaseCivNpc() 
        {
            for (int i = 0; i < 6; i++)
            {
                this.cl[i] = new Clothes();
            }
        }
        public override void load(BinaryReader reader)
        {
            base.load(reader);
            for (int i = 0; i < 6; i++)
            {
                cl[i].load(reader);
            }
            for (int i = 0; i < 5; i++)
            {
                colors[i] = Util.readColor(reader);
            }
            race = new Race(new Color(0, 0, 0, 0), null);
            if (reader.ReadBoolean())
            {
                race.Load(reader);
            }
        }
        public override void save(BinaryWriter writer)
        {
            base.save(writer);
            for (int i = 0; i < 6; i++)
            {
                cl[i].save(writer);
            }
            for (int i = 0; i < 5; i++)
            {
                Util.SaveColor(colors[i],writer);
            }
            writer.Write(this.race != null);
            if (this.race != null)
            {
                race.Save(writer);
            }
        }
        public BaseCivNpc(int blockx)
        {
            position.X = blockx * ITile.TileMaxSize;
            position.Y = Program.game.dimension[Program.game.currentD].mapHeight[blockx] * ITile.TileMaxSize - ITile.TileMaxSize * 3;
            type = Program.game.rand.Next(2);
            rect.Width = width;

            to = position.X;
            to += Program.game.rand.Next(-1, 1) * ITile.TileMaxSize;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1, 7));
            for (int i = 0; i < 6; i++)
            {
                cl[i] = new Clothes();
            }
        }

        public BaseCivNpc(Vector2 positions)
        {
            position = positions;
            to = position.X;
            to += Program.game.rand.Next(-ITile.TileMaxSize, ITile.TileMaxSize);
            rect.Width = width;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1, 7));
            for (int i = 0; i < 6; i++)
            {
                cl[i] = new Clothes();
            }
        }

        public BaseCivNpc(int blockx, Race race, Clothes[] clslot, Color[] color)
        {
            position.X = blockx * ITile.TileMaxSize;
            if (blockx <= 0) blockx = 0;
            else if (blockx >= SizeGeneratior.WorldWidth) blockx = SizeGeneratior.WorldWidth - 2;
            position.Y = Program.game.dimension[Program.game.currentD].mapHeight[blockx] * ITile.TileMaxSize - ITile.TileMaxSize * 3;
            type = Program.game.rand.Next(2);
            rect.Width = width;

            to = position.X;
            for (int i = 0; i < 6; i++)
            {
                this.cl[i] = clslot[i];
            }
            this.colors = color;
            to += Program.game.rand.Next(-1, 1) * ITile.TileMaxSize;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1, 7));
            this.race = race;
        }

        public BaseCivNpc(Vector2 positions, Race race, Clothes[] clslot, Color[] color)
        {
            position = positions;
            to = position.X;
            this.colors = color;
            this.race = race;
            to += Program.game.rand.Next(-ITile.TileMaxSize, ITile.TileMaxSize);
            rect.Width = width;
            rect.Height = height;
            drop.Clear();
            drop.Add(new Item(1, 7));
            for (int i = 0; i < 6; i++)
            {
                this.cl[i] = clslot[i];
            }
        }
        public virtual void DrawNPC(SpriteEffects effect, SpriteBatch spriteBatch, SpriteFont Font1, Texture2D head, Texture2D head2,
            Texture2D body, Texture2D legs, Texture2D eye, Texture2D shadow)
        {
            effect = SpriteEffects.None;
            if (direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.DrawString(Font1, ((int)(hp)).ToString(), new Vector2((int)(position.X + (width - head.Width)), (int)(position.Y) - 30), Color.Black);
            int hairid = cl[0].getid();
            int shirtid = cl[1].getid();
            int pantsid = cl[2].getid();
            int shoesid = cl[3].getid();
            int beltid = cl[4].getid();
            int glovesid = cl[5].getid();
            Rectangle rect = new Rectangle((int)(position.X + (width - head.Width)),
                    (int)(position.Y), head.Width, head.Height);
            Rectangle src = new Rectangle(0, 0, head.Width, head.Height);
            spriteBatch.Draw(head, rect, src, race.getZombieColor(),
                    0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(eye, rect, src, Color.White,
                    0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(body, rect, src,
               race.getColor(), 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(legs,
               rect, src,
                    race.getColor(), 0, Vector2.Zero, effect, 0);
            if (hairid != -1) spriteBatch.Draw(Clothes.hair[hairid], rect, src, Color.Black, 0, Vector2.Zero, effect, 0);
            if (shirtid != -1) spriteBatch.Draw(Clothes.shirt[shirtid], rect, src, colors[0], 0, Vector2.Zero, effect, 0);
            if (pantsid != -1) spriteBatch.Draw(Clothes.pants[pantsid], rect, src, colors[1], 0, Vector2.Zero, effect, 0);
            if (shoesid != -1) spriteBatch.Draw(Clothes.shoes[shoesid], rect, src, colors[2], 0, Vector2.Zero, effect, 0);
            if (beltid != -1) spriteBatch.Draw(Clothes.belt[beltid], rect, src, colors[3], 0, Vector2.Zero, effect, 0);
            if (glovesid != -1) spriteBatch.Draw(Clothes.gloves[glovesid], rect, src, colors[4], 0, Vector2.Zero, effect, 0);
            DrawShadow(shadow, spriteBatch);
        }

       

        public override void update()
        {
            base.update();
        }
    }
}
