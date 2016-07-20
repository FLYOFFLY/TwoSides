using Microsoft.Xna.Framework;
using System;
using TwoSides.World;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Tile;
using TwoSides.Physics.Entity;
namespace TwoSides.Physics
{
    class Colision
    {

        public static Vector2 TileCollision(CEntity entity, Vector2 Position, Vector2 Velocity, int WidthEntity, int HeightEntity,bool isDown)
        {
            Vector2 result = Velocity;
            Vector2 vector = Velocity;
            Vector2 finalPos = Position + Velocity;
            Vector2 startPos = Position;
           int startX = (int)(Position.X / ITile.TileMaxSize) - 1;
            int endX = (int)((Position.X + (float)WidthEntity) / ITile.TileMaxSize) + 2;
            int startY = (int)(Position.Y / ITile.TileMaxSize) - 1;
            int endY = (int)((Position.Y + (float)HeightEntity) / ITile.TileMaxSize) + 2;
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
            for (int TIleX = startX; TIleX < endX; TIleX++)
            {
                for (int TileY = startY; TileY < endY; TileY++)
                {

                    if (Program.game.dimension[Program.game.currentD].map[TIleX, TileY].active)
                    {
                        Rectangle AABB = Program.game.dimension[Program.game.currentD].map[TIleX, TileY].getBoxRect(TIleX,TileY);
                        Vector2 TilePos;
                        TilePos.X = (float)(TIleX * 16)+AABB.X;
                        TilePos.Y = (float)(TileY * 16)+AABB.Y;
                        if (finalPos.X + (float)WidthEntity > TilePos.X && finalPos.X < TilePos.X + AABB.Width
                            && finalPos.Y + (float)HeightEntity > TilePos.Y && finalPos.Y < TilePos.Y + AABB.Height)
                        {
                            if (Program.game.dimension[Program.game.currentD].map[TIleX, TileY].issolid())
                            {
                                int soildType = Program.game.dimension[Program.game.currentD].map[TIleX, TileY].getSoildType() ;
                                if (soildType== 1)
                                {
                                    ColSquare(AABB, WidthEntity, HeightEntity, ref result, ref vector, ref startPos,
                                        ref num5, ref num6, ref num7, ref num8, TIleX, TileY, TilePos);
                                }
                                else if(soildType == 2){
                                    ColPlatform(AABB, WidthEntity, HeightEntity, ref result, ref vector, ref startPos, num5, Velocity, ref num7, ref num8, TIleX, TileY, TilePos,isDown);
                                
                                }
                            }
                            else {
                                Program.game.dimension[Program.game.currentD].map[TIleX, TileY].InTile(entity);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static void ColSquare(Rectangle BoxColision,int WidthEntity, int HeightEntity, ref Vector2 result, ref Vector2 vector,
            ref Vector2 startPos, ref int num5,ref  int num6, ref int num7, ref int num8, int tileX, int tileY, Vector2 TilePos)
        {
            if (startPos.X + (float)WidthEntity <= TilePos.X)
            {
                num5 = tileX;
                num6 = tileY;
                if (num6 != num8)
                {
                    result.X = (TilePos.X) - (startPos.X + (float)WidthEntity);
                }
                if (num7 == num5)
                {
                    result.Y = vector.Y;
                }
            }
            else if (startPos.Y + (float)HeightEntity <= TilePos.Y)
            {
                num7 = tileX;
                num8 = tileY;
                if (num7 != num5)
                {
                    result.Y = (TilePos.Y) - (startPos.Y + (float)HeightEntity);
                }
            }
            else if (startPos.X >= TilePos.X + BoxColision.Width)
            {
                num5 = tileX;
                num6 = tileY;
                if (num6 != num8)
                {
                    result.X = TilePos.X + BoxColision.Width - startPos.X;
                }
                if (num7 == num5)
                {
                    result.Y = vector.Y;
                }
            }
            else if (startPos.Y >= TilePos.Y + BoxColision.Height)
            {
                num7 = tileX;
                num8 = tileY;
                result.Y = TilePos.Y + BoxColision.Height - startPos.Y;
                if (num8 == num6)
                {
                    result.X = vector.X + 0.01f;
                }
            }
        }
        private static void ColNaklon(Rectangle BoxColision, int WidthEntity, int HeightEntity, ref Vector2 result, ref Vector2 vector,
           ref Vector2 startPos, ref int num5, ref  int num6, ref int num7, ref int num8, int tileX, int tileY, Vector2 TilePos)
        {
            if (startPos.X + (float)WidthEntity <= TilePos.X)
            {
                num5 = tileX;
                num6 = tileY;
                if (num6 != num8)
                {
                    result.X = (TilePos.X) - (startPos.X + (float)WidthEntity);
                }
                if (num7 == num5)
                {
                    result.Y = vector.Y;
                }
            }
            else if (startPos.Y + (float)HeightEntity <= TilePos.Y)
            {
                num7 = tileX;
                num8 = tileY;
                if (num7 != num5)
                {
                    result.Y = (TilePos.Y) - (startPos.Y + (float)HeightEntity);
                }
            }
            else if (startPos.X >= TilePos.X + BoxColision.Width)
            {
                num5 = tileX;
                num6 = tileY;
                if (num6 != num8)
                {
                    result.X = TilePos.X + BoxColision.Width - startPos.X;
                }
                if (num7 == num5)
                {
                    result.Y = vector.Y;
                }
            }
            else if (startPos.Y >= TilePos.Y + BoxColision.Height)
            {
                num7 = tileX;
                num8 = tileY;
                result.Y = TilePos.Y + BoxColision.Height - startPos.Y;
                if (num8 == num6)
                {
                    result.X = vector.X + 0.01f;
                }
            }
        }
        private static void ColPlatform(Rectangle BoxColision, int WidthEntity, int HeightEntity, ref Vector2 result, ref Vector2 vector,
          ref Vector2 startPos, int num5,Vector2 velocity, ref int num7, ref int num8, int tileX, int tileY, Vector2 TilePos,bool isDown)
        {
            if (startPos.Y + (float)HeightEntity <= TilePos.Y)
            {
                if (velocity.Y > 0f && !isDown)
                {
                    num7 = tileX;
                    num8 = tileY;
                    if (num7 != num5)
                    {
                        result.Y = (TilePos.Y) - (startPos.Y + (float)HeightEntity);
                    }
                }
            }
        }
    }
}
