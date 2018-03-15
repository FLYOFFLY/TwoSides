using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TwoSides.GameContent.Dimensions;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GUI;
using TwoSides.Physics;
using TwoSides.Physics.Entity;
using TwoSides.Physics.Entity.NPC;
using TwoSides.Utils;
using TwoSides.World;
using TwoSides.World.Tile;

namespace TwoSides.GameContent.Entity
{
    [Serializable]
    public sealed class Achivement{
        const int ACHIVEMENT_COUNT = 2;
        public enum AchivementType
        {
            MODIFED_TERRAIN = 0,
            TALKING_NPC
        }

        readonly bool[] _achivemets= new bool[ACHIVEMENT_COUNT];
        readonly string[] _desc = new string[ACHIVEMENT_COUNT];
        readonly string[] _name = new string[ACHIVEMENT_COUNT];

        public Achivement(){
            _desc[0] = "Modifed Terrain";
            _desc[1] = "Tallking NPC";
            _name[0] = "God";
            _name[1] = "Communicative";
            for (int i = 0; i < ACHIVEMENT_COUNT; i++)
            {
                _achivemets[i] = false;
            }
        }
        public void Complete(AchivementType  type)
        {
            if (!_achivemets[(int)type])
            {
                Program.Game.Player.AddLog("Achivement Complete:\n" + _name[(int)type] + "\n" + _desc[(int)type]); // 0		
            }
            _achivemets[(int)type] = true;
        }
    }

    [Serializable]
    public sealed class Stats
    {

        int _translition;
        int _killBossUltraNpc;

        readonly int[] _idItems = new int[Item.ItemMax + Clothes.MaxArmor];
        
        public Stats()
        {
            for (int i = 0; i < Item.ItemMax + Clothes.MaxArmor; i++)
            {
                _idItems[i] = new int();
            }
            Clear();
        }
        
        public void Clear()
        {
            for (int i = 0; i < Item.ItemMax + Clothes.MaxArmor; i++)
            {
                _idItems[i] = 0;
            }
        }
        
