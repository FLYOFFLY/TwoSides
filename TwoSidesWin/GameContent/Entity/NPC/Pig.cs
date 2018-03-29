using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.Physics.Entity.NPC;
using TwoSides.World;

namespace TwoSides.GameContent.Entity.NPC
{
    public class Pig : BaseNpc
    {

        float _frame;
       // int frame;
        public Pig(Texture2D npcSkin) : base(new[]{npcSkin})
        {
            Width = 36;
            Height = 20;
        }

        public Pig(int blockx, Texture2D npcSkin)
            : base(blockx, new[] { npcSkin })
        {
            WayPoint = Position.X;
            Width = 36;
            Height = 20;
            Drop.Clear();
            Drop.Add(new Item(1, 54));
        }
        public override void Update()
        {
            base.Update();
            if ((int)_frame < 3) _frame += 1 / 24.0f;
            else _frame = 0;
        }
        public override void RenderNpc( Render render, SpriteFont font, Texture2D shadow)
        {

            SpriteEffects effect = SpriteEffects.None;
            if (Direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            render.DrawString(font, ((int)Hp).ToString(CultureInfo.CurrentCulture), new Vector2((int)(Position.X + (Width - NpcSkin[0].Width)), (int)Position.Y - 30), Color.Black);

            Rectangle src = new Rectangle(0, 0, Width,Height);
            src.Y += (int)_frame * src.Height;
            Rect = new Rectangle((int)(Position.X + (Width - NpcSkin[0].Width)),
                    (int)Position.Y, src.Width, src.Height);
            foreach ( Texture2D skin in NpcSkin )
            {
                render.Draw(skin, Rect, src, effect);
            }
            DrawShadow(shadow, render);
        }
        
    }
}
