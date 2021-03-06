﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.World.Generation;

namespace TwoSides.GameContent.Tiles
{
    public interface ITileList
    {
        void Loadtiles(ContentManager content);
        void RenderPlasters(BaseDimension dimension, Rectangle rect, Render render);
        void RenderTiles(BaseDimension dimension, Rectangle rect, Render render);
        void RenderWall(BaseDimension dimension, Rectangle rect, Render render);
    }
}