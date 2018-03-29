using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GUI;
using TwoSides.Physics.Entity.NPC;
using TwoSides.World;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Entity.NPC
{
    [Serializable]
    public sealed class Civilian :BaseCivNpc
    {
        bool _enabled ;
        public int Activedialog;

        public List<Dialog> Dialog;
        public override void Load(BinaryReader reader)
        {
            base.Load(reader);
            Activedialog = reader.ReadInt32();
        }
        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            writer.Write(Activedialog);
        }
        public Civilian(Vector2 positions)
        {
            Position = positions;
            WayPoint = Position.X;

            WayPoint += Program.Game.Rand.Next(-Tile.TILE_MAX_SIZE, Tile.TILE_MAX_SIZE);
            Rect.Width = Width;
            Rect.Height = Height;
            Drop.Clear();
            Drop.Add(new Item(1, 7));
            Activedialog = 0;
            Dialog = null;
        }

        public Civilian(Vector2 positions, List<Dialog> dialog)
        {
            Position = positions;
            WayPoint = Position.X;

            WayPoint += Program.Game.Rand.Next(-Tile.TILE_MAX_SIZE, Tile.TILE_MAX_SIZE);
            Rect.Width = Width;
            Rect.Height = Height;
            Drop.Clear();
            Drop.Add(new Item(1, 7));
            Dialog = dialog;
            Activedialog = 0;
        }

        public void ToggleEnabled(){
            _enabled = !_enabled;
        }

        public void Off()
        {
            _enabled = false;
        }

        public void RenderNpc(Render render, SpriteFont font1, Texture2D head, Texture2D body,
            Texture2D legs, Texture2D hand, Texture2D shadow)
        {

            SpriteEffects effect = SpriteEffects.None;
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
           if(Activedialog ==0)
               render.DrawString(font1, "Press RKM on NPC", new Vector2((int)(Position.X + (Width - head.Width)), (int)Position.Y - 30));
            render.Draw(hand, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), effect, ColorScheme.BaseRace);
            render.Draw(Clothes.ShirtLeft[1], new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), effect, ColorScheme.BloodColor);

            render.Draw(head, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), effect, ColorScheme.BaseRace);
            render.Draw(body, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), effect, ColorScheme.BaseRace);
            render.Draw(legs, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), effect, ColorScheme.BaseRace);
            render.Draw(Clothes.Shirt[1], new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height),effect,ColorScheme.BloodColor);
            render.Draw(Clothes.Pants[0], new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height),effect,ColorScheme.BloodColor);
            render.Draw(Clothes.Shoes[0], new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height),effect,ColorScheme.BloodColor);
            DrawShadow(shadow, render);
        }

        public void RenderDialog(Render render)
        {
            if ( !_enabled || Dialog == null ) return;

            render.End();
            Dialog[Activedialog].Render(render);
            render.Start();
        }
        public void ToggleVisible(int btn)
        {
            Dialog[Activedialog].ChangeVisible(btn);
        }

        public void ToggleVisible(int btn,bool newChange)
        {
            if ( Dialog[Activedialog].IsVisible(btn)!=newChange) Dialog[Activedialog].ChangeVisible(btn, newChange);
        }
        public override void Update()
        {
            if (_enabled)
            {
                Dialog?[Activedialog].Update();
            }
            HorisontalMove();
            VerticalMove();
            Move();
        }
    }
}