        public void AddRecipe(int iditem, int ammount){
            _idItems[iditem] += ammount;
        }
        public enum StatsType
        {
            ITEM_COUNT,
            KILL_BOSS,
            TELEPORT_TO_OTHER_WORLD

        }
        public int Getstats(StatsType type, int value)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch ( type ) {
                case StatsType.ITEM_COUNT:
                    return _idItems[value];
                case StatsType.KILL_BOSS:
                    // Program.game.console.addlog(type + " " + KillBossUltraNPC);
                    return _killBossUltraNpc;
                default:
                    Program.Game.Console.AddLog(type + " " + _translition);
                    return _translition;
            }
        }
        public void SetStats(StatsType type, int value)
        {
            if (type == StatsType.KILL_BOSS)
            {
                // Program.game.console.addlog(type + " " + KillBossUltraNPC);
                _killBossUltraNpc = 1;
            }
            else
            {
                Program.Game.Console.AddLog(type + " " + _translition);
               _translition ++;
            }
        }
    }
    
    public class Player : DynamicEntity
    {
        #region CONSTS
        
        #region INVENTORY
        public const int Slotmax = 9 * 5;
        #endregion
        
        #region  PHYSIC
        const int JUMP_HEIGHT = 15;
        const float RUN_ACCELERATION = 0.08f;
        const float RUN_SLOWDOWN = 0.2f;
        const float JUMP_SPEED = 5.01f;
        #endregion

        #endregion

        #region FIELDS

        #region INVENTORY
        public Item[] Slot = new Item[Slotmax + 3];
        public Clothes[] Clslot = new Clothes[6];
        public int CurrentMaxSlot = 1 + 9 * 4;
        public Color[] Colors = new Color[5];
        #endregion

        #region QUEST
        public List<Quest> Quests = new List<Quest>();
        Quest _quest;
        #endregion

        #region STATS
        readonly Achivement _achivement = new Achivement();
        public bool[] Bloods = new bool[4];
        Color _colorRace;
        readonly Stats _stats;
        public bool Speedup;
        public float Temperature = 36;
        public float Blood;
        public float Hunger;
        public float Drought;
        #endregion

        #region INPUT
        MouseState _mouseState;
        bool _controlLeft;
        bool _controlRight;
        bool _controlG;
        bool _controlJump;
        int _direction = 1;
        public bool ControlJ;
        public bool Keydownf;
        public bool Isj;
        public bool ReleaseJump;
        public int Jump;

        public int TileTargetX;
        public int TileTargetY;
        [NonSerialized]
        KeyboardState _keyState;
        int _timeSlotUse;
        public Item MouseItem;//исправить
        #endregion

        #region PHYSIC

        [NonSerialized]
        Vector2 _gravityDirection = new Vector2(0, 1);
        float _maxRunSpeed = 3f;
        // ReSharper disable once InconsistentNaming
        protected new float MaxFallSpeed = 20f;
        #endregion

        readonly List<FlashText> _fTlist = new List<FlashText>();

        [NonSerialized]
        public Rectangle Rect;
        bool _zombiekill;
        bool _itemto;
        int _special = -1;
        float _itemframe;
        float _animationfog;


        float _deltaAnimation;
        public bool Zombie;
        public bool IsOpen;

        public int Width = 20;
        public int Height = 42;
        public int ItemAnimation;
        public int TypeKill = -1;
        public int Carma;
        public int Bellframe;
        public string Name = "";
        [NonSerialized]
        Vector2 _tilePos = new Vector2(-1, -1);
        #endregion
        
        #region PROPERTYS
        public Quest Quest2 { get; set; }
        public Quest Quest3 { get; set; }
        public Quest Quest4 { get; set; }
        public Quest Quest5 { get; set; }
        public int SelectedItem { get; private set; }
        public bool IsHorisontal { get; set; }
        #endregion

        public Player()
        {
            _stats = new Stats();
            for (int i = 0; i < Slotmax + 3; i++)
            {
                Slot[i] = new Item();
            }
            for (int i = 0; i < 5; i++)
            {
                Colors[i] = Color.Red;
            }
            Rect.Width = Width;
            _zombiekill = false;
            Rect.Height = Height;
        }

        #region METHODS

        public override void Load(BinaryReader reader)
        {
            base.Load(reader);
            for (int i = 0; i < 6; i++)
            {
                Clslot[i] = new Clothes();
                Clslot[i].Load(reader);
            }
            for (int i = 0; i < Slotmax + 3; i++)
            {
                Slot[i] = new Item();
                Slot[i].Load(reader);
            }
            for (int i = 0; i < 5; i++)
            {
                Colors[i] = Tools.ReadColor(reader);
            }
            Name = reader.ReadString();
            _colorRace = Tools.ReadColor(reader);
        }
        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            for (int i = 0; i < 6; i++)
            {
                Clslot[i].Save(writer);
            }
            for (int i = 0; i < Slotmax + 3; i++)
            {
                Slot[i].Save(writer);
            }
            for (int i = 0; i < 5; i++)
            {
                Tools.SaveColor(Colors[i], writer);
            }
            writer.Write(Name);
            Tools.SaveColor(_colorRace, writer);
        }


        public void BossKill() => _stats.SetStats(Stats.StatsType.KILL_BOSS, _stats.Getstats(Stats.StatsType.KILL_BOSS, 0) + 1);

        public int GetDef() => Slot[Slotmax].GetDef() + Slot[Slotmax + 1].GetDef() + Slot[Slotmax + 2].GetDef();

        public float GetX() => Position.X;

        public float GetY() => Position.Y;

        public float GetXLocal() => Position.X / Tile.TileMaxSize;

        public float GetYLocal() => Position.Y / Tile.TileMaxSize;

        void RemoveRecipes(int id)
        {
            for (int i = 0; i < Recipe.Recipes[id].GetSize(); i++)
            {

                int slotIds = GetSlotItem(Recipe.Recipes[id].GetIngrident(i), Recipe.Recipes[id].Hp, Recipe.Recipes[id].GetSize(i));
                if (slotIds == -1) continue;

                Slot[slotIds].Ammount -= Recipe.Recipes[id].GetSize(i);

                if (Slot[slotIds].Ammount <= 0)
                {
                    Slot[slotIds] = new Item { IsEmpty = true };
                }
            }
        }

        public bool GetValidRecipes(int id)
        {
            bool t = false;
            for (int i = 0; i < Recipe.Recipes[id].GetSize(); i++)
            {
                int slotIds = GetSlotItem(Recipe.Recipes[id].GetIngrident(i),
                                          Recipe.Recipes[id].Hp,
                                          Recipe.Recipes[id].GetSize(i));

                if (slotIds != -1)
                {
                    t = true;
                }
                else { t = false; break; }
            }

            if (!t) return false;

            if (!Recipe.Recipes[id].Isblock) return true;
            for (int xi = -3; xi <= 3; xi++)
            {
                for (int yi = -5; yi <= 5; yi++)
                {
                    int newx = (int)(Position.X / Tile.TileMaxSize) + xi;
                    int newy = (int)(Position.Y / Tile.TileMaxSize) + yi;
                    if (newx >= SizeGeneratior.WorldWidth ||
                        newx < 0) continue;
                    if (newy >= SizeGeneratior.WorldHeight ||
                        newy < 0) continue;
                    if (Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[newx, newy].IdTexture == Recipe.Recipes[id].Idblock) return true;
                }
            }

            return false;
        }

        public void SetColorRace(RaceType id)
        {
            if (Race.Racelist.Count <= 0)
            {
                switch (id)
                {
                    case RaceType.NIGER:
                        _colorRace = new Color(81, 21, 21);
                        break;
                    case RaceType.EUROPIAN:
                        _colorRace = new Color(213, 171, 123);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(id), id, null);
                }
            }

            else
            {
                Race race = Race.Racelist[(int)id - 1];
                _colorRace = race.GetColor();
            }
        }

        public void SetUserColor(Color clrace) => _colorRace = clrace;

        public Color GetColor() => _colorRace;

        void MissionHelloWorld(Civilian civ,int ActiveDialog)
        {
        }
        void MissionQuest2(Civilian civ, int ActiveDialog)
        {
        }
        void MissionQuest3(Civilian civ)
        {
        }
        void Dialog()
        {
            Civilian civ = Program.Game.Dimension[0].Civil[0];

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (civ.Activedialog)
            {
                case 1:
                    int questId1 = Quests.IndexOf(_quest);
                    if (_zombiekill) Quests[questId1].SetCompliction(0);
                    break;
                case 3:
                    int questId3 = Quests.IndexOf(Quest2);

                    PassedQuestDialog(civ, questId3, 4, Quests[questId3]);
                    break;
                case 5:
                    int questId5 = Quests.IndexOf(Quest3);
                    if (_stats.Getstats(Stats.StatsType.ITEM_COUNT, 19) >= 2) Quests[questId5].SetCompliction(0);
                    if (_stats.Getstats(Stats.StatsType.KILL_BOSS, 8) >= 1) Quests[questId5].SetCompliction(1);
                    PassedQuestDialog(civ, questId5, 6, Quests[questId5]);
                    break;
                case 6:
                    JobDialog(civ, Quest4, 7);
                    break;
                case 7:
                    {
                        int questId7 = Quests.IndexOf(Quest4);
                        if (_stats.Getstats(0, 22) >= 1) Quests[questId7].SetCompliction(1);
                        if (_stats.Getstats(0, 23) >= 1) Quests[questId7].SetCompliction(2);
                        if (_stats.Getstats(0, 24) >= 1) Quests[questId7].SetCompliction(3);
                        PassedQuestDialog(civ, questId7, 8,Quests[questId7]);
                        break;
                    }
                case 8:
                    JobDialog(civ, Quest5, 9);
                    break;
                case 9:
                    break;
            }
        }

        void PassedQuestDialog(Civilian civ, int questId, int newDialog, Quest currentQuest)
        {
            if (currentQuest.IsPassed())
            {
                civ.ToggleVisible(1, true);
                civ.ToggleVisible(3, true);
            }
            else
            {
                civ.ToggleVisible(1, false);
                civ.ToggleVisible(3, false);
            }
        }

        private void PassedQuestNo(Civilian civ,bool isCarma)
        {
            if (isCarma) Carma = Math.Max(Carma - 2, 0);
            civ.ToggleEnabled();
        }

        private void PassedQuestYes(Civilian civ, int questId, int newDialog,bool isCarma)
        {
            if(isCarma) Carma = Math.Max(Carma - 1, 0);
            _quest.Passed();
            Quests.RemoveAt(questId);
           if(newDialog>=0) civ.Activedialog = newDialog;

        }

        void JobDialog(Civilian civ, Quest questGive, int newDialog)
        {

           
        }

        private void AccesQuest(Civilian civ,int questID,Quest questAcces)
        {
            civ.Activedialog = questID;
            Quests.Add(questAcces);
        }

        public void QuestUpdate()
        {
            int questId = Quests.IndexOf(Quest5);
            if (questId < 0) return;

            if (_stats.Getstats(Stats.StatsType.KILL_BOSS, 0) >= 1) Quests[questId].SetCompliction(0);
            if (_stats.Getstats(Stats.StatsType.TELEPORT_TO_OTHER_WORLD, 0) >= 1)
            {
                Quests[questId].SetCompliction(1);
            }
            Quest questg = Quests[questId];
            if (!questg.IsPassed()) return;

            _quest.Passed();
            Quests.RemoveAt(questId);
        }
        public void Spawn()
        {


            //  stats.Clear();
            ItemAnimation = 0;
            Quests.Clear();
            List<Goal> gl = new List<Goal> { new Goal("Kill 1 Zombie") };
            List<Goal> gl2 = new List<Goal>
                            {
                                new Goal("Obtain 3 stone" , 1 , 3 , 2) ,
                                new Goal("Obtain 1 wood" , 5 , 1 , 2) ,
                                new Goal("Obtain 1 Water Botle" , 255 , 1 , 2) ,
                                new Goal("Obtain 1 Burger" , 256 , 1 , 2)
                            };
            List<Goal> gl3 = new List<Goal> { new Goal("Crafting 2 wand"), new Goal("Crafting PickAxe") };
            List<Goal> gl4 = new List<Goal>
                            {
                                new Goal("Obtain 8 stone" , 1 , 8 , 2) ,
                                new Goal("Crafting stone helmet") ,
                                new Goal("Crafting stone breastplate") ,
                                new Goal("Crafting stone Shoes")
                            };
            List<Goal> gl5 = new List<Goal> { new Goal("Kill ultra zombie"), new Goal("send the show world") };
            _quest = new Quest(gl, new Item(1, 25), "First step", Program.Game.Font1);
            Quest2 = new Quest(gl2, new Item(1, 11), "Gathering Resources", Program.Game.Font1);
            Quest3 = new Quest(gl3, new Item(1, 10), "Master", Program.Game.Font1);
            Quest4 = new Quest(gl4, new Item(2, 14), "Preparation", Program.Game.Font1);
            Quest5 = new Quest(gl5, new Item(2, 11), "First Win", Program.Game.Font1);
            if (Program.Game.Dimension[Program.Game.CurrentDimension].Civil.Count >= 1)
            {
                Civilian civs = Program.Game.Dimension[Program.Game.CurrentDimension].Civil[0];
                AddQuestDialog(civs, 0, 1, _quest);
                AddQuestDialog(civs, 2, 3, Quest2);
                AddQuestDialog(civs, 4, 5, Quest3);
                AddQuestDialog(civs, 6, 7, Quest4);
                AddQuestDialog(civs, 8, 9, Quest5);
                AddQuestDialog(civs, 9);

                AddQuestDialog(civs, 1, 2);
                AddQuestDialog(civs, 3, 4);
                AddQuestDialog(civs, 5, 6);
                AddQuestDialog(civs, 7, 8);
            }

            IsOpen = false;
            MouseItem = new Item();
            Zombie = false;
            Hunger = 100;
            Drought = 100;
            Position.X = 1 * Tile.TileMaxSize;
            Bellframe = 0;
            Position.Y = Program.Game.Dimension[Program.Game.CurrentDimension].MapHeight[1] * Tile.TileMaxSize - Tile.TileMaxSize * 3;
            for (int i = 0; i < Slotmax + 3; i++)
            {
                Slot[i] = new Item { IsEmpty = true };
            }

            for (int i = 0; i < 4; i++)
            {
                Bloods[i] = false;
            }
            Slot[0].IsEmpty = false;
            Slot[0].Ammount = 1;
            Carma = 100;
            Slot[0].Id = 8;

            SelectedItem = 0;
            Slot[2].Id = 1;
            Slot[2].Ammount = 6;
            Slot[2].IsEmpty = false;
            Slot[1].Id = 6;
            Slot[1].Ammount = 1;
            Slot[1].IsEmpty = false;
            Slot[3].Id = 25;
            Slot[3].Ammount = 1;
            Slot[3].IsEmpty = false;
            Blood = 5000;
            _maxRunSpeed = 3;
            Temperature = 36.6f;
            _stats.Clear();
            TypeKill = -1;
            foreach (Civilian civ in Program.Game.Dimension[Program.Game.CurrentDimension].Civil)
            {
                civ.Activedialog = 0;
                civ.Off();
            }
            //slot[slotmax] = new Item(1, 31);
            // slot[slotmax+1] = new Item(1, 32);
            //slot[slotmax+2] = new Item(1, 33);
            _deltaAnimation = 0;
        }

        private void AddQuestDialog(Civilian civs, int idStartQuest, int idNextDialog, Quest quest)
        {
            Program.Game.Dimension[0].Civil[0].Dialog[idStartQuest].Buttons[0].OnClicked += (_, _a) => { Carma = Math.Max(Carma - 2, 0); civs.ToggleEnabled(); };
            Program.Game.Dimension[0].Civil[0].Dialog[idStartQuest].Buttons[1].OnClicked += (_, _a) => { Carma = Math.Max(Carma + 2, 0); AccesQuest(civs, idNextDialog, quest); };
            Program.Game.Dimension[0].Civil[0].Dialog[idStartQuest].Buttons[3].OnClicked += (_, _a) => { AccesQuest(civs, idNextDialog, quest); };
            Program.Game.Dimension[0].Civil[0].Dialog[idStartQuest].Buttons[2].OnClicked += (_, _a) => { civs.ToggleEnabled(); };
        }

        private void AddQuestDialog(Civilian civs, int i)
        {
            Program.Game.Dimension[0].Civil[0].Dialog[i].Buttons[0].OnClicked += (_, _a) => { Carma = Math.Max(Carma - 2, 0); civs.ToggleEnabled(); };
            Program.Game.Dimension[0].Civil[0].Dialog[i].Buttons[1].OnClicked += (_, _a) => civs.ToggleEnabled();
            Program.Game.Dimension[0].Civil[0].Dialog[i].Buttons[3].OnClicked += (_, _a) => civs.ToggleEnabled();
            Program.Game.Dimension[0].Civil[0].Dialog[i].Buttons[2].OnClicked += (_, _a) => civs.ToggleEnabled();
        }

        private void AddQuestDialog(Civilian civs, int i, int questIDNew)
        {
            Program.Game.Dimension[0].Civil[0].Dialog[i].Buttons[0].OnClicked += (_, _a) => PassedQuestNo(civs, true);
            Program.Game.Dimension[0].Civil[0].Dialog[i].Buttons[1].OnClicked += (_, _a) => PassedQuestYes(civs, Quests.IndexOf(_quest), questIDNew, true);
            Program.Game.Dimension[0].Civil[0].Dialog[i].Buttons[3].OnClicked += (_, _a) => PassedQuestYes(civs, Quests.IndexOf(_quest), questIDNew, false);
            Program.Game.Dimension[0].Civil[0].Dialog[i].Buttons[2].OnClicked += (_, _a) => PassedQuestNo(civs, false);
        }

        public int GetSlotFull()
        {
            for (int i = 0; i < Slotmax; i++)
            {
                if (Slot[i].IsEmpty) return i;
            }
            return -1;
        }

        public int GetSlotItem(int iditem)
        {
            for (int i = 0; i < CurrentMaxSlot; i++)
            {
                if (!Slot[i].IsEmpty && Slot[i].Id == iditem && Slot[i].GetStack() > Slot[i].Ammount) return i;

            }
            return -1;
        }

        public int GetSlotItem(int iditem, int ammout)
        {
            for (int i = 0; i < CurrentMaxSlot; i++)
            {
                if (!Slot[i].IsEmpty && Slot[i].Id == iditem && Slot[i].GetStack() > Slot[i].Ammount && Slot[i].Ammount >= ammout) return i;

            }
            return -1;
        }

        public int GetSlotItem(int iditem, float hpb, bool plusravno = true)
        {
            for (int i = 0; i < CurrentMaxSlot; i++)
            {
                if (!Slot[i].IsEmpty && Slot[i].Id == iditem && (Slot[i].Hp >= hpb && plusravno || Math.Abs(Slot[i].Hp - hpb) < float.Epsilon && !plusravno) && Slot[i].GetStack() > Slot[i].Ammount) return i;

            }
            return -1;
        }

        public int GetSlotItem(int iditem, float hpb, int ammout, bool plusravno = true)
        {
            for (int i = 0; i < CurrentMaxSlot; i++)
            {
                if (!Slot[i].IsEmpty && Slot[i].Ammount >= ammout && Slot[i].Id == iditem && (Slot[i].Hp >= hpb && plusravno || Math.Abs(Slot[i].Hp - hpb) < float.Epsilon && !plusravno) && Slot[i].GetStack() > Slot[i].Ammount) return i;

            }
            return -1;
        }

        public void ClearClothes()
        {
            for (int i = 0; i < Clslot.Length; i++)
            {
                Clslot[i] = new Clothes();
            }
        }

        public void RenderFog(Texture2D fog, SpriteBatch spriteBatch)
        {
            if (_animationfog > fog.Width - Program.Game.Resolution.X)
            {
                _animationfog = 0;
            }
            else _animationfog += Program.Game.Seconds * ((fog.Width - Program.Game.Resolution.X) / 60.0f);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp);
            Rectangle dest = new Rectangle(0, 0,
           Program.Game.Resolution.X * 2, Program.Game.Resolution.Y * 2);
            Rectangle src = new Rectangle((int)_animationfog, 0, 800, 600);
            if (Program.Game.CurrentDimension == 2)
            {
                if (Program.Game.Dimension[Program.Game.CurrentDimension].MapHeight[(int)Position.X / Tile.TileMaxSize] > Position.Y / Tile.TileMaxSize) spriteBatch.Draw(fog, dest, src, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            spriteBatch.End();
        }

        public void Render(Texture2D eye, Texture2D head, Texture2D body, Texture2D legs, SpriteBatch spriteBatch, float scale = 1.0f)
        {
            SpriteEffects effect = SpriteEffects.None;
            Rectangle dest = new Rectangle(
                (int)(Position.X + (Width - head.Width)),
                (int)Position.Y, (int)(head.Width * scale), (int)(head.Height * scale));
            Rectangle src = new Rectangle(0, 0,
                24, 42);
            Rectangle src2 = new Rectangle(0, ItemAnimation * 42,
                 24, 42);
            if (_direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(head, dest, src, _colorRace, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(body, dest, src, _colorRace, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(legs, dest, src2, _colorRace, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(eye, dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            RenderClothes(spriteBatch, effect, dest, src, src2);
            //   spriteBatch.Draw(Program.game.shootgun, dest2, new Rectangle(0,0,40,17), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            // spriteBatch.Draw(suit[1], dest, src, Color.White, 0, Vector2.Zero, effect, 0);//
        }

        void RenderClothes(SpriteBatch spriteBatch, SpriteEffects effect, Rectangle dest, Rectangle src, Rectangle src2)
        {
            Clslot[0].Render(spriteBatch, dest, src, Color.Black, 0, effect);
            Clslot[1].Render(spriteBatch, dest, src, Colors[0], 1, effect);
            Clslot[2].Render(spriteBatch, dest, src2, Colors[1], 2, effect);
            Clslot[3].Render(spriteBatch, dest, src2, Colors[2], 3, effect);
            Clslot[4].Render(spriteBatch, dest, src, Colors[3], 4, effect);
            Clslot[5].Render(spriteBatch, dest, src, Colors[4], 5, effect);
            if (!Slot[Slotmax].IsEmpty)
            {
                spriteBatch.Draw(Clothes.Armor[Slot[Slotmax].GetArmorModel()], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            }
            if (!Slot[Slotmax + 1].IsEmpty)
            {
                spriteBatch.Draw(Clothes.Armor[Slot[Slotmax + 1].GetArmorModel()], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            }
            if (!Slot[Slotmax + 2].IsEmpty)
            {
                spriteBatch.Draw(Clothes.Armor[Slot[Slotmax + 2].GetArmorModel()], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            }
            // ReSharper disable once InvertIf
            if (_special != -1)
            {
                spriteBatch.Draw(Clothes.Suit[_special], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            }
        }

        public void RenderLeftPart(Texture2D hand, SpriteBatch sb, float scale = 1.0f)
        {
            SpriteEffects effect = SpriteEffects.None;
            Rectangle dest = new Rectangle(
                (int)(Position.X + (Width - hand.Width)),
                (int)Position.Y, (int)(hand.Width * scale), (int)(hand.Height * scale));
            Rectangle src = new Rectangle(0, 0,
                24, 42);
            if (_direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            sb.Draw(hand, dest, src, _colorRace, 0, Vector2.Zero, effect, 0);
            Clslot[1].RenderLeft(sb, dest, src, Colors[0], 1, effect);
            Clslot[5].RenderLeft(sb, dest, src, Colors[4], 5, effect);
        }

        public void Render(Texture2D hand, Texture2D eye, Texture2D head, Texture2D body, Texture2D legs, SpriteBatch spriteBatch, Texture2D shadow)
        {
            RenderLeftPart(hand, spriteBatch);
            if (!Slot[SelectedItem].IsEmpty)
            {
                if (_direction > 0)
                {
                    /*spriteBatch.Draw(itemIcon,
                        new Rectangle((int)position.X + 22 - 8,
                        (int)position.Y + 30 - 8,
                        16, 16),
                        src,
                        Color.White, itemframe, new Vector2(0, itemIcon.Height / 2), effect, 0);
                        */
                    Item.Render(spriteBatch, Slot[SelectedItem].Id,
                        (int)Position.X + 22 - 8, (int)(Position.Y + 30 - 8), SpriteEffects.None, _itemframe, new Vector2(0, 0.5f));
                }
                else
                {
                    Item.Render(spriteBatch, Slot[SelectedItem].Id, (int)Position.X - 22 + 16,
                            (int)Position.Y + 30 - 8, SpriteEffects.FlipHorizontally, -_itemframe, new Vector2(0.5f, 0.5f));
                    /*    spriteBatch.Draw(itemIcon,
                            new Rectangle((int)position.X- 22 + 16,
                            (int)position.Y + 30 - 8,
                            16, 16),
                            src,
                            Color.White, -itemframe, new Vector2(itemIcon.Width / 2, itemIcon.Height / 2), effect, 0);
                    */
                }
            }
            Render(eye, head, body, legs, spriteBatch);
            int startnew = (int)Position.Y / Tile.TileMaxSize;
            startnew += 3;
            int endnew = startnew + 10;
            for (int j = startnew; j < endnew; j++)
            {
                if (j >= SizeGeneratior.WorldHeight) continue;
                if (!Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[(int)Math.Floor(Position.X) / Tile.TileMaxSize, j].Active) continue;

                int del = j - startnew;
                //if (del == 0) del = 1;
                Rectangle rectShadow = new Rectangle((int)Math.Floor(Position.X),
                                                     j * Tile.TileMaxSize, shadow.Width, shadow.Height - del);

                spriteBatch.Draw(shadow, rectShadow, Color.Black);
                break;
            }
            //   spriteBatch.Draw(Program.game.shootgun, dest2, new Rectangle(0,0,40,17), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            // spriteBatch.Draw(suit[1], dest, src, Color.White, 0, Vector2.Zero, effect, 0);//
        }

        public void Teleport()
        {
            Position.X = 1 * Tile.TileMaxSize;
            Position.Y = Program.Game.Dimension[Program.Game.CurrentDimension].MapHeight[1] * Tile.TileMaxSize - Tile.TileMaxSize * 3;
            if (Position.Y >= (SizeGeneratior.WorldHeight - 1) * Tile.TileMaxSize) Position.Y = Tile.TileMaxSize * (SizeGeneratior.WorldHeight - 1);
        }

        public void Teleport(int x, int y)
        {
            Position.X = x * Tile.TileMaxSize;
            Position.Y = y * Tile.TileMaxSize;
        }

        public void Teleport(int x)
        {
            if (x < 0 || x >= SizeGeneratior.WorldWidth) return;

            Position.X = x * Tile.TileMaxSize;
            Position.Y = Program.Game.Dimension[Program.Game.CurrentDimension].MapHeight[x] * Tile.TileMaxSize - Tile.TileMaxSize * 3;
        }
        void Move()
        {
            Vector2 vec = new Vector2(_gravityDirection.X, _gravityDirection.Y);
            if (_tilePos.X >= 0)
                vec = Tools.GetDir(_tilePos.ToPoint(), Position.ToPoint());
            const float SPEED_ACC = RUN_ACCELERATION;
            float speedMax = _maxRunSpeed;
            const float NEW_JUMP_SPEED = JUMP_SPEED;
            const int NEW_JUMP_HEIGHT = JUMP_HEIGHT;
            /*for(int i = 0;i<3;i++){
                speedAcc*=slot[slotmax+i].getSpeedModdif();
                maxRunSpeed*=slot[slotmax+i].getSpeedModdif();
                newJumpSpeed *= slot[slotmax + i].getSpeedModdif();
                newJumpHeight = (int)(newJumpHeight* slot[slotmax + i].getSpeedModdif());
            }*/
            HorisontalMove(SPEED_ACC, speedMax);
            float a = VerticalMove(vec, NEW_JUMP_SPEED, NEW_JUMP_HEIGHT);
            Velocity = Colision.TileCollision(this, Position, Velocity, Width, Height, _keyState.IsKeyDown(Keys.S));
            Position += Velocity;
            if (Velocity.X != 0 && ItemAnimation == 0)
            {
                ItemAnimation = 1;
            }
            if (ItemAnimation != 0)
            {
                if (_deltaAnimation >= 1 / 2.0f)
                {
                    if (ItemAnimation < 3)
                    {
                        ItemAnimation++;
                    }
                    else { ItemAnimation = 0; }
                    _deltaAnimation = 0;
                }
                else _deltaAnimation += Program.Game.Seconds;
            }
            if (Position.X < 0)
            {
                Position.X = 0;
            }
            if (Position.X > (SizeGeneratior.WorldWidth - 1) * Tile.TileMaxSize)
            {
                Position.X = (SizeGeneratior.WorldWidth - 1) * Tile.TileMaxSize;
            }
            if (Position.Y < 0)
            {
                Position.Y = 0;
            }
            if (Position.Y > (SizeGeneratior.WorldHeight - 1) * Tile.TileMaxSize)
            {
                Position.Y = (SizeGeneratior.WorldHeight - 1) * Tile.TileMaxSize;
            }
            if (Velocity.Y < a && a > MaxFallSpeed - 5) { Bloods[0] = Bloods[1] = true; }
        }

        float VerticalMove(Vector2 vec, float newJumpSpeed, int newJumpHeight)
        {
            if (_controlJump)
            {
                if (Jump > 0)
                {
                    if (Velocity.Y > -newJumpSpeed + Gravity * 2f)
                    {
                        Jump = 0;
                    }
                    else
                    {
                        Velocity.Y = -newJumpSpeed;
                        Jump--;
                    }
                }
                else if (Math.Abs(Velocity.Y) < float.Epsilon && ReleaseJump)
                {
                    Velocity.Y = -newJumpSpeed;
                    Jump = newJumpHeight;
                }
                ReleaseJump = false;
            }
            else
            {
                Jump = 0;
                ReleaseJump = true;
            }
            if (!IsHorisontal)
            {
                Velocity.Y = Velocity.Y + Gravity * vec.Y;

                Velocity.X += Gravity * vec.X;
            }
            else
            {
                if (_keyState.IsKeyDown(Keys.W))
                {
                    Velocity.Y = Velocity.Y - RUN_ACCELERATION;
                }
                else if (_keyState.IsKeyDown(Keys.S))
                {
                    Velocity.Y = Velocity.Y + RUN_ACCELERATION;
                }
            }
            IsHorisontal = false;
            if (Velocity.Y > MaxFallSpeed)
            {
                Velocity.Y = MaxFallSpeed;
            }
            float a = Velocity.Y;
            return a;
        }

        void HorisontalMove(float speedAcc, float speedMax)
        {
            if (_controlLeft && Velocity.X > -speedMax)
            {
                if (Velocity.X > RUN_SLOWDOWN)
                {
                    Velocity.X = Velocity.X - RUN_SLOWDOWN;
                }
                Velocity.X = Velocity.X - speedAcc;

                // ReSharper disable once InvertIf
                if (_direction == 1)
                {
                    if (ItemAnimation == 0)
                    {
                        _direction = -1;
                    }
                }
            }
            else if (_controlRight && Velocity.X < speedMax)
            {
                if (Velocity.X < -RUN_SLOWDOWN)
                {
                    Velocity.X = Velocity.X + RUN_SLOWDOWN;
                }
                Velocity.X = Velocity.X + speedAcc;
                // ReSharper disable once InvertIf
                if (_direction == -1)
                {
                    if (ItemAnimation == 0)
                    {
                        _direction = 1;
                    }
                }
            }
            else if (Math.Abs(Velocity.Y) < float.Epsilon)
            {
                if (Velocity.X > RUN_SLOWDOWN)
                {
                    Velocity.X -= RUN_SLOWDOWN;
                }
                else if (Velocity.X < -RUN_SLOWDOWN)
                {
                    Velocity.X += RUN_SLOWDOWN;
                }
                else
                {
                    Velocity.X = 0f;
                }
            }
            else if (Velocity.X > RUN_SLOWDOWN * 0.5)
            {
                Velocity.X -= RUN_SLOWDOWN * 0.5f;
            }
            else if (Velocity.X < -RUN_SLOWDOWN * 0.5)
            {
                Velocity.X += RUN_SLOWDOWN * 0.5f;
            }
            else
            {
                Velocity.X = 0f;
            }
        }

        public bool HasItem(int idItem)
        {
            for (int i = 0; i < CurrentMaxSlot; i++)
            {
                if (!Slot[i].IsEmpty && Slot[i].Id == idItem) return true;
            }
            return false;
        }
        void UpdateKey()
        {
            if (Zombie)
            {
                if (Program.Game.InScreen(Mouse.GetState().Position.ToVector2()))
                {
                    Mouse.SetPosition(Mouse.GetState().X + Program.Game.Rand.Next(-3, 3),
                                      Mouse.GetState().Y + Program.Game.Rand.Next(-3, 3) );
                }
            }
            UpdateMove();
            UpdateInventoryControl();
            UpdateFun();
            _keyState = Program.Game.KeyState;
        }

        void UpdateFun()
        {
            if ( Program.Game.KeyState.IsKeyDown(Keys.G) && !_controlG )
            {
                Bellframe = Bellframe == 0 ? 1 : 0;
                _controlG = true;
            }
            if ( Program.Game.KeyState.IsKeyDown(Keys.F) )
            {
                _tilePos.X = TileTargetX * Tile.TileMaxSize;
                _tilePos.Y = TileTargetY * Tile.TileMaxSize;
            }
            if ( Program.Game.KeyState.IsKeyUp(Keys.G) ) _controlG = false;
            if ( Program.Game.KeyState.IsKeyUp(Keys.E) && _keyState.IsKeyDown(Keys.E) ||
                 Program.Game.KeyState.IsKeyDown(Keys.E) && _keyState.IsKeyUp(Keys.E) )
            {
                _gravityDirection *= -1;
            }
            Program.Game.Camera.LerpZoom(Program.Game.KeyState.IsKeyDown(Keys.E) ? 0.5f : 1.0f , Program.Game.Seconds);
            if ( Program.Game.KeyState.IsKeyDown(Keys.F) )
            {
                if ( !Keydownf )
                {
                    Keydownf = true;
                }
            }
            else
            {
                Keydownf = false;
            }
        }

        void UpdateInventoryControl()
        {
            if ( Program.Game.KeyState.IsKeyDown((Keys) GameInput.ActiveIventory) )
            {
                Isj = true;
            }
            if ( Program.Game.KeyState.IsKeyUp((Keys) GameInput.ActiveIventory) )
            {
                if ( Isj )
                {
                    ControlJ = !ControlJ;
                    Isj = false;
                    Program.Game.Console.AddLog(ControlJ.ToString());
                }
            }
            if ( Program.Game.KeyState.IsKeyDown(Keys.D1) )
                SelectedItem = 0;
            if ( Program.Game.KeyState.IsKeyDown(Keys.D2) )
                SelectedItem = 1;
            if ( Program.Game.KeyState.IsKeyDown(Keys.D3) )
                SelectedItem = 2;
            if ( Program.Game.KeyState.IsKeyDown(Keys.D4) )
                SelectedItem = 3;
            if ( Program.Game.KeyState.IsKeyDown(Keys.D5) )
                SelectedItem = 4;
            if ( Program.Game.KeyState.IsKeyDown(Keys.D6) )
                SelectedItem = 5;
            if ( Program.Game.KeyState.IsKeyDown(Keys.D7) )
                SelectedItem = 6;
            if ( Program.Game.KeyState.IsKeyDown(Keys.D8) )
                SelectedItem = 7;
            if ( Program.Game.KeyState.IsKeyDown(Keys.D9) )
                SelectedItem = 8;

            if ( Program.Game.KeyState.IsKeyDown((Keys) GameInput.Drop) && !Slot[SelectedItem].IsEmpty )
                DropPlayerThing(SelectedItem);
        }

        void UpdateMove()
        {
            _controlLeft = false;
            _controlRight = false;
            _controlJump = false;
            if ( Program.Game.KeyState.IsKeyDown((Keys) GameInput.MoveLeft) )
            {
                _controlLeft = true;
            }
            if ( Program.Game.KeyState.IsKeyDown((Keys) GameInput.MoveRight) )
            {
                _controlRight = true;
            }
            if ( Program.Game.KeyState.IsKeyDown((Keys) GameInput.Jump) && (!Bloods[0] || !Bloods[1]) )
            {
                _controlJump = true;
            }
        }

        // ReSharper disable once UnusedMember.Local
        void Angry()
        {

            if (Bloods[0] && Bloods[1]) _maxRunSpeed = 1;
            else if (Bloods[0] || Bloods[1]) _maxRunSpeed = 2;
            if (Temperature > 40) _maxRunSpeed -= 0.5f;
        }

        public void KillNpc(bool isnpc = false)
        {
            if (isnpc)
            {
                Zombie npc = new Zombie(Position, new Race(GetColor(), null), Clslot, Colors);
                npc.Drop.Clear();
                for (int i = 0; i < Slotmax + 3; i++)
                {
                    if (!Slot[i].IsEmpty)
                    {
                        npc.Drop.Add(Slot[i]);
                    }

                }
                Program.Game.Dimension[Program.Game.CurrentDimension].Zombies.Add(npc);
            }
            else
            {

                for (int i = 0; i < Slotmax + 3; i++)
                {
                    if (!Slot[i].IsEmpty)
                    {
                        Program.Game.AddDrop((int)Position.X, (int)Position.Y, Slot[i]);
                    }

                }
            }

            Spawn();
        }

        public void Update(float delta, float seconds)
        {
            if (Quests.Count >= 1) QuestUpdate();
            if (Program.Game.Dimension[Program.Game.CurrentDimension].Civil.Count >= 1) Dialog();
            UpdateKey();
            foreach (Bind bind in GameInput.BindKey)
            {
                bind.Update();
            }
            Move();
            foreach (FlashText ft in _fTlist)
            {
                ft.Update();
                // ReSharper disable once InvertIf
                if (ft.IsInvisible())
                {
                    _fTlist.Remove(ft);
                    break;
                }
            }
            UpdateHealth(delta, seconds);
            UpdateMouse();
        }

        void UpdateMouse()
        {
            Rect.X = (int)Position.X;
            Rect.Y = (int)Position.Y;
            TileTargetX = (int)Tools.MouseToCamera(Program.Game.MouseState.Position.ToVector2()).X / Tile.TileMaxSize;
            TileTargetY = (int)Tools.MouseToCamera(Program.Game.MouseState.Position.ToVector2()).Y / Tile.TileMaxSize;
            MathHelper.Clamp(SelectedItem + Program.Game.MouseState.ScrollWheelValue - _mouseState.ScrollWheelValue, 0, CurrentMaxSlot - 1);
            if (_timeSlotUse > 0) _timeSlotUse -= 1;
            if (Tools.MouseInCube(0, 0, Program.Game.Resolution.X, Program.Game.Resolution.Y))
            {
                if (GameInput.MouseButtonIsDown(GameInput.MouseButton.LEFT_BUTTON))
                {
                    int r = -1;
                    if (_itemframe >= 1) _itemto = true;
                    else if (_itemframe < 0) _itemto = false;
                    if (_itemto) _itemframe -= 10f * Program.Game.Seconds;
                    else _itemframe += 10f * Program.Game.Seconds;
                    if (!DamageNpc())
                    {
                        for (int i = 0; i < CurrentMaxSlot; i++)
                        {
                            if (!ControlJ && i > 9) break;
                            if (i % 9 == 0) r++;
                            if (Slot[i].IsEmpty) continue;

                            if (Tools.MouseInCube(i % 9 * 32, 32 * r, 32, 32))
                            {
                                SelectedItem = i;
                            }
                        }
                    }
                }
                else if (GameInput.MouseButtonIsPressed((int)GameInput.MouseButton.LEFT_BUTTON))
                {
                    if (_itemframe >= 1) _itemto = true;
                    else if (_itemframe < 0) _itemto = false;
                    if (_itemto) _itemframe -= 10f * Program.Game.Seconds;
                    else _itemframe += 10f * Program.Game.Seconds;
                    if (!UpdateRecept() && !Bloods[2])
                    {
                        if (Tools.Distance((int)Position.X / Tile.TileMaxSize, TileTargetX, 7) &&
                        Tools.Distance((int)Position.Y / Tile.TileMaxSize, TileTargetY, 7) &&
                        Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].Active)
                        {
                            DestoryBlock(TileTargetX, TileTargetY);
                        }
                        DamageNpc();
                    }

                }
                else if (GameInput.MouseButtonIsDown((int)GameInput.MouseButton.RIGHT_BUTTON))
                {
                    if (!MouseItem.IsEmpty)
                    {
                        DropPlayerThing(ref MouseItem);
                    }
                }
                else if (GameInput.MouseButtonIsPressed((int)GameInput.MouseButton.RIGHT_BUTTON))
                {
                    if (ChatNpc())
                    {
                        _timeSlotUse = 60;
                    }
                    bool drag = false;
                    DragAndDrop(ref drag);
                    if (!Slot[SelectedItem].IsEmpty && !Bloods[3])
                    {
                        if (_timeSlotUse <= 0)
                            UseSlot(SelectedItem);
                        if (_timeSlotUse <= 0 && Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].Active &&
                            UseBlock(Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY]))
                        {
                            _timeSlotUse = 50;
                        }

                    }
                }
                if (GameInput.MouseButtonIsReleased((int)GameInput.MouseButton.LEFT_BUTTON) || GameInput.MouseButtonIsReleased((int)GameInput.MouseButton.RIGHT_BUTTON))
                {
                    _itemto = false;
                    _itemframe = 0;
                }
            }
            _mouseState = Program.Game.MouseState;
        }
        float GetMassFactor() => Slot[Slotmax].GetMassFactor() + Slot[Slotmax + 1].GetMassFactor() + Slot[Slotmax + 2].GetMassFactor();

        void UpdateHealth(float delta, float seconds)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int partId = 0; partId < Bloods.Length; partId++)
            {
                if (Bloods[partId])
                    Blood -= 100 * delta;
            }

            Drought = Math.Max(0, Drought - (25 + 10 * GetMassFactor()) * delta);

            Hunger = Math.Max(0, Hunger - 25 * delta);

            if (Zombie) Temperature += 1 * seconds;
            if (Blood <= 4000 || Hunger < 10 || Drought < 10 || Temperature >= 45)
                TypeKill = 1;
        }

        void DestoryBlock(int blockX, int blockY)
        {
            Removeblock();
        }

        void DropPlayerThing(int dropslot)
        {
            float dirx = 0;
            if (TileTargetX < Position.X / 16) dirx = 1;
            if (TileTargetX > Position.X / 16) dirx = -1;
            Program.Game.AddDrop((int)Position.X + (int)(-dirx * Width), (int)Position.Y, Slot[dropslot], dirx);
            Slot[dropslot] = new Item();
        }

        void DropPlayerThing(ref Item dropslot)
        {
            float dirx = 0;
            if (TileTargetX < Position.X / 16) dirx = 1;
            if (TileTargetX > Position.X / 16) dirx = -1;
            Program.Game.AddDrop((int)Position.X + (int)(-dirx * Width), (int)Position.Y, dropslot, dirx);
            dropslot = new Item();
        }

        void DragAndDrop(ref bool t)
        {
            if (ControlJ)
            {
                t = DragItemSlot(t, CurrentMaxSlot);
                for (int i = Slotmax; i < Slotmax + 3; i++)
                {
                    if ( !Tools.MouseInCube((i - Slotmax) * 32 , Program.Game.Resolution.Y - 32 ,32 , 32) ) continue;

                    if (MouseItem.GetArmorSlot() - 1 == i - Slotmax)
                    {
                        t = MouseItemSlot(t, ref Slot[i]);
                    }
                    t = true;
                    break;
                }
            }
            else t = DragItemSlot(t, 9);
        }
        void PickItem(ref Item i)
        {
            MouseItem = new Item(i.Ammount , i.Id) {Hp = i.Hp};
            i = new Item();
        }
        bool DragItemSlot(bool t,int maxSlot)
        {
            int r = -1;
            int centerSlot = Program.Game.Resolution.X - 9 * 32;
            for (int i = 0; i < maxSlot; i++)
            {

                if (i % 9 == 0) r++;
                int xslot = i % 9 * 32;
                int yslot = 32 * r;
                if ( !Tools.MouseInCube(xslot + centerSlot , yslot , 32 , 32) ) continue;

                t = MouseItemSlot(t, ref Slot[i]);
                break;
            }
            return t;
        }

         bool MouseItemSlot(bool t, ref Item item)
        {
            if (!item.IsEmpty && MouseItem.IsEmpty)
            {
                t = true;
                PickItem(ref item);
            }
            else if (!MouseItem.IsEmpty)
            {
                t = true;
                SlotChange(ref item, ref MouseItem);
                MouseItem = new Item();
            }
            return t;
        }

        static void SlotChange(ref Item item1,ref Item item2)
        {
            Item temp = new Item(item1.Ammount , item1.Id) {Hp = item1.Hp};
            item1 = new Item(item2.Ammount , item2.Id) {Hp = item2.Hp};
            item2 = new Item(temp.Ammount , temp.Id) {Hp = temp.Hp};

        }
        public void TeleportToShowMap()
        {
            _stats.SetStats(Stats.StatsType.TELEPORT_TO_OTHER_WORLD,_stats.Getstats(Stats.StatsType.TELEPORT_TO_OTHER_WORLD,0)+1);
            Program.Game.CurrentDimension = 2;
            Teleport();
        }
        bool UseBlock(Tile title) => title.Blockuse(TileTargetX,TileTargetY,this);
        void UseSlot(int selecteditem)
        {

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Slot[SelectedItem].GetTypeItem())
            {
                case (int) Item.Type.EAT when Drought < 100:
                {

                    const int HEAL = 10;

                    Drought = Math.Min(100 , Drought + HEAL);
                    RemoveItemCurrentSlot();
                    _timeSlotUse = 10;
                    break;
                }

                case (int) Item.Type.WATER when Hunger < 100:
                {
                    const int HEAL = 10;
                    Hunger = Math.Min(100 , Hunger + HEAL);
                    _timeSlotUse = 10;
                    RemoveItemCurrentSlot();
                    break;
                }
                case (int) Item.Type.BANDAGE when Bloods.Any(val => val):
                {
                    int a = 0;
                    while ( !Bloods[a] )
                    {
                        a = Program.Game.Rand.Next(0 , 4);
                    }

                    Bloods[a] = false;
                    RemoveItemCurrentSlot();
                    break;
                }
                case (int)Item.Type.HEAD:
                case (int)Item.Type.BODY:
                case (int)Item.Type.LEGS:
                    {
                        Item sl = new Item();
                        int slotId = Slotmax + (Slot[selecteditem].GetTypeItem() - (int)Item.Type.HEAD);
                        if (!Slot[slotId].IsEmpty) sl = Slot[slotId];
                        Slot[slotId] = Slot[selecteditem];
                        Slot[selecteditem] = sl;
                        break;
                    }
                case (int)Item.Type.WEAPONGUN :
                    {
                        int slotIds = GetSlotItem(1);
                        if (slotIds == -1) return;

                        _timeSlotUse = 10;
                        Program.Game.Bullets.Add(new Bullet(Position, Tools.Distance(Position)));
                        RemoveItemSlot(slotIds);
                    }

                    break;
                case (int)Item.Type.BLOCKICON:
                case (int)Item.Type.VERTICALBLOCKICON:
                    {
                        if (Tools.Distance((int)Position.X / Tile.TileMaxSize, TileTargetX, 7) &&
                             Tools.Distance((int)Position.Y / Tile.TileMaxSize, TileTargetY, 7))
                        {
                            AddBlock();
                        }
                        break;
                    }
            }
        }

        public void AddLog(string text)
        {
            XnaLayout newgui = new XnaLayout();
            Button button = new Button(Program.Game.Black, Program.Game.Font1, new Rectangle(Program.Game.Resolution.X / 2 - 500 / 2,
                Program.Game.Resolution.Y - 100 - Program.Game.Guis.Count * 60 + 40, 500, 20), "Close");
            Label lab = new Label(text, new Vector2(Program.Game.Resolution.X / 2 - 500 / 2,
                                                    Program.Game.Resolution.Y - 100 - Program.Game.Guis.Count * 60), Program.Game.Font1);
            Image image = new Image(Program.Game.Achivement, new Rectangle(Program.Game.Resolution.X / 2 - 500 / 2,
                                                                           Program.Game.Resolution.Y - 100 - Program.Game.Guis.Count * 60, 500, 60));
            newgui.AddButon(button);
            newgui.AddLabel(lab);
            newgui.AddImage(image);
            newgui.AddClicked((sender , e) => Program.Game.RemoveGui(newgui), 0);
            Program.Game.AddGui(newgui);
        }
        
        public bool AddHorisontalBlock()
        {
            if ( Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX - 1 , TileTargetY].Active )
                return false;

            Program.Game.Dimension[Program.Game.CurrentDimension].SetTexture(TileTargetX, TileTargetY, Slot[SelectedItem].GetBlockId());
            Program.Game.Dimension[Program.Game.CurrentDimension].SetTexture(TileTargetX - 1, TileTargetY, Slot[SelectedItem].GetBlockId() + 1);
            Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].Hp = Math.Max(1, Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].GetBlockHp() * (Slot[SelectedItem].Hp / 100));
            RemoveItemCurrentSlot();
            return true;
        }
        
        public bool AddVerticallBlock()
        {
            if ( Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX , TileTargetY - 1].Active )
                return false;

            Program.Game.Dimension[Program.Game.CurrentDimension].SetTexture(TileTargetX, TileTargetY, Slot[SelectedItem].GetBlockId());
            Program.Game.Dimension[Program.Game.CurrentDimension].SetTexture(TileTargetX, TileTargetY - 1, Slot[SelectedItem].GetBlockId() + 1);
            Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].Hp = Math.Max(1, Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].GetBlockHp() * (Slot[SelectedItem].Hp / 100));
            RemoveItemCurrentSlot();
            return true;
        }
        
        void AddBlock()
        {
            if ( Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX , TileTargetY].Active 
                || !Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX , TileTargetY].AddedBlock(Slot[SelectedItem].GetBlockId() , TileTargetX , TileTargetY) ) return;

            //if (Block.isLightBlock(slot[selectedItem].Idblock)) type = ITile.TYPE.NO_SOILD_BLOCK; //TODO
            if (Slot[SelectedItem].GetTypeItem() == (int)Item.Type.VERTICALBLOCKICON)
            {
                if (AddHorisontalBlock())
                {
                    _achivement.Complete(Achivement.AchivementType.MODIFED_TERRAIN);
                }
            }
            else
            {
                Program.Game.Dimension[Program.Game.CurrentDimension].SetTexture(TileTargetX, TileTargetY, Slot[SelectedItem].GetBlockId());
                _achivement.Complete(Achivement.AchivementType.MODIFED_TERRAIN);
            }
        }
        
        void RemoveItemSlot(int slotId)
        {

            Slot[slotId].Ammount--;
            if (Slot[slotId].Ammount <= 0)
            {
                Slot[slotId] = new Item();
            }
        }
        
        void RemoveItemCurrentSlot()
        {
            RemoveItemSlot(SelectedItem);
        }
        
        public void SetSlot(Item slots)
        {
            int ammout = slots.Ammount;
            for (int j = 0; j < ammout; j++)
            {
                int i = GetSlotItem(slots.Id, slots.Hp);//pos.x = 5;tiletar=6; 5-6=-1 
                if (i != -1)
                {
                    Slot[i].Ammount += 1;
                    slots.Ammount -= 1;
                }
                else
                {
                    Slot[GetSlotFull()] = slots;
                    break;
                }
            }
        }
        
        public void GetDrop(int tileX, int tileY)
        {


            List<Item> itemDrop = Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[tileX, tileY].Destory(tileX,tileY,this);
            foreach (Item bs in itemDrop)
            {
                Program.Game.AddDrop(tileX * 16 + Program.Game.Dimension[Program.Game.CurrentDimension].Rand.Next(-8, 8), tileY * 16, bs);
            }
        }
        
        
        // ReSharper disable once UnusedMethodReturnValue.Local
        bool Removeblock()
        {
            if ( !Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX , TileTargetY]
                         .IsNeadTool(Slot[SelectedItem]) ) return false;

            Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].DamageSlot(
                                                                                                   Slot[SelectedItem].GetPower() * (Slot[SelectedItem].Hp / 100) * Program.Game.Seconds,new Vector2(TileTargetX*Tile.TileMaxSize, TileTargetY * Tile.TileMaxSize));
            Slot[SelectedItem].DamageSlot(Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].GetBlockHp()
                                          * (Program.Game.Seconds / 2));

            // ReSharper disable once InvertIf
            if (Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[TileTargetX, TileTargetY].Hp <= 0)
            {
                _achivement.Complete(Achivement.AchivementType.MODIFED_TERRAIN);
                GetDrop(TileTargetX, TileTargetY);
            }
            return true;
        }
        
        bool ChatNpc()
        {
            foreach ( Civilian npc in Program.Game.Dimension[Program.Game.CurrentDimension].Civil )
            {
                if ( !Tools.ScMouseInCube((int) npc.Position.X ,
                                          (int) npc.Position.Y , npc.Width , npc.Height) ||
                     !Tools.Distance((int) Position.X , (int) npc.Position.X , 32 + 8) ||
                     !Tools.Distance((int) Position.Y , (int) npc.Position.Y , 32 + 8) ) continue;

                _achivement.Complete(Achivement.AchivementType.TALKING_NPC);
                npc.ToggleEnabled();
                return true;
            }

            return false;
        }
        bool DamageNpc(BaseNpc npc)
        {
            Vector2 posEnemy = new Vector2((int)npc.Position.X, (int)npc.Position.Y);
            //System.Console.WriteLine(posEnemy.ToString());

            // ReSharper disable once InvertIf
            if (Tools.ScMouseInCube((int)posEnemy.X, (int)posEnemy.Y, npc.Width, npc.Height) &&
            Tools.Distance((int)Position.X, (int)npc.Position.X, 32 + 8) &&
            Tools.Distance((int)Position.Y, (int)npc.Position.Y, 32 + 8))
            {
                float hpdamage = Slot[SelectedItem].GetDamage() * (Slot[SelectedItem].Hp / 100) * Program.Game.Seconds;
                FlashText ft = new FlashText(new Vector2(posEnemy.X, posEnemy.Y - 20), hpdamage.ToString(CultureInfo.CurrentCulture), Program.Game.Font1, Color.Red);
                npc.Hp -= hpdamage;
                _fTlist.Add(ft);
                Slot[SelectedItem].DamageSlot(1 * Program.Game.Seconds);
                Vector2 dir = npc.Position - Position;
                dir.Normalize();
                npc.AddForce(dir, hpdamage * 10);

                if ((int)npc.Hp <= 1)
                {
                    if (Program.Game.Dimension[Program.Game.CurrentDimension].Civil.Count >= 1)
                    {
                        Civilian civ = Program.Game.Dimension[Program.Game.CurrentDimension].Civil[0];
                        if (civ.Activedialog == 1) { _zombiekill = true; }
                    }
                    Program.Game.Console.AddLog("NPC DROP:" + npc.Drop.Count);
               }
            }
            return false;
        }
        public void DamageHead()
        {
            if (!Slot[Slotmax].IsEmpty)
            {
                DamageArmor(0);
            }
            else TypeKill = 0;
        }
        public void DamageArmor(int armorId)
        {
            Slot[Slotmax+ armorId].DamageSlot(Math.Max(1, 10 - Slot[Slotmax + armorId].GetDef()));
            if (Slot[Slotmax + armorId].Hp < 2) Slot[Slotmax + armorId] = new Item();
        }
        bool DamageNpc()
        {
            if ( Bloods[2] ) return false;

            if ( Program.Game.Dimension[Program.Game.CurrentDimension].Zombies.Count < 1 ) return false;

            if ( Program.Game.Dimension[Program.Game.CurrentDimension].Zombies.Any(DamageNpc) )
                return true;

            return Program.Game.Dimension[Program.Game.CurrentDimension] is NormalWorld world && world.Npcs.Any(DamageNpc);
        }
        
        bool UpdateRecept()
        {
            if ( !ControlJ ) return false;

            int a = 0;
            for (int i = 0; i < Recipe.Recipes.Count && a<=9; i++)
            {
                if ( !GetValidRecipes(i) ) continue;

                if (Tools.MouseInCube(a * 32, 35 + 32 * -1, 32, 32))
                {
                    RemoveRecipes(i);
                    _stats.AddRecipe(Recipe.Recipes[i].Slot.Id, Recipe.Recipes[i].Slot.Ammount);
                    GiveItem(Recipe.Recipes[i].Slot);

                    return true;
                }
                a++;
            }

            return false;
        }

        void GiveItem(Item item)
        {
            int i2 = GetSlotItem(item.Id, 100);//pos.x = 5;tiletar=6; 5-6=-1 
            if (i2 != -1)
            {
                Slot[i2].Ammount += item.Ammount;
                Slot[i2].IsEmpty = false;
            }
            else
            {
                i2 = GetSlotFull();
                Slot[i2] = item;
                Slot[i2].IsEmpty = false;
            }
        }

        public void ActiveSpecial(int p)
        {
            _special = p;
        }

        #endregion
    }
}
