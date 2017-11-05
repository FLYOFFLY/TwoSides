using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides;
using TwoSides.GUI;
using TwoSides.Utils;

using TwoSides.GameContent.Dimensions;
using TwoSides.World;

namespace TwoSides.GUI.HUD
{
    public sealed class Console
    {
        public bool Isactive;
        readonly XnaLayout _gui;
        readonly Log _consoleLog = new Log("console");
        public Console(SpriteFont font)
        {
            _gui = new XnaLayout();
            TextField inp = new TextField(new Vector2(100, 100), font);
            inp.OnEnter += UpdateCheat;
            _gui.AddInput(inp);
            Label lab = new Label("", new Vector2(100, 80), font);
            _gui.AddLabel(lab);
            _consoleLog.HasData = true;
        }

        public void AddLog(string log)
        {
            string newline = DateTime.Now.ToString("[yyyy:dd:hh:mm:ss]", CultureInfo.InvariantCulture) + log;
            string a = _gui.GetLabel(0).GetText() + "\n" + newline;
            _consoleLog.WriteLog(log);
            _gui.GetLabel(0).SetText(a);
            _gui.GetLabel(0).Up(14);
        }

        static void AddItem(int blockId)
        {
            Item temp = new Item
                        {
                            Id = blockId ,
                            Ammount = 1 ,
                            IsEmpty = false
                        };
            Program.Game.Player.SetSlot(temp);
        }

        public void Changeactive()
        {
            Isactive = !Isactive;
        }
        public void Update()
        {
            _gui.Update();
        }
        public void AddBlockWorld(int x, int y,int id)
        {
            Program.Game.Dimension[Program.Game.CurrentDimension].SetTexture(x, y, id);
        }
        public void UpdateCheat(object sender,TextFieldArgs e)
        {
            UpdateCheat(e.Text);
        }

        public void UpdateCheat(string text)
        {
            string[] fs = text.ToUpper(CultureInfo.CurrentCulture).Split();
            if ( fs.Length == 0 ) return;

            StandartCommand(fs);
            AddLog(text);
        }

        public void PlayerCommand(IReadOnlyList<string> fs)
        {
            if(fs[0] == "Kill".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 1) Program.Game.Player.Spawn();
            else if(fs[0] == "ATLA".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 1) Program.Game.Player.ActiveSpecial(3);
            else if(fs[0] == "Minenotch".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 1) Program.Game.Player.ActiveSpecial(1);
            else if(fs[0] == "DK19981".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 1) Program.Game.Player.ActiveSpecial(2);
            
        }

        void StandartCommand(IReadOnlyList<string> fs)
        {
            PlayerCommand(fs);
            if (fs[0] == "Bind".ToUpper(CultureInfo.CurrentCulture) && fs.Count > 2)
            {

                string command = "";
                for (int i = 2; i < fs.Count; i++)
                {
                    command += fs[i];
                }
                GameInput.BindKey.Add(new Bind(fs[1], command));
            }
            else if (fs[0] == "tp".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 3) Program.Game.Player.Teleport(int.Parse(fs[1], CultureInfo.CurrentCulture), int.Parse(fs[2], CultureInfo.CurrentCulture));
            else if (fs[0] == "tpx".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 2) Program.Game.Player.Teleport(int.Parse(fs[1], CultureInfo.CurrentCulture));
            else if (fs[0] == "AddBlock".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 2) AddItem(int.Parse(fs[1], CultureInfo.CurrentCulture));
            else if (fs[0] == "ABW".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 4) AddBlockWorld(int.Parse(fs[1], CultureInfo.CurrentCulture), int.Parse(fs[2], CultureInfo.CurrentCulture), int.Parse(fs[3], CultureInfo.CurrentCulture));
            else if (fs[0] == "CAVE".ToUpper(CultureInfo.CurrentCulture) && fs.Count == 1)
            {
                if (Program.Game.Dimension[Program.Game.CurrentDimension] is NormalWorld world)
                    world.CaveOpenater((int)Program.Game.Player.GetXLocal(), (int)Program.Game.Player.GetYLocal());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _gui.Draw(spriteBatch);
        }
    }
    public class Command
    {
        public string Cmd;
        public string Funcname;
        public Command(string cmd, string funcname)
        {
            Cmd = cmd;
            Funcname = funcname;
        }
    }
}
