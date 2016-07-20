using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoSides.World;
using TwoSides.World.Tile;
using TwoSides.Physics.Entity;

namespace TwoSides.GameContent.Tiles
{
    public class GrassBlock : BaseTile
    {
        protected int adjTexture;
        public GrassBlock(float maxHP,int adjtexture,int id) : base(maxHP,id)
        {
            this.adjTexture = adjtexture;
        }
        public override Rectangle getBoxRect(int x,int y,ITile title)
        {
            return new Rectangle(0, 0, 16,16);
        }
        public override bool blockuse(int x, int y, World.Generation.BaseDimension dimension, Physics.Entity.CEntity entity)
        {
            if (entity is Player)
            {
                Player plr = ((Player)entity);
                if (plr.slot[plr.selectedItem].iditem == 50)
                {
                    if (!dimension.map[x, y - 1].active)
                    {
                        dimension.settexture(x, y - 1, 32);
                        dimension.addUpdate(x, y - 1);
                        plr.slot[plr.selectedItem].ammount--;
                        if (plr.slot[plr.selectedItem].ammount <= 0)
                        {
                            plr.slot[plr.selectedItem] = new Item();
                        }
                    }
                }
            }
            return false;
        }
        public override int getSoildType()
        {
            return 1;
        }
        public override int getIDSideTexture()
        {
            return adjTexture;
        }
        //
    }
}
