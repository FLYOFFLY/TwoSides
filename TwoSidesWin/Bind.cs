using System.Globalization;
using Microsoft.Xna.Framework.Input;

namespace TwoSides
{
    public class Bind
    {
        public string Key       {get; }
        public string Command { get; }
         KeyboardState _keyState;
        public Bind(string key, string command)
        {
            Key = key;
            Command = command;
        }

        public void Update()
        {
            KeyboardState oldKeyboardState = _keyState;
            _keyState = Keyboard.GetState();
            Keys[] pressedKeys = _keyState.GetPressedKeys();
            foreach ( Keys key in pressedKeys )
            {
                if ( oldKeyboardState.IsKeyDown(key) ) continue;

                string keyText = "";
                if ( key >= Keys.A && key <= Keys.Z )
                {
                    // ReSharper disable once InvertIf
                    keyText = key.ToString().ToUpper(CultureInfo.CurrentCulture);
                }
                else if ( IsNum(key) )
                {
                    keyText = key.ToString().Remove(0 , 1);
                }
                else if ( key == Keys.OemQuestion )
                {
                    keyText = "/";
                }

                if ( Key == keyText )
                {

                    Program.Game.Console.UpdateCheat(Command);
                }
            }
        }

        static bool IsNum(Keys key) => key >= Keys.D1 && key <= Keys.D9;

    }
}
