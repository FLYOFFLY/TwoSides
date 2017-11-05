using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.World;

namespace TwoSides.GUI
{
    [Serializable]
    public sealed class Goal
    {
        public bool IsComp;
        public int Type,IdItem, Ammout;
        public string GoalsText;

        public Goal(string goal, int idItem, int ammout, int type)
        {
            GoalsText = goal;
            IdItem = idItem;
            Ammout = ammout;
            Type = type;
            IsComp = false;
        }
        
        public Goal(string goal)
        {
            GoalsText = goal;
            Type = 1;
            IsComp = false;
        }
        
        public void RemoveItem()
        {
            int slotIds = Program.Game.Player.GetSlotItem(IdItem, Ammout);

            if ( slotIds == -1 ) return;
            if ( Program.Game.Player.Slot[slotIds].Ammount > Ammout ) return;

            Program.Game.Player.Slot[slotIds].Ammount -= Ammout;
            Program.Game.Player.Slot[slotIds] = new Item {IsEmpty = true};
        }
        
        public void SetComp()
        {
            if (Type == 1) IsComp = true;
        }
        
        public bool IsComplicte()
        {
            switch ( Type ) {
                case 0:
                    return Program.Game.Player.GetSlotItem(IdItem, Ammout) != -1;
                case 2 when Program.Game.Player.GetSlotItem(IdItem, Ammout) != -1:
                    IsComp = true;
                    break;
            }

            return IsComp;
        }
    }

    [Serializable]
    public sealed class Quest
    {
        public string Name;

        public List<Goal> Goals;

        public Item Reward;
        [NonSerialized]
        public SpriteFont Font;

        public Quest(List<Goal> goals, Item reward, string name, SpriteFont font)
        {
            Goals = goals;
            Reward = reward;
            Name = name;
            Font = font;
        }

        public void SetCompliction(int id)
        {
            Goals[id].SetComp();
        }

        public Item GetReward() => Reward;

        public void Passed()
        {
            Program.Game.Player.SetSlot(GetReward());
            foreach (Goal goal in Goals )
            {
                if ( goal.Type != 0 ) continue;
                goal.RemoveItem();
            }
            Program.Game.Player.AddLog("Quest Complete:\n" + Name);
        }

        public bool IsPassed()
        {
            return Goals.All(goal => goal.IsComplicte());
        }

        public int GetQuestHeight()
        {
            int a = (int) Font.MeasureString(Name).Y + Goals.Sum(goal => (int) Font.MeasureString(goal.GoalsText).Y);
            return a + (int)Font.MeasureString(GetRewardText()).Y;
        }

        public string GetRewardText() => "Reward:" + Reward.GetName() + ", Count:" + Reward.Ammount;

        public string GetMax()
        {
            string a = Name;
            foreach ( Goal goal in Goals )
            {
                if (a.Length < goal.GoalsText.Length) a = goal.GoalsText;
            }
            if (a.Length < GetRewardText().Length) a = GetRewardText();
            return a;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 pos)
        {
            if (Font == null) Font = Program.Game.Font1;
            spriteBatch.Begin();
            string max = GetMax();
            spriteBatch.Draw(Program.Game.Dialogtex, new Rectangle((int)pos.X - (int)Font.MeasureString(max).X,
                (int)pos.Y, (int)Font.MeasureString(max).X, GetQuestHeight()), Color.Blue);
            Vector2 position = pos;
            position.X = pos.X - (int)Font.MeasureString(Name).X;
            spriteBatch.DrawString(Font, Name, position, Color.White);
            position.Y += (int)Font.MeasureString(Name).Y;
            foreach ( Goal goal in Goals ) {
                position.X = pos.X - (int)Font.MeasureString(goal.GoalsText).X;
                Color cl = Color.Red;
                if (goal.IsComplicte()) cl = Color.Green;
                spriteBatch.DrawString(Font, goal.GoalsText, position, cl);
                position.Y += (int)Font.MeasureString(goal.GoalsText).Y;
            }
            position.X = pos.X - (int)Font.MeasureString(GetRewardText()).X;
            spriteBatch.DrawString(Font, GetRewardText(), position, Color.Red);
            spriteBatch.End();
        }
    }
}
