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
           int startX = (int)(position.X / Tile.TileMaxSize) - 1;
            int endX = (int)((position.X + widthEntity) / Tile.TileMaxSize) + 2;
            int startY = (int)(position.Y / Tile.TileMaxSize) - 1;
            int endY = (int)((position.Y + heightEntity) / Tile.TileMaxSize) + 2;
            int num5 = -1;
            int num6 = -1;
            int num7 = -1;
            int num8 = -1;
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
            for (int tileX = startX; tileX < endX; tileX++)
            {
                for (int tileY = startY; tileY < endY; tileY++)
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
                        int soildType = Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[tileX, tileY].GetSoildType() ;
                        switch ( soildType ) {
                            case 1:
                                ColSquare(aabb, widthEntity, heightEntity, ref result, ref vector, ref startPos,
                                          ref num5, ref num6, ref num7, ref num8, tileX, tileY, tilePos);
                                break;
                            case 2:
                                ColPlatform(heightEntity, ref result, ref startPos, num5, velocity, ref num7, ref num8, tileX, tileY, tilePos, isDown);
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
            ref Vector2 startPos, ref int num5,ref  int num6, ref int num7, ref int num8, int tileX, int tileY, Vector2 tilePos)
        {
            if (startPos.X + widthEntity <= tilePos.X)
            {
                num5 = tileX;
                num6 = tileY;
                if (num6 != num8)
                {
                    result.X = tilePos.X - (startPos.X + widthEntity);
                }
                if (num7 == num5)
                {
                    result.Y = vector.Y;
                }
            }
            else if (startPos.Y + heightEntity <= tilePos.Y)
            {
                num7 = tileX;
                num8 = tileY;
                if (num7 != num5)
                {
                    result.Y = tilePos.Y - (startPos.Y + heightEntity);
                }
            }
            else if (startPos.X >= tilePos.X + boxColision.Width)
            {
                num5 = tileX;
                num6 = tileY;
                if (num6 != num8)
                {
                    result.X = tilePos.X + boxColision.Width - startPos.X;
                }
                if (num7 == num5)
                {
                    result.Y = vector.Y;
                }
            }
            else if (startPos.Y >= tilePos.Y + boxColision.Height)
            {
                num7 = tileX;
                num8 = tileY;
                result.Y = tilePos.Y + boxColision.Height - startPos.Y;
                if (num8 == num6)
                {
                    result.X = vector.X + 0.01f;
                }
            }
        }

        static void ColPlatform(int heightEntity, ref Vector2 result, ref Vector2 startPos, int num5, Vector2 velocity,
            ref int num7, ref int num8, int tileX, int tileY, Vector2 tilePos, bool isDown)
        {
            if ( !(startPos.Y + heightEntity <= tilePos.Y) ) return;

            if ( !(velocity.Y > 0f) || isDown ) return;

            num7 = tileX;
            num8 = tileY;
            if (num7 != num5)
            {
                result.Y = tilePos.Y - (startPos.Y + heightEntity);
            }
        }
    }
}
