using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.World;
using System.Collections;
using TwoSides.GameContent.Dimensions;
using TwoSides.Utils;
using TwoSides.ModLoader;
namespace TwoSides.GUI
{
    sealed public class Console
    {
        public bool isactive = false;
        GUI gui;
        Log consoleLog = new Log("console");
        public Console(SpriteFont font)
        {
            gui = new GUI();
            TextField inp = new TextField(new Vector2(100, 100), font);
            inp.e_OnEnter += updatecheat;
            gui.AddInput(inp);
            Label lab = new Label("", new Vector2(100, 80), font);
            gui.AddLabel(lab);
            consoleLog.hasData = true;
        }

        public void addLog(string log)
        {
            string newline = DateTime.Now.ToString("[yyyy:dd:hh:mm:ss]") + log;
            string a = gui.GetLabel(0).GetText() + "\n" + newline;
            consoleLog.WriteLog(log);
            gui.GetLabel(0).SetText(a);
            gui.GetLabel(0).Up(14);
        }

        void addblock(int blockid)
        {
            Item temp = new Item();
            temp.iditem = blockid;
            temp.ammount = 1;
            temp.IsEmpty = false;
            Program.game.player.setslot(temp);
        }

        public void changeactive()
        {
            isactive = !isactive;
        }
        public void update()
        {
            gui.Update();
        }
        public void addBlockWorld(int x, int y,int id)
        {
            Program.game.dimension[Program.game.currentD].settexture(x, y, id);
        }
        public void updatecheat(Object sender,TextFieldArgs e)
        {
            updatecheat(e.Text);
        }

        public void updatecheat(string text)
        {
            String[] fs = text.Split();
            if (fs.Count() != 0)
            {
                if (fs[0] == "Kill".ToUpper() && fs.Count() == 1) Program.game.player.spawn();
                else if (fs[0] == "ATLA".ToUpper() && fs.Count() == 1) Program.game.player.activespecial(3);
                else if (fs[0] == "Minenotch".ToUpper() && fs.Count() == 1) Program.game.player.activespecial(1);
                else if (fs[0] == "DK19981".ToUpper() && fs.Count() == 1) Program.game.player.activespecial(2);
                else if (fs[0] == "Bind".ToUpper() && fs.Count() > 2)
                {

                    string command = "";
                    for (int i = 2; i < fs.Count(); i++)
                    {
                        command += fs[i];
                    }
                    GameInput.BindKey.Add(new Bind(fs[1], command));
                }
                else if (fs[0] == "tp".ToUpper() && fs.Count() == 3) Program.game.player.tp(int.Parse(fs[1]), int.Parse(fs[2]));
                else if (fs[0] == "tpx".ToUpper() && fs.Count() == 2) Program.game.player.tp(int.Parse(fs[1]));
                else if (fs[0] == "AddBlock".ToUpper() && fs.Count() == 2) addblock(int.Parse(fs[1]));
                else if (fs[0] == "ABW".ToUpper() && fs.Count() == 3 + 1) addBlockWorld(int.Parse(fs[1]), int.Parse(fs[2]), int.Parse(fs[3]));
                else if (fs[0] == "CAVE".ToUpper() && fs.Count() == 1)
                {
                    if (Program.game.dimension[Program.game.currentD] is NormalWorld)
                        ((NormalWorld)Program.game.dimension[Program.game.currentD]).CaveOpenater((int)Program.game.player.getXLocal(), (int)Program.game.player.getYLocal());
                }
                foreach (ModFile mod in ModLoader.ModLoader.Mods)
                {
                    foreach (LuaScript script in mod.getScripts())
                    {
                        foreach (Command cmd in script.userCommandStr)
                        {
                            if (fs[0] == cmd.cmd.ToUpper()) script.callFunction(cmd.funcname);
                        }
                    }
                }
                addLog(text);
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            gui.Draw(spriteBatch);
        }
    }
    public class Command
    {
        public string cmd;
        public string funcname;
        public Command(string cmd, string funcname)
        {
            this.cmd = cmd;
            this.funcname = funcname;
        }
    }
}
