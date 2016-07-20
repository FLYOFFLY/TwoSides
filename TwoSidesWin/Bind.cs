using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TwoSides
{
    public class Bind
    {
        public string Key       {get;private set;}
        public string command { get; private set; }
         KeyboardState keystate_CurrentKeyboardState;
        public Bind(string key, string command)
        {
            this.Key = key;
            this.command = command;
        }
        public void update(KeyboardState keyboardState)
        {
            KeyboardState oldKeyboardState = keystate_CurrentKeyboardState;
            keystate_CurrentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys;
            pressedKeys = keystate_CurrentKeyboardState.GetPressedKeys();
            foreach (Keys key in pressedKeys)
            {
                if (oldKeyboardState.IsKeyUp(key))
                {
                    if (key == Keys.T || key == Keys.Q || key == Keys.W || key == Keys.E || key == Keys.R ||
                       key == Keys.Y || key == Keys.U || key == Keys.I || key == Keys.O || key == Keys.P ||
                       key == Keys.A || key == Keys.S || key == Keys.D || key == Keys.F || key == Keys.G ||
                       key == Keys.Z || key == Keys.L || key == Keys.K || key == Keys.J || key == Keys.H ||
                       key == Keys.X || key == Keys.C || key == Keys.V || key == Keys.B || key == Keys.N || key == Keys.M
                       )
                    {
                        if (this.Key == key.ToString().ToUpper())
                        {
                            Program.game.console.updatecheat(command);
                            break;
                        }
                    }
                    else if (key == Keys.D3 || key == Keys.D2 || key == Keys.D1 || key == Keys.D1 || key == Keys.D0 ||
                       key == Keys.D4 || key == Keys.D5 || key == Keys.D6 || key == Keys.D7 || key == Keys.D9 || key == Keys.D8)
                    {
                        if (this.Key == key.ToString().Remove(0, 1))
                        {
                            Program.game.console.updatecheat(command);
                            break;
                        }
                    }
                    else if (key == Keys.OemQuestion)
                    {
                        if (this.Key ==  "/")
                        {
                            Program.game.console.updatecheat(command);
                            break;
                        }
                    }
                }
            }
        }
    }
}
