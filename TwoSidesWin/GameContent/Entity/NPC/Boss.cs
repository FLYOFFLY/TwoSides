using System;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.World;

namespace TwoSides.GameContent.Entity.NPC
{
    [Serializable]
    public sealed class Boss:Zombie
    {
        public void SetSetting()
        {
            Hp = 10;

            Type = 5;
            MaxRunSpeed = 1.0f;
            RunAcceleration = 0.16f;
            RunSlowdown = 0.1f;
            JumpHeight = 30;
            JumpSpeed = 10.01f;
        }
        public Boss(int blockx)
            : base(blockx)
        {
            SetSetting();
        }
        public Boss(Vector2 positions)
            : base(positions)
        {
            SetSetting();
        }

         public override void RenderNpc(Render render, SpriteFont font1, Texture2D head, Texture2D head2,
              Texture2D body, Texture2D legs, Texture2D eye, Texture2D hand, Texture2D shadow)
        {

            SpriteEffects effect = SpriteEffects.None;
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            render.DrawString(font1, ((int)Hp).ToString(CultureInfo.CurrentCulture), new Vector2((int)(Position.X + (Width - head.Width)), (int)Position.Y - 30), Color.Black);
            render.Draw(head2, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height),Color.White,effect);
            render.Draw(body, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75), effect);
            render.Draw(legs, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75), effect);
            render.Draw(Clothes.Shirt[1], new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red,effect);
            render.Draw(Clothes.Pants[0], new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red,effect);
            render.Draw(Clothes.Shoes[0], new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.Red,effect);
            render.Draw(eye, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75),effect);
            render.Draw(hand, new Rectangle((int)(Position.X + (Width - head.Width)), (int)Position.Y, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), new Color(136, 105, 75),effect);
            DrawShadow(shadow, render);
        }
        public override void Kill() 
        {
            foreach (Item slots in Drop)
            {
                Program.Game.AddDrop((int)Position.X, (int)Position.Y, slots);
            }
            Program.Game.Player.BossKill();
        }

    }
}
