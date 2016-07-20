using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NLua;
using TwoSides.GUI;

namespace TwoSides.ModLoader
{
    public class LuaScript : IDisposable
    {
        Lua script;
        public ArrayList userCommandStr = new ArrayList();
        public LuaScript(string filename)
        {
            script = new Lua();
            script.DoFile(filename);
            script.RegisterFunction("addCmdLine", this, typeof(LuaScript).GetMethod("addCmdLine"));
            script.RegisterFunction("addLogConsole", this, typeof(LuaScript).GetMethod("addLogConsoles"));
            script.RegisterFunction("getMaxPoint", this, typeof(LuaScript).GetMethod("MaxPoint"));
            //script.RegisterFunction("drawText", this, typeof(Game1).GetMethod("DrawText"));
            script.RegisterFunction("AddBulletToMouse", this, typeof(Game1).GetMethod("AddBulletToMouse"));
            script["console"] = Program.game.console;
            System.Console.WriteLine(script["console"]);
            script["player"] = Program.game.player;
            System.Console.WriteLine(script["player"]);
            script["NormalWorld"] = Program.game.dimension[0];
            script["ShowWorld"] = Program.game.dimension[1];
            script["HellWorld"] = Program.game.dimension[2];
            callFunction("init");
        }
        public int MaxPoint(int x)
        {
            return Program.game.dimension[Program.game.currentD].GetMax(x);
        }
        public object[] callFunction(string func, params object[] args)
        {
            object[] paramtype = null;
            if (script[func] is LuaFunction)
            {
                LuaFunction funcLua = script[func] as LuaFunction;
                if (funcLua != null)
                {
                    if (args != null) paramtype = funcLua.Call(args);
                    else paramtype = funcLua.Call();
                }
                funcLua.Dispose();
            }
            return paramtype;
        }
        //
        public void DrawText(string a)
        {
            Program.game.DrawText("HI", 5, 5);
        }
        public void addLogConsoles(string log)
        {
            Program.game.console.addLog(log);
        }
        public void addCmdLine(string cmd, string funcname)
        {
            System.Console.WriteLine("HI");
            userCommandStr.Add(new Command(cmd, funcname));
        }
        protected virtual void Dispose(bool mine)
        {
            if (mine)
            {
                userCommandStr.Clear();
            }
            script.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
