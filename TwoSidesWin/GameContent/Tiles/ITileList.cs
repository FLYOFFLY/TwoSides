using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TwoSIdes.World.Generation;

namespace TwoSIdes.GameContent.Tiles
{
    public interface ITileList
    {
        void Loadtiles(ContentManager content);
        void RenderPlasters(BaseDimension dimension, Rectangle rect, SpriteBatch spriteBatch);
        void RenderTiles(BaseDimension dimension, Rectangle rect, SpriteBatch spriteBatch);
        void RenderWall(BaseDimension dimension, Rectangle rect, SpriteBatch spriteBatch);
    }
}