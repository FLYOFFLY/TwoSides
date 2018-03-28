using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GameContent.Entity;
using TwoSides.Physics.Entity;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    internal class BlockTree: BaseTile
    {
        public BlockTree(float maxHp,int id)
            : base(maxHp,id)
        {
        }
        public override bool IsSolid() => false;

        public override void Render(ITileDatecs tileDate, Render render, Texture2D texture, BaseDimension dimension, Vector2 pos, int x, int y, int frame, int subTexture, Color color)
        {
            TreeDate treeDate = (TreeDate)tileDate;
            if (dimension.MapTile[x,y].IdTexture == 16)
                render.Draw(texture, new Rectangle((int)pos.X-25, (int)pos.Y-42, 64, 64), new Rectangle(64 * frame, 64 * treeDate.TypeTree, 64, 64), color);
            else if (dimension.MapTile[x, y].IdTexture == 38)
                render.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, 16, 16), new Rectangle(16 * subTexture, 16 * treeDate.TypeTree, 16, 16), color);
            else base.Render(tileDate, render, texture, dimension, pos, x, y, frame, subTexture, color);
        }
        public override void InTile(DynamicEntity entity)
        {
            if (entity is Player player) {
                player.IsHorisontal = true;
            }
        }
        public override ITileDatecs ChangeTile() => new TreeDate();

        public override void Update(int x, int y, BaseDimension dimension,DynamicEntity entity)
        {
            if (dimension.MapTile[x, y].IdTexture == 38)
            {
                if (dimension.MapTile[x, y + 1].Active || dimension.MapTile[x, y - 1].Active) return;
            }
            else if (dimension.MapTile[x, y + 1].Active) return;
            if (entity is Player player)
            {
                player.GetDrop(x, y);
            }
        }
    }
}
