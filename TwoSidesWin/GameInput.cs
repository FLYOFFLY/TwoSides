using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TwoSides
{
    public class GameInput
    {
        public static int MoveLeft, MoveRight, Jump, ActiveIventory, Drop;
        static bool[] mIsDown = new bool[2];
        static bool[] moldIsDown = new bool[2];
        public static List<Bind> BindKey = new List<Bind>();
        public static void updateMouse(MouseState mstate)
        {
            moldIsDown = mIsDown;
            mIsDown[0]  = mstate.LeftButton == ButtonState.Pressed;
            mIsDown[1] = mstate.RightButton == ButtonState.Pressed;
        }
        public enum MouseButton
        {
            LeftButton = 0,
            RightButton = 1,
        };
        public static bool MouseButtonIsPressed(int mouse)
        {
            return mIsDown[mouse] && moldIsDown[mouse];
        }
        public static bool MouseButtonIsDown(int mouse)
        {
            return mIsDown[mouse] && !moldIsDown[mouse];
        }
        public static bool MouseButtonIsReleased(int mouse)
        {
            return !mIsDown[mouse] && moldIsDown[mouse];
        }
    }
}
