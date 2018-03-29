using System;
using System.IO;

using Microsoft.Xna.Framework;

using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Tile;

namespace TwoSides.Utils
{
    public static class Tools
    {
        public static Vector2 GetDir(Point point1, Point point2)
        {
            Vector2 one = point1.ToVector2();
            Vector2 two = point2.ToVector2();
            one-=two;
            one.Normalize();
            return one;
        }

        public static bool Distance(int from, int to, int radius) => Math.Abs(from - to) < radius;

        public static object[] Distance(int x, int y,int mx,int my)
        {
            y += 16;
            object[] arr = new object[2];
            arr[0] = mx - x;
            arr[1] = my - y;
            return arr;
        }

        public static object[] Distance(int x,int y)
        {
            var mx = GameInput.GetMousePos().X + (int)Program.Game.Camera.Pos.X;
            var my = GameInput.GetMousePos().Y + (int)Program.Game.Camera.Pos.Y;
            return Distance(x,y,(int)mx,(int)my);
        }

        public static Vector2 Distance(Vector2 pos)
        {
            var mx = GameInput.GetMousePos().X + (int)Program.Game.Camera.Pos.X;
            var my = GameInput.GetMousePos().Y + (int)Program.Game.Camera.Pos.Y;
            pos.Y += 16;
            return new Vector2(mx, my) - pos;
        }

        public static double AngleMouse(int x, int y)
        {

            var mx = GameInput.GetMousePos().X + (int)Program.Game.Camera.Pos.X;
            var my = GameInput.GetMousePos().Y + (int)Program.Game.Camera.Pos.Y;
            //A = (A < 0) ? A + 360 : A;   //Без этого диапазон от 0...180 и -1...-180
            return Math.Atan2(my - y, x - mx);
        }

        public static Vector2 GetTile(int x, int y)
        {
            var n = x / Tile.TILE_MAX_SIZE;
            var n2 = y / Tile.TILE_MAX_SIZE;
            if (n < 0) n = 0;
            else if (n >= SizeGeneratior.WorldWidth) n = SizeGeneratior.WorldWidth - 1;
            if (n2 < 0) n2 = 0;
            else if (n2 >= SizeGeneratior.WorldHeight) n = SizeGeneratior.WorldHeight - 1;
            return new Vector2(n, n2);
        }
        public static bool InCube(Rectangle obj1, Rectangle obj2) => obj1.Intersects(obj2);

        public static bool MouseInCube(int x, int y, int w, int h)
        {
            var mx = GameInput.GetMousePos().X;
            var my = GameInput.GetMousePos().Y;
            return mx >= x && mx <= x + w &&
               my >= y && my <= y + h;
        }

        public static bool ScMouseInCube(int x, int y, int w, int h)
        {
            Vector2 mouse =GameInput.GetMousePos();
            mouse = Vector2.Transform(mouse,Program.Game.Camera.GetInverse());
            return mouse.X >= x && mouse.X <= x + w &&
               mouse.Y >= y && mouse.Y <= y + h;
        }

        public static Vector2 MouseToCamera(Vector2 mousePos)
        {
            mousePos = Vector2.Transform(mousePos, Program.Game.Camera.GetInverse());
            return mousePos;
        }

        public static bool InRadious(int x1, int y1, int xCenter, int yCenter, int radius, bool outline)
        {
            if ((x1 - xCenter) * (x1 - xCenter) + (y1 - yCenter) * (y1 - yCenter) < radius * radius)
                return true;

            return (x1 - xCenter) * (x1 - xCenter) + (y1 - yCenter) * (y1 - yCenter) == radius * radius && outline;
        }


        public static ColorScheme ReadColor(BinaryReader reader) => new ColorScheme(new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte()));

        public static void SaveColor(ColorScheme colorScheme,BinaryWriter writer)
        {
            Color color = colorScheme.Color;
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
            writer.Write(color.A);
        }
    }
}
