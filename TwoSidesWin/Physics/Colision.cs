using Microsoft.Xna.Framework;

using TwoSides.GameContent.GenerationResources;
using TwoSides.Physics.Entity;
using TwoSides.World.Tile;

namespace TwoSides.Physics
{
    public static  class Colision
    {

        public static Vector2 TileCollision(DynamicEntity entity, Vector2 position, Vector2 velocity, int widthEntity, int heightEntity,bool isDown)
        {
            Vector2 result = velocity;
            Vector2 vector = velocity;
            Vector2 finalPos = position + velocity;
            Vector2 startPos = position;
           var startX = (int)(position.X / Tile.TILE_MAX_SIZE) - 1;
            var endX = (int)((position.X + widthEntity) / Tile.TILE_MAX_SIZE) + 2;
            var startY = (int)(position.Y / Tile.TILE_MAX_SIZE) - 1;
            var endY = (int)((position.Y + heightEntity) / Tile.TILE_MAX_SIZE) + 2;
            var leftTileX = -1;
            var leftTileY = -1;
            var rightTileX = -1;
            var righTileY = -1;
            if (startX < 0)
            {
                startX = 0;
            }
            if (endX > SizeGeneratior.WorldWidth)
            {
                endX = SizeGeneratior.WorldWidth;
            }
            if (startY < 0)
            {
                startY = 0;
            }
            if (endY > SizeGeneratior.WorldHeight)
            {
                endY = SizeGeneratior.WorldHeight;
            }
            for (var tileX = startX; tileX < endX; tileX++)
            {
                for (var tileY = startY; tileY < endY; tileY++)
                {
                    if ( !Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[tileX , tileY].Active )
                        continue;

                    Rectangle aabb = Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[tileX, tileY].GetBoxRect(tileX,tileY);
                    Vector2 tilePos;
                    tilePos.X = (float)(tileX * 16)+aabb.X;
                    tilePos.Y = (float)(tileY * 16)+aabb.Y;
                    if ( !(finalPos.X + widthEntity > tilePos.X) || !(finalPos.X < tilePos.X + aabb.Width) ||
                         !(finalPos.Y + heightEntity > tilePos.Y) ||
                         !(finalPos.Y < tilePos.Y + aabb.Height) ) continue;

                    if (Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[tileX, tileY].IsSolid())
                    {
                        var soildType = Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[tileX, tileY].GetSoildType() ;
                        switch ( soildType ) {
                            case 1:
                                ColSquare(aabb, widthEntity, heightEntity, ref result, ref vector, ref startPos,
                                          ref leftTileX, ref leftTileY, ref rightTileX, ref righTileY, tileX, tileY, tilePos);
                                break;
                            case 2:
                                ColPlatform(heightEntity, ref result, ref startPos, leftTileX, velocity, ref rightTileX, ref righTileY, tileX, tileY, tilePos, isDown);
                                break;
                        }
                    }
                    else {
                        Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[tileX, tileY].InTile(entity);
                    }
                }
            }

            return result;
        }

        static void ColSquare(Rectangle boxColision,int widthEntity, int heightEntity, ref Vector2 result, ref Vector2 vector,
            ref Vector2 startPos, ref int leftTileX,ref  int leftTileY, ref int rightTileX, ref int righTileY, int tileX, int tileY, Vector2 tilePos)
        {
            if (startPos.X + widthEntity <= tilePos.X)
            {
                leftTileX = tileX;
                leftTileY = tileY;
                if (leftTileY != righTileY)
                {
                    result.X = tilePos.X - (startPos.X + widthEntity);
                }
                if (rightTileX == leftTileX)
                {
                    result.Y = vector.Y;
                }
            }
            else if (startPos.Y + heightEntity <= tilePos.Y)
            {
                rightTileX = tileX;
                righTileY = tileY;
                if (rightTileX != leftTileX)
                {
                    result.Y = tilePos.Y - (startPos.Y + heightEntity);
                }
            }
            else if (startPos.X >= tilePos.X + boxColision.Width)
            {
                leftTileX = tileX;
                leftTileY = tileY;
                if (leftTileY != righTileY)
                {
                    result.X = tilePos.X + boxColision.Width - startPos.X;
                }
                if (rightTileX == leftTileX)
                {
                    result.Y = vector.Y;
                }
            }
            else if (startPos.Y >= tilePos.Y + boxColision.Height)
            {
                rightTileX = tileX;
                righTileY = tileY;
                result.Y = tilePos.Y + boxColision.Height - startPos.Y;
                if (righTileY == leftTileY)
                {
                    result.X = vector.X + 0.01f;
                }
            }
        }

        static void ColPlatform(int heightEntity, ref Vector2 result, ref Vector2 startPos, int leftTileX, Vector2 velocity,
            ref int rightTileX, ref int rightTileY, int tileX, int tileY, Vector2 tilePos, bool isDown)
        {
            if ( !(startPos.Y + heightEntity <= tilePos.Y) ) return;

            if ( !(velocity.Y > 0f) || isDown ) return;

            rightTileX = tileX;
            rightTileY = tileY;
            if (rightTileX != leftTileX)
            {
                result.Y = tilePos.Y - (startPos.Y + heightEntity);
            }
        }
    }
}
