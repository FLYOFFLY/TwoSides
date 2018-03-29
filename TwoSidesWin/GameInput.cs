using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TwoSides
{
    public static class GameInput
    {
        public static int MoveLeft, MoveRight, Jump, ActiveIventory, Drop;
        static readonly bool[] MouseIsDown = new bool[2];
        static bool[] _mouseOldIsDown = new bool[2];
        public static List<Bind> BindKey = new List<Bind>();
        static Point _posCursor;

        public static MouseState GetState()
        {
            return Mouse.GetState();
        }

        public static void UpdateMouse()
        {
            MouseState mstate = Mouse.GetState();
            _posCursor = mstate.Position;
            _mouseOldIsDown = MouseIsDown;
            MouseIsDown[0]  = mstate.LeftButton == ButtonState.Pressed;
            MouseIsDown[1] = mstate.RightButton == ButtonState.Pressed;
        }
        public enum MouseButton : int
        {
            LEFT_BUTTON = 0,
            RIGHT_BUTTON = 1
        }

        public static Vector2 GetMousePos() => _posCursor.ToVector2();
        public static bool MouseButtonIsPressed(int mouse) => MouseIsDown[mouse] && _mouseOldIsDown[mouse];

        public static bool MouseButtonIsDown(int mouse) => MouseIsDown[mouse] && !_mouseOldIsDown[mouse];

        public static bool MouseButtonIsReleased(int mouse) => !MouseIsDown[mouse] && _mouseOldIsDown[mouse];

        public static bool MouseButtonIsDown(MouseButton mouseButton)
        {
            return MouseButtonIsDown((int)mouseButton);
        }
    }
}
