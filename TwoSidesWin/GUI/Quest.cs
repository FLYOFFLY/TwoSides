using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoSides.World;

namespace TwoSides.GUI
{
    [Serializable]
    sealed public class Goals
    {
        public bool IsComp;
        public int type,iditem, ammout;
        public string goalstext;

        public Goals(string goal, int iditem, int ammout, int type)
        {
            goalstext = goal;
            this.iditem = iditem;
            this.ammout = ammout;
            this.type = type;
            IsComp = false;
        }
        
        public Goals(string goal)
        {
            this.goalstext = goal;
            type = 1;
            IsComp = false;
        }
        
        public void removeitem()
        {
            int slotids = Program.game.player.getslotitem(iditem, ammout);
            
            if (slotids != -1)
            {
                Program.game.player.slot[slotids].ammount -= ammout;

                if (Program.game.player.slot[slotids].ammount <= 0)
                {
                    Program.game.player.slot[slotids] = new Item();
                    Program.game.player.slot[slotids].IsEmpty = true;
                }

            }
        }
        
        public void setComp()
        {
            if (type == 1) IsComp = true;
        }
        
        public bool IsComplicte()
        {
            if (type == 0)
            {
                return Program.game.player.getslotitem(iditem, ammout) != -1;
            }
            if (type == 2 && Program.game.player.getslotitem(iditem, ammout) != -1)
                IsComp = true;
            return IsComp;
        }
    }

    [Serializable]
    sealed public class Quest
    {
        public String name;

        public ArrayList goals = new ArrayList();

        public Item reward = new Item();
        [NonSerialized]
        public SpriteFont font;

        public Quest(ArrayList goals, Item reward, String name, SpriteFont font)
        {
            this.goals = goals;
            this.reward = reward;
            this.name = name;
            this.font = font;
        }

        public void SetCompliction(int id)
        {
            ((Goals)goals[id]).setComp();
        }

        public Item GetReward()
        {
            return reward;
        }

        public void Passed()
        {
            Program.game.player.setslot(GetReward());
            for (int i = 0; i < goals.Count; i++)
            {
                Goals goal = (Goals)goals[i];
                if (goal.type == 0)
                {
                    goal.removeitem();
                }
            }
            Program.game.player.AddLog("Quest Complete:\n" + name);
        }

        public bool isPassed()
        {
            for (int i = 0; i < goals.Count; i++)
            {
                Goals goal = (Goals)goals[i];
                if (!goal.IsComplicte()) return false;
            }
            return true;
        }

        public int getQuestHeight()
        {
            int a = (int)font.MeasureString(name).Y;
            for (int i = 0; i < goals.Count; i++)
            {
                Goals goal = (Goals)goals[i];
                a += (int)font.MeasureString(goal.goalstext).Y;
            }
            return a + (int)font.MeasureString(getreward()).Y;
        }

        public string getreward()
        {
            return "Reward:" + reward.GetName() + ", Count:" + reward.ammount;
        }

        public string getmax()
        {
            string a = name;
            for (int i = 0; i < goals.Count; i++)
            {
                Goals goal = (Goals)goals[i];
                if (a.Length < goal.goalstext.Length) a = goal.goalstext;
            }
            if (a.Length < getreward().Length) a = getreward();
            return a;
        }

        public void render(SpriteBatch spriteBatch, Vector2 pos)
        {
            if (font == null) font = Program.game.Font1;
            spriteBatch.Begin();
            string max = getmax();
            spriteBatch.Draw(Program.game.dialogtex, new Rectangle((int)pos.X - (int)font.MeasureString(max).X,
                (int)pos.Y, (int)font.MeasureString(max).X, getQuestHeight()), Color.Blue);
            Vector2 position = pos;
            position.X = pos.X - (int)font.MeasureString(name).X;
            spriteBatch.DrawString(font, name, position, Color.White);
            position.Y += (int)font.MeasureString(name).Y;
            for (int i = 0; i < goals.Count; i++)
            {
                Goals goal = (Goals)goals[i];
                position.X = pos.X - (int)font.MeasureString(goal.goalstext).X;
                Color cl = Color.Red;
                if (goal.IsComplicte()) cl = Color.Green;
                spriteBatch.DrawString(font, goal.goalstext, position, cl);
                position.Y += (int)font.MeasureString(goal.goalstext).Y;
            }
            position.X = pos.X - (int)font.MeasureString(getreward()).X;
            spriteBatch.DrawString(font, getreward(), position, Color.Red);
            spriteBatch.End();
        }
    }
}
