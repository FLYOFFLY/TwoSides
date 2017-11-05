using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace TwoSides
{
    public static class GameInput
    {
        public static int MoveLeft, MoveRight, Jump, ActiveIventory, Drop;
        static readonly bool[] MouseIsDown = new bool[2];
        static bool[] _mouseOldIsDown = new bool[2];
        public static List<Bind> BindKey = new List<Bind>();
        public static void UpdateMouse(MouseState mstate)
        {
            _mouseOldIsDown = MouseIsDown;
            MouseIsDown[0]  = mstate.LeftButton == ButtonState.Pressed;
            MouseIsDown[1] = mstate.RightButton == ButtonState.Pressed;
        }
        public enum MouseButton
        {
            LEFT_BUTTON = 0,
            RIGHT_BUTTON = 1
        }

        public static bool MouseButtonIsPressed(int mouse) => MouseIsDown[mouse] && _mouseOldIsDown[mouse];

        public static bool MouseButtonIsDown(int mouse) => MouseIsDown[mouse] && !_mouseOldIsDown[mouse];

        public static bool MouseButtonIsReleased(int mouse) => !MouseIsDown[mouse] && _mouseOldIsDown[mouse];
    }
}
