using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwoSides.World;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Tile;
using System.IO;
namespace TwoSides.Utils
{
    sealed public class Util
    {
        public static Vector2 dir(Point point1, Point point2)
        {
            Vector2 one = point1.ToVector2();
            Vector2 two = point2.ToVector2();
            one-=two;
            one.Normalize();
            return one;
        }
        public static Boolean directional(int from, int to, int radius)
        {
            return Math.Abs(from - to) < radius;
        }

        public static object[] directional(int x, int y,int mx,int my)
        {
            y += 16;
            object[] arr = new object[2];
            arr[0] = mx - x;
            arr[1] = my - y;
            return arr;
        }

        public static object[] directional(int x,int y)
        {
            int mx = Program.game.mouseState.X + (int)Program.game.camera.pos.X;
            int my = Program.game.mouseState.Y + (int)Program.game.camera.pos.Y;
            return directional(x,y,mx,my);
        }

        public static Vector2 directional(Vector2 pos)
        {
            int mx = Program.game.mouseState.X + (int)Program.game.camera.pos.X;
            int my = Program.game.mouseState.Y + (int)Program.game.camera.pos.Y;
            pos.Y += 16;
            return new Vector2(mx, my) - pos;
        }

        public static double angletomouse(int x, int y)
        {

            int mx = Program.game.mouseState.X + (int)Program.game.camera.pos.X;
            int my = Program.game.mouseState.Y + (int)Program.game.camera.pos.Y;
            double A = Math.Atan2(my - y, x - mx);
            //A = (A < 0) ? A + 360 : A;   //Без этого диапазон от 0...180 и -1...-180
            return A;
        }

        public static Vector2 getcube(int x, int y)
        {
            int n = x / ITile.TileMaxSize;
            int n2 = y / ITile.TileMaxSize;
            if (n < 0) n = 0;
            else if (n >= SizeGeneratior.WorldWidth) n = SizeGeneratior.WorldWidth - 1;
            if (n2 < 0) n2 = 0;
            else if (n2 >= SizeGeneratior.WorldHeight) n = SizeGeneratior.WorldHeight - 1;
            return new Vector2(n, n2);
        }
        public static bool InCube(Rectangle obj1, Rectangle obj2)
        {
            return obj1.Intersects(obj2);
        }

        public static bool MouseInCube(int x, int y, int w, int h)
        {
            int mx = Program.game.mouseState.X;
            int my = Program.game.mouseState.Y;
            return mx >= x && mx <= x + w &&
               my >= y && my <= y + h;
        }

        public static bool ScMouseInCube(int x, int y, int w, int h)
        {
            Vector2 mouse = Program.game.mouseState.Position.ToVector2();
            mouse = Vector2.Transform(mouse,Program.game.camera.getInverse());
            return mouse.X >= x && mouse.X <= x + w &&
               mouse.Y >= y && mouse.Y <= y + h;
        }

        public static Vector2 cameraMouse(Vector2 mousePos)
        {
            mousePos = Vector2.Transform(mousePos, Program.game.camera.getInverse());
            return mousePos;
        }

        public static Boolean inradious(int x1, int y1, int Xc, int Yc, int radius, bool outline)
        {
            if (((x1 - Xc) * (x1 - Xc) + (y1 - Yc) * (y1 - Yc)) < radius * radius)
            {
                return true;
            }
            else if (((x1 - Xc) * (x1 - Xc) + (y1 - Yc) * (y1 - Yc)) == radius * radius) { return outline; }
            else
                return false;
        }


        public static Color readColor(BinaryReader reader)
        {
            return new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }

        public static void SaveColor(Color color,BinaryWriter writer)
        {
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
            writer.Write(color.A);
        }
    }
}
