using Microsoft.Xna.Framework;

using TwoSides.GameContent.Entity;
using TwoSides.Physics.Entity;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Tiles
{
    public class GrassBlock : BaseTile
    {
        protected int AdjTexture;
        public GrassBlock(float maxHp,int adjtexture,int id) : base(maxHp,id) => AdjTexture = adjtexture;

        public override Rectangle GetBoxRect(int x,int y,Tile title) => new Rectangle(0, 0, 16,16);

        public override bool UseBlock(int x, int y, BaseDimension dimension, DynamicEntity entity)
        {
            // ReSharper disable once InvertIf
            if (entity is Player plr)
            {
                if ( plr.Slot[plr.SelectedItem].Id != 50 ) return false;

                if ( dimension.MapTile[x , y - 1].Active ) return false;

                dimension.SetTexture(x, y - 1, 32);
                dimension.AddUpdateTile(x, y - 1);
                plr.Slot[plr.SelectedItem].Ammount--;
                if (plr.Slot[plr.SelectedItem].Ammount <= 0)
                {
                    plr.Slot[plr.SelectedItem] = new Item();
                }
            }
            return false;
        }
        public override int GetIdSideTexture() => AdjTexture;

        //
    }
}
