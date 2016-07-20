using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using TwoSides.Physics.Entity.NPC;
using TwoSides.World;
using TwoSides.GUI;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.GameContent.Entity;
using TwoSides.World.Tile;
using TwoSides.Utils;
using TwoSides.ModLoader;
using System.IO;
using TwoSides.GameContent.Tiles;
using System.Collections.Generic;
using TwoSides.GameContent.Dimensions;
namespace TwoSides.Physics.Entity
{
    [Serializable]
    sealed public class Achivement{
        const int AchivementCount = 2;
        public enum ACHIVEMENT
        {
            ModifedTerrain = 0,
            TalkingNPC,
        }
        bool[] Achivemets= new bool[AchivementCount];
        string[] Desc = new string[AchivementCount];
        string[] Name = new string[AchivementCount];

        public Achivement(){
            Desc[0] = "Modifed Terrain";
            Desc[1] = "Tallking NPC";
            Name[0] = "God";
            Name[1] = "Communicative";
            for (int i = 0; i < AchivementCount; i++)
            {
                Achivemets[i] = false;
            }
        }
        public void Complete(ACHIVEMENT  id)
        {
            if (!this.Achivemets[(int)id])
            {
                Program.game.player.AddLog("Achivement Complete:\n" + Name[(int)id] + "\n" + Desc[(int)id]); // 0		
            }
            Achivemets[(int)id] = true;
        }
    }

    [Serializable]
    sealed public class Stats
    {
        
        public int Translition;
        public int KillBossUltraNPC;

        int[] iditems = new int[Item.itemmax + Clothes.maxArmor];
        
        public Stats()
        {
            for (int i = 0; i < Item.itemmax + Clothes.maxArmor; i++)
            {
                iditems[i] = new int();
            }
            Clear();
        }
        
        public void Clear()
        {
            for (int i = 0; i < Item.itemmax + Clothes.maxArmor; i++)
            {
                iditems[i] = 0;
            }
        }
        
        public void AddRecipe(int iditem, int ammount){
            if (iditem < ModLoader.ModLoader.StartIdItem)
            this.iditems[iditem] += ammount;
        }
        public enum STATS
        {
            ItemCount,
            KillBoss,
            TeleportToOtherWorld,

        };
        public int Getstats(STATS type, int value)
        {
            if (type == STATS.ItemCount) { return this.iditems[value]; }
            else if (type == STATS.KillBoss)
            {
              // Program.game.console.addlog(type + " " + KillBossUltraNPC);
                return this.KillBossUltraNPC;
            }
            else
            {
               Program.game.console.addLog(type + " " + this.Translition);
                return this.Translition;
            }
        }
    }
    
    public class Player : CEntity
    {
        
        public const int slotmax = 9 * 5;

        ArrayList FTlist = new ArrayList();
        public ArrayList Quests = new ArrayList();

        public bool[] bloods = new bool[4];
        public Item[] slot = new Item[slotmax + 3];
        public Color[] colors = new Color[5];
        public Clothes[] clslot = new Clothes[6];

        Achivement achivement = new Achivement();
        MouseState ms;
        Color crace;
        Quest quest;
        Stats stats;
        [NonSerialized]
        public Rectangle rect;
        public Quest quest2 { get; set; }
        public Quest quest4 { get; set; }
        public Quest quest5 { get; set; }
        public Quest quest3 { get; set; }

        bool controlLeft;
        bool controlRight;
        bool controlG;
        bool controlJump;
        bool zombiekill;
        bool itemto = false;
        int special = -1;
        int direction = 1;
        float itemframe = 0;
        float animationfog = 0;

        int jumpHeight = 15;
        float delta;
        float maxRunSpeed = 3f;
        float runAcceleration = 0.08f;
        float runSlowdown = 0.2f;
        float jumpSpeed = 5.01f;
        protected new float maxFallSpeed = 20f;

        public bool controlJ;
        public bool keydownf = false;
        public bool isj;
        public bool releaseJump;
        public bool zombie = false;
        public bool speedup;
        public bool isopen = false;
        public float Temperature = 36;
        public float blood;
        public float hunger;
        public float drought;

        public int width = 20;
        public int height = 42;
        public int itemAnimation;
        public int typeKill = -1;
        public int jump;
        public int selectedItem { get; private set; }
        public int tileTargetX;
        public int tileTargetY;
        public int carma;
        public int bellframe = 0;
        public Item MouseItem;//исправить
        public string Name = "";
        public override void load(BinaryReader reader)
        {
            base.load(reader);
            for (int i = 0; i < 6; i++)
            {
                clslot[i] = new Clothes();
                clslot[i].load(reader);
            }
            for (int i = 0; i < slotmax + 3; i++)
            {
                slot[i] = new Item();
                slot[i].load(reader);
            }
            for (int i = 0; i < 5; i++)
            {
               colors[i] =Util.readColor( reader);
            }
            Name = reader.ReadString();
            crace = Util.readColor(reader);
        }
        public override void save(BinaryWriter writer)
        {
            base.save(writer);
            for (int i = 0; i < 6; i++)
            {
                clslot[i].save(writer);
            }
            for (int i = 0; i < slotmax + 3; i++)
            {
                slot[i].save(writer);
            }
            for (int i = 0; i < 5; i++)
            {
                Util.SaveColor(colors[i], writer);
            }
            writer.Write(Name);
            Util.SaveColor(crace, writer);
        }
        public Player()
        {
            stats = new Stats();
            for (int i = 0; i < slotmax + 3; i++)
            {
                slot[i] = new Item();
            }
            for (int i = 0; i < 5; i++)
            {
                colors[i] = Color.Red;
            }
            rect.Width = width;
            zombiekill = false;
            rect.Height = height;
        }

        public void bosskil()
        {
            stats.KillBossUltraNPC++;
        }
      
        public int gefdef()
        {
            return slot[slotmax].getDef() + slot[slotmax + 1].getDef() + slot[slotmax + 2].getDef();
        }
        public float getX()
        {
            return position.X;
        }
        public float getY()
        {
            return position.Y;
        }
        public float getXLocal()
        {
            return position.X/ITile.TileMaxSize;
        }
        public float getYLocal()
        {
            return position.Y / ITile.TileMaxSize;
        }
        private void removerecipes(int id)
        {
            for (int i = 0; i < ((Recipe)Program.game.recipes[id]).getsize(); i++)
            {

                int slotids = this.getslotitem(((Recipe)Program.game.recipes[id]).getigr(i), ((Recipe)Program.game.recipes[id]).hp, ((Recipe)Program.game.recipes[id]).getconut(i));
                if (slotids != -1)
                {

                    slot[slotids].ammount -= ((Recipe)Program.game.recipes[id]).getconut(i);

                    if (slot[slotids].ammount <= 0)
                    {
                        slot[slotids] = new Item();
                        slot[slotids].IsEmpty = true;
                    }

                }
            }
        }
        
        public bool getvalidrecipes(int id)
        {
            bool t = false;
            for (int i = 0; i < ((Recipe)Program.game.recipes[id]).getsize(); i++)
            {
                int slotids = this.getslotitem(((Recipe)Program.game.recipes[id]).getigr(i), ((Recipe)Program.game.recipes[id]).hp, ((Recipe)Program.game.recipes[id]).getconut(i));

                if (slotids != -1)
                {
                    t = true;
                }
                else { t = false; break; }
            }
            if (t)
            {
                if (!((Recipe)Program.game.recipes[id]).isblock) return true;
                for (int xi = -3; xi <= 3; xi++)
                {
                    for (int yi = -5; yi <= 5; yi++)
                    {
                        int newx = (int)(position.X / ITile.TileMaxSize) + xi;
                        int newy = (int)(position.Y / ITile.TileMaxSize) + yi;
                        if (newx >= SizeGeneratior.WorldWidth ||
                            newx < 0) continue;
                        if (newy >= SizeGeneratior.WorldHeight ||
                            newy < 0) continue;
                        if (Program.game.dimension[Program.game.currentD].map[newx, newy].idtexture == ((Recipe)Program.game.recipes[id]).idblock) return true;
                    }
                }
            }
            return false;
        }
        
        public void setCR(int id)
        {
            if (Race.racelist.Count <= 0)
            {
                if (id == 2) crace = new Color(81, 21, 21);
                if (id == 1) crace = new Color(213, 171, 123);
            }

            else
            {
                Race race = (Race)Race.racelist[id - 1];
                crace = race.getColor();
            }
        }
        
        public void setUserColor(Color clrace)
        {
            crace = clrace;
        }
        
        public Color getcolor()
        {
            return crace;
        }

        void dialog()
        {

            Civilian civ = (Civilian)(Program.game.dimension[0].civil[0]);
            if (civ.activedialog == 0)
            {
                jobDialog(civ, quest, 1);
            }
            else if (civ.activedialog == 1)
            {
                int questid = Quests.IndexOf(quest);
                Quest questg = (Quest)Quests[questid];
                if (zombiekill) ((Quest)Quests[questid]).SetCompliction(0);
                PassedQuestDialog(civ, questid, 2, questg);
            }
            else if (civ.activedialog == 2)
            {
                jobDialog(civ, quest2, 3);
            }
            else if (civ.activedialog == 3)
            {
                int questid = Quests.IndexOf(quest2);
                Quest questg = (Quest)Quests[questid];

                PassedQuestDialog(civ, questid, 4, questg);
            }
            else if (civ.activedialog == 4)
            {
                jobDialog(civ,quest3,5);
            }
            else if (civ.activedialog == 5)
            {
                int questid = Quests.IndexOf(quest3);
                Quest questg = (Quest)Quests[questid];
                if (stats.Getstats(Stats.STATS.ItemCount, 19) >= 2) ((Quest)Quests[questid]).SetCompliction(0);
                if (stats.Getstats(Stats.STATS.KillBoss, 8) >= 1) ((Quest)Quests[questid]).SetCompliction(1);
                PassedQuestDialog(civ, questid, 6, questg);
            }
            else if (civ.activedialog == 6)
            {
                jobDialog(civ, quest4, 7);
            }
            else if (civ.activedialog == 7)
            {
                int questid = Quests.IndexOf(quest4);
                Quest questg = (Quest)Quests[questid];
                if (stats.Getstats(0, 22) >= 1) ((Quest)Quests[questid]).SetCompliction(1);
                if (stats.Getstats(0, 23) >= 1) ((Quest)Quests[questid]).SetCompliction(2);
                if (stats.Getstats(0, 24) >= 1) ((Quest)Quests[questid]).SetCompliction(3);
                PassedQuestDialog(civ, questid,8,questg);
            }
            else if (civ.activedialog == 8)
            {
                jobDialog(civ, quest5, 9);
            }
            else if (civ.activedialog == 9)
            {
                if (civ.isbtnclicked(1) || civ.isbtnclicked(3))
                {
                    civ.changeenable();
                }
                else if (civ.isbtnclicked(0) || civ.isbtnclicked(2))
                {
                    if (civ.isbtnclicked(0)) carma = Math.Max(carma - 2, 0);
                    civ.changeenable();
                }
            }
        }

        private void PassedQuestDialog(Civilian civ, int questid,int newDialog,Quest currentQuest)
        {
            if (currentQuest.isPassed())
            {
                civ.changevisible(1, true);
                civ.changevisible(3, true);
            }
            else
            {
                civ.changevisible(1, false);
                civ.changevisible(3, false);
            }
            if (civ.isbtnclicked(1) || civ.isbtnclicked(3))
            {
                if (civ.isbtnclicked(1)) carma = Math.Max(carma - 1, 0);
                quest.Passed();
                Quests.RemoveAt(questid);
                civ.activedialog = newDialog;
            }
            else if (civ.isbtnclicked(0) || civ.isbtnclicked(2))
            {
                if (civ.isbtnclicked(0)) carma = Math.Max(carma - 2, 0);
                civ.changeenable();
            }
        }

        private void jobDialog(Civilian civ,Quest questGive,int newDialog)
        {

            if (civ.isbtnclicked(1) || civ.isbtnclicked(3))
            {
                if (civ.isbtnclicked(1)) carma = Math.Max(carma - 1, 0);
                civ.activedialog = newDialog;
                Quests.Add(questGive);
            }
            else if (civ.isbtnclicked(0) || civ.isbtnclicked(2))
            {
                if (civ.isbtnclicked(0)) carma = Math.Max(carma - 2, 0);
                civ.changeenable();
            }
        }
        
        public void QuestUpdate()
        {
            int questid = Quests.IndexOf(quest5);
            if (questid >= 0)
            {
                if (stats.Getstats(Stats.STATS.KillBoss, 0) >= 1) ((Quest)Quests[questid]).SetCompliction(0);
                if (stats.Getstats(Stats.STATS.TeleportToOtherWorld, 0) >= 1)
                {
                    ((Quest)Quests[questid]).SetCompliction(1);
                }
                Quest questg = (Quest)Quests[questid];
                if (questg.isPassed())
                {
                    quest.Passed();
                    Quests.RemoveAt(questid);
                }
            }
        }
        
        public void spawn()
        {
            //  stats.Clear();
            itemAnimation = 0;
            Quests.Clear();
            ArrayList gl = new ArrayList();
            gl.Add(new Goals("Kill 1 Zombie"));
            ArrayList gl2 = new ArrayList();
            gl2.Add(new Goals("Obtain 3 stone", 1, 3, 2));
            gl2.Add(new Goals("Obtain 1 wood", 5, 1, 2));
            gl2.Add(new Goals("Obtain 1 Water Botle", 255, 1, 2));
            gl2.Add(new Goals("Obtain 1 Burger", 256, 1, 2));
            ArrayList gl3 = new ArrayList();
            gl3.Add(new Goals("Crafting 2 wand"));
            gl3.Add(new Goals("Crafting PickAxe"));
            ArrayList gl4 = new ArrayList();
            gl4.Add(new Goals("Obtain 8 stone", 1, 8, 2));
            gl4.Add(new Goals("Crafting stone helmet"));
            gl4.Add(new Goals("Crafting stone breastplate"));
            gl4.Add(new Goals("Crafting stone Shoes"));
            ArrayList gl5 = new ArrayList();
            gl5.Add(new Goals("Kill ultra zombie"));
            gl5.Add(new Goals("send the show world"));
            quest = new Quest(gl, new Item( 1, 25), "First step", Program.game.Font1);
            quest2 = new Quest(gl2, new Item(1, 11), "Gathering Resources", Program.game.Font1);
            quest3 = new Quest(gl3, new Item(1, 10), "Master", Program.game.Font1);
            quest4 = new Quest(gl4, new Item(2, 14), "Preparation", Program.game.Font1);
            quest5 = new Quest(gl5, new Item(2, 11), "First Win", Program.game.Font1);
            isopen = false;
            MouseItem = new Item();
            zombie = false;
            hunger = 100;
            drought = 100;
            position.X = 1 * ITile.TileMaxSize;
            bellframe = 0;
            position.Y = Program.game.dimension[Program.game.currentD].mapHeight[1] * ITile.TileMaxSize - ITile.TileMaxSize * 3;
            for (int i = 0; i < slotmax + 3; i++)
            {
                slot[i] = new Item();
                slot[i].IsEmpty = true;
            }

            for (int i = 0; i < 4; i++)
            {
                bloods[i] = false;
            }
            slot[0].IsEmpty = false;
            slot[0].ammount = 1;
            carma = 100;
            slot[0].iditem = 8;

            this.selectedItem = 0;
            slot[2].iditem = 1;
            slot[2].ammount = 6;
            slot[2].IsEmpty = false;
            slot[1].iditem = 6;
            slot[1].ammount = 1;
            slot[1].IsEmpty = false;
            slot[3].iditem = 25;
            slot[3].ammount = 1;
            slot[3].IsEmpty = false;
            blood = 5000;
            maxRunSpeed = 3;
            Temperature = 36.6f;
            stats.Clear();
            typeKill = -1;
            foreach (Civilian civ in Program.game.dimension[Program.game.currentD].civil)
            {
                civ.activedialog = 0;
                civ.offenable();
            }
            //slot[slotmax] = new Item(1, 31);
           // slot[slotmax+1] = new Item(1, 32);
//slot[slotmax+2] = new Item(1, 33);
            delta = 0;
        }
        
        public int getslotfull()
        {
            for (int i = 0; i < slotmax; i++)
            {
                if (slot[i].IsEmpty) return i;
            }
            return -1;
        }
        
        public int getslotitem(int iditem)
        {
            for (int i = 0; i < currentMaxSlot; i++)
            {
                if (!slot[i].IsEmpty && slot[i].iditem == iditem && slot[i].GetStack() > slot[i].ammount) return i;

            }
            return -1;
        }
        
        public int getslotitem(int iditem, int ammout)
        {
            for (int i = 0; i < currentMaxSlot; i++)
            {
                if (!slot[i].IsEmpty && slot[i].iditem == iditem && slot[i].GetStack() > slot[i].ammount && slot[i].ammount >= ammout) return i;

            }
            return -1;
        }
        
        public int getslotitem(int iditem, float hpb, bool plusravno = true)
        {
            for (int i = 0; i < currentMaxSlot; i++)
            {
                if (!slot[i].IsEmpty && slot[i].iditem == iditem && ((slot[i].HP >= hpb && plusravno) || (slot[i].HP == hpb && !plusravno)) && slot[i].GetStack() > slot[i].ammount) return i;

            }
            return -1;
        }
        
        public int getslotitem(int iditem, float hpb, int ammout, bool plusravno = true)
        {
            for (int i = 0; i < currentMaxSlot; i++)
            {
                if (!slot[i].IsEmpty && slot[i].ammount >= ammout && slot[i].iditem == iditem && ((slot[i].HP >= hpb && plusravno) || (slot[i].HP == hpb && !plusravno)) && slot[i].GetStack() > slot[i].ammount) return i;

            }
            return -1;
        }
        
        public void clearclothes()
        {
            for (int i = 0; i < 6; i++)
            {
                clslot[i] = new Clothes();
            }
        }
        
        public void renderfog(Texture2D fog, SpriteBatch spriteBatch)
        {
            if (animationfog > fog.Width - Program.game.graphics.PreferredBackBufferWidth)
            {
                animationfog = 0;
            }
            else animationfog += Program.game.seconds * ((fog.Width - Program.game.graphics.PreferredBackBufferWidth) / 60);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null);
            Rectangle dest = new Rectangle(0, 0,
            Program.game.graphics.PreferredBackBufferWidth * 2, Program.game.graphics.PreferredBackBufferHeight * 2);
            Rectangle src = new Rectangle((int)animationfog, 0, 800, 600);
            if (Program.game.currentD == 2)
            {
                if (Program.game.dimension[Program.game.currentD].mapHeight[(int)position.X / ITile.TileMaxSize] > position.Y / ITile.TileMaxSize) spriteBatch.Draw(fog, dest, src, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            } spriteBatch.End();
        }
        
        public void render( Texture2D eye, Texture2D head, Texture2D body, Texture2D legs, SpriteBatch spriteBatch,float scale = 1.0f)
        {
            SpriteEffects effect = SpriteEffects.None;
            Rectangle dest = new Rectangle(
                (int)(position.X + (width - head.Width)),
                (int)(position.Y ), (int)(head.Width*scale), (int)(head.Height*scale));
            Rectangle src = new Rectangle(0, 0,
                24, 42);
            Rectangle dest2 = new Rectangle((int)(position.X), (int)(position.Y + 20), 40, 17);
            Rectangle src2 = new Rectangle(0, itemAnimation * 42,
                24, 42);
            int hairid =  -1;
            int shirtid = -1;
            int pantsid = -1;
            int shoesid = -1;
            int beltid =  -1;
            int glovesid = -1;
            if (direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(head, dest, src, crace, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(body, dest, src, crace, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(legs, dest, src2, crace, 0, Vector2.Zero, effect, 0);
            spriteBatch.Draw(eye, dest, src, Color.White, 0, Vector2.Zero, effect, 0);

            if (hairid != -1) spriteBatch.Draw(Clothes.hair[hairid], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            if (shirtid != -1)
            {
                if (shirtid < Clothes.maxShirt) spriteBatch.Draw(Clothes.shirt[shirtid], dest, src, colors[0], 0, Vector2.Zero, effect, 0);
                else spriteBatch.Draw((Texture2D)Clothes.shirtMods[shirtid - Clothes.maxShirt], dest, src, colors[0], 0, Vector2.Zero, effect, 0);
            }
            if (pantsid != -1)
            {
                if (pantsid < Clothes.maxPants) spriteBatch.Draw(Clothes.pants[pantsid], dest, src, colors[1], 0, Vector2.Zero, effect, 0);
                else spriteBatch.Draw((Texture2D)Clothes.pantsMods[pantsid - Clothes.maxPants], dest, src, colors[1], 0, Vector2.Zero, effect, 0);
            }
            if (shoesid != -1) spriteBatch.Draw(Clothes.shoes[shoesid], dest, src, colors[2], 0, Vector2.Zero, effect, 0);
            if (beltid != -1)
            {
                if (beltid < Clothes.maxBelt)
                    spriteBatch.Draw(Clothes.belt[beltid], dest, new Rectangle(0, bellframe * legs.Height, legs.Width, legs.Height), colors[3], 0, Vector2.Zero, effect, 0);
                else
                {
                    spriteBatch.Draw((Texture2D)Clothes.beltMods[beltid - Clothes.maxBelt], dest, new Rectangle(0, bellframe * legs.Height, legs.Width, legs.Height), colors[3], 0, Vector2.Zero, effect, 0);
                }

            }
            if (glovesid != -1) spriteBatch.Draw(Clothes.gloves[glovesid], dest, src, colors[4], 0, Vector2.Zero, effect, 0);
            if (!slot[slotmax].IsEmpty)
            {
                    spriteBatch.Draw(Clothes.armor[slot[slotmax].getArmorModel()], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            } if (!slot[slotmax + 1].IsEmpty)
            {
                spriteBatch.Draw(Clothes.armor[slot[slotmax + 1].getArmorModel()], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            } if (!slot[slotmax + 2].IsEmpty)
            {
                spriteBatch.Draw(Clothes.armor[slot[slotmax + 2].getArmorModel()], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            }
            if (special != -1)
            {
                if (special < Clothes.maxCostume) spriteBatch.Draw(Clothes.suit[special], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
                else spriteBatch.Draw((Texture2D)Clothes.specialMods[special - Clothes.maxCostume], dest, src, Color.White, 0, Vector2.Zero, effect, 0);
            }
            //   spriteBatch.Draw(Program.game.shootgun, dest2, new Rectangle(0,0,40,17), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            // spriteBatch.Draw(suit[1], dest, src, Color.White, 0, Vector2.Zero, effect, 0);//
        }
        public void   renderleft(Texture2D hand, SpriteBatch sb,float scale = 1.0f)
        {
            SpriteEffects effect = SpriteEffects.None;
            Rectangle dest = new Rectangle(
                (int)(position.X + (width - hand.Width)),
                (int)(position.Y), (int)(hand.Width * scale), (int)(hand.Height * scale));
            Rectangle src = new Rectangle(0, 0,
                24, 42);
            if (direction < 0)
                effect = SpriteEffects.FlipHorizontally;
            sb.Draw(hand, dest, src, crace, 0, Vector2.Zero, effect, 0);
        }
        public void render(Texture2D Hand,Texture2D eye, Texture2D head, Texture2D body, Texture2D legs, SpriteBatch spriteBatch,Texture2D shadow)
        {
            render(eye, head, body, legs, spriteBatch);
            if (slot[selectedItem].IsEmpty == false)
            {
                Texture2D itemIcon;
                if (slot[selectedItem].iditem >= ModLoader.ModLoader.StartIdItem)
                {
                    int a = ModLoader.ModLoader.getIndexItem(slot[selectedItem].iditem);
                    itemIcon = ((ItemMod)ModLoader.ModLoader.ItemsMods[a]).texture;
                }
                else itemIcon = Item.items[slot[selectedItem].iditem];
                Rectangle src = new Rectangle(0, 0, itemIcon.Width, itemIcon.Height);
                SpriteEffects effect = SpriteEffects.None;
                if (direction > 0)
                {
                    spriteBatch.Draw(itemIcon,
                        new Rectangle((int)position.X + 22 - 8,
                        (int)position.Y + 30 - 8,
                        16, 16),
                        src,
                        Color.White, itemframe, new Vector2(0, itemIcon.Height / 2), effect, 0);
                }
                else
                {
                    effect = SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(itemIcon,
                        new Rectangle((int)position.X- 22 + 16,
                        (int)position.Y + 30 - 8,
                        16, 16),
                        src,
                        Color.White, -itemframe, new Vector2(itemIcon.Width / 2, itemIcon.Height / 2), effect, 0);
                }
            }
            renderleft(Hand,spriteBatch);
            int startnew = (int)position.Y / ITile.TileMaxSize;
            startnew += 3;
            int endnew = startnew + 10;
            for (int j = startnew; j < endnew; j++)
            {
                if (j >= SizeGeneratior.WorldHeight) continue;
                if (Program.game.dimension[Program.game.currentD].map[(int)Math.Floor(position.X) / ITile.TileMaxSize, j].active)
                {
                    int del = j - startnew;
                    //if (del == 0) del = 1;
                    Rectangle rect = new Rectangle((int)Math.Floor(position.X),
                    j * ITile.TileMaxSize, shadow.Width, (shadow.Height - (del)));

                    spriteBatch.Draw(shadow, rect, Color.Black);
                    break;
                }
            }
            //   spriteBatch.Draw(Program.game.shootgun, dest2, new Rectangle(0,0,40,17), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            // spriteBatch.Draw(suit[1], dest, src, Color.White, 0, Vector2.Zero, effect, 0);//
        }
        
        public void tp()
        {
            position.X = 1 * ITile.TileMaxSize;
            position.Y = Program.game.dimension[Program.game.currentD].mapHeight[1] * ITile.TileMaxSize - ITile.TileMaxSize * 3;
            if (position.Y >= (SizeGeneratior.WorldHeight - 1) * ITile.TileMaxSize) position.Y = ITile.TileMaxSize * (SizeGeneratior.WorldHeight - 1);
        }
        
        public void tp(int x, int y)
        {
            position.X = x * ITile.TileMaxSize;
            position.Y = y * ITile.TileMaxSize;
        }
        
        public void tp(int x)
        {

            if (x >= 0 && x < SizeGeneratior.WorldWidth)
            {
                position.X = x * ITile.TileMaxSize;
                position.Y = Program.game.dimension[Program.game.currentD].mapHeight[(int)x] * ITile.TileMaxSize - ITile.TileMaxSize * 3;
            }
        }
        [NonSerialized]
        Vector2 gravityVec = new Vector2(0, 1);
        void move()
        {
            Vector2 vec = new Vector2(gravityVec.X,gravityVec.Y);
            if (tilePos.X >= 0)
                vec = Util.dir(tilePos.ToPoint(), position.ToPoint());
            float speedAcc = runAcceleration;
            float speedMax = maxRunSpeed;
            float newJumpSpeed = jumpSpeed;
            int newJumpHeight = jumpHeight;
            /*for(int i = 0;i<3;i++){
                speedAcc*=slot[slotmax+i].getSpeedModdif();
                maxRunSpeed*=slot[slotmax+i].getSpeedModdif();
                newJumpSpeed *= slot[slotmax + i].getSpeedModdif();
                newJumpHeight = (int)(newJumpHeight* slot[slotmax + i].getSpeedModdif());
            }*/
            if (this.controlLeft && this.velocity.X > -speedMax)
            {
                if (this.velocity.X > runSlowdown)
                {
                    this.velocity.X = this.velocity.X - runSlowdown;
                }
                this.velocity.X = this.velocity.X - speedAcc;

                if (direction == 1)
                {
                    if (this.itemAnimation == 0)
                    {
                        this.direction = -1;
                    }
                }
            }
            else if (this.controlRight && this.velocity.X < speedMax)
            {
                if (this.velocity.X < -runSlowdown)
                {
                    this.velocity.X = this.velocity.X + runSlowdown;
                }
                this.velocity.X = this.velocity.X + speedAcc;
                if (direction == -1)
                {
                    if (this.itemAnimation == 0)
                    {
                        this.direction = 1;
                    }
                }
            }
            else if (this.velocity.Y == 0f)
            {
                if (this.velocity.X > runSlowdown)
                {
                    this.velocity.X = this.velocity.X - runSlowdown;
                }
                else if (this.velocity.X < -runSlowdown)
                {
                    this.velocity.X = this.velocity.X + runSlowdown;
                }
                else
                {
                    this.velocity.X = 0f;
                }
            }
            else if ((double)this.velocity.X > (double)runSlowdown * 0.5)
            {
                this.velocity.X = this.velocity.X - runSlowdown * 0.5f;
            }
            else if ((double)this.velocity.X < (double)(-(double)runSlowdown) * 0.5)
            {
                this.velocity.X = this.velocity.X + runSlowdown * 0.5f;
            }
            else
            {
                this.velocity.X = 0f;
            }
            if (this.controlJump)
            {
                if (this.jump > 0)
                {
                    if (this.velocity.Y > -newJumpSpeed + gravity * 2f)
                    {
                        this.jump = 0;
                    }
                    else
                    {
                        this.velocity.Y = -newJumpSpeed;
                        this.jump--;
                    }
                }
                else if (this.velocity.Y == 0f && this.releaseJump)
                {
                    this.velocity.Y = -newJumpSpeed;
                    this.jump = newJumpHeight;
                }
                this.releaseJump = false;
            }
            else
            {
                this.jump = 0;
                this.releaseJump = true;
            }
            if (!this.isHorisontal)
            {
                this.velocity.Y = this.velocity.Y + gravity * vec.Y;

                velocity.X += gravity * vec.X;
            }
            else
            {
                if (keystate.IsKeyDown(Keys.W))
                {
                    this.velocity.Y = this.velocity.Y -runAcceleration;
                }
                else if (keystate.IsKeyDown(Keys.S))
                {
                    this.velocity.Y = this.velocity.Y + runAcceleration;
                }
            }
            this.isHorisontal = false;
            if (this.velocity.Y > maxFallSpeed)
            {
                this.velocity.Y = maxFallSpeed;
            }
            float a = velocity.Y;
            this.velocity = Colision.TileCollision(this,this.position, this.velocity, this.width, this.height,keystate.IsKeyDown(Keys.S));
            this.position += this.velocity;
            if (velocity.X != 0 && itemAnimation == 0)
            {
                itemAnimation = 1;
            }
            if (itemAnimation != 0)
            {
                if (delta >= 1 / 2)
                {
                    if (this.itemAnimation < 3)
                    {
                        this.itemAnimation++;
                    }
                    else { itemAnimation = 0; }
                    delta = 0;
                }
                else delta += Program.game.seconds;
            }
            if (position.X < 0)
            {
                position.X = 0;
            }
            if (position.X > (SizeGeneratior.WorldWidth - 1) * ITile.TileMaxSize)
            {
                position.X = (SizeGeneratior.WorldWidth - 1) * ITile.TileMaxSize;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            if (position.Y > (SizeGeneratior.WorldHeight - 1) * ITile.TileMaxSize)
            {
                position.Y = (SizeGeneratior.WorldHeight - 1) * ITile.TileMaxSize;
            }
            if (velocity.Y < a && a > maxFallSpeed - 5) { bloods[0] = bloods[1] = true; }
        }
        public bool hasItem(int idItem)
        {
            for (int i = 0; i < currentMaxSlot; i++)
            {
                if (!slot[i].IsEmpty && slot[i].iditem == idItem) return true;
            }
            return false;
        }
        [NonSerialized]
        Vector2 tilePos = new Vector2(-1, -1);
        [NonSerialized]
        KeyboardState keystate;
        void keyupdate()
        {
            if (zombie)
            {
                if (Mouse.GetState().X >= 0 && Mouse.GetState().X <= Program.game.graphics.PreferredBackBufferWidth && Mouse.GetState().Y >= 0
                    && Mouse.GetState().Y <= Program.game.graphics.PreferredBackBufferHeight)
                    Mouse.SetPosition(Mouse.GetState().X + Program.game.rand.Next(-3, 3), Mouse.GetState().Y + Program.game.rand.Next(-3, 3));
            } this.controlLeft = false;
            this.controlRight = false;
            this.controlJump = false;
            if (Program.game.keyState.IsKeyDown((Keys)GameInput.ActiveIventory))
            {
                isj = true;
            }
            if (Program.game.keyState.IsKeyUp((Keys)GameInput.ActiveIventory))
            {
                if (isj) { controlJ = !controlJ; isj = false; Program.game.console.addLog(controlJ.ToString()); }
            }
            if (Program.game.keyState.IsKeyDown((Keys)GameInput.MoveLeft))
            {
                this.controlLeft = true;
            }
            if (Program.game.keyState.IsKeyDown((Keys)GameInput.MoveRight))
            {
                this.controlRight = true;
            }
            if (Program.game.keyState.IsKeyDown(Keys.F))
            {
                tilePos.X = tileTargetX * ITile.TileMaxSize;
                tilePos.Y = tileTargetY * ITile.TileMaxSize;
            }
            if (Program.game.keyState.IsKeyDown((Keys)GameInput.Jump) && (!bloods[0] || !bloods[1]))
            {
                this.controlJump = true;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D1))
            {
                this.selectedItem = 0;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D2))
            {
                this.selectedItem = 1;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D3))
            {
                this.selectedItem = 2;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D4))
            {
                this.selectedItem = 3;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D5))
            {
                this.selectedItem = 4;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D6))
            {
                this.selectedItem = 5;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D7))
            {
                this.selectedItem = 6;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D8))
            {
                this.selectedItem = 7;
            }
            if (Program.game.keyState.IsKeyDown(Keys.D9))
            {
                this.selectedItem = 8;
            }

            if (Program.game.keyState.IsKeyDown(Keys.G) && !controlG)
            {
                if (bellframe == 0) bellframe = 1;
                else bellframe = 0;
                controlG = true;
            }
            if (Program.game.keyState.IsKeyDown((Keys)GameInput.Drop))
            {
                if (!slot[selectedItem].IsEmpty) dropplayer(selectedItem);
            }
            if (Program.game.keyState.IsKeyUp(Keys.G)) controlG = false;
            if ((Program.game.keyState.IsKeyUp(Keys.E) && keystate.IsKeyDown(Keys.E)) || (Program.game.keyState.IsKeyDown(Keys.E) && keystate.IsKeyUp(Keys.E)))
            {
                gravityVec *= -1;
            }
            if (Program.game.keyState.IsKeyDown(Keys.E)) Program.game.camera.lerpZoom(0.5f, Program.game.seconds);
            else Program.game.camera.lerpZoom(1.0f, Program.game.seconds);
            if (Program.game.keyState.IsKeyDown(Keys.F))
            {
                if (!keydownf)
                {
                    keydownf = true;
                }
            }
            else
            {
                keydownf = false;
            }
            keystate = Program.game.keyState;
        }
        void angry()
        {

            if (bloods[0] && bloods[1]) maxRunSpeed = 1;
            else if (bloods[0] || bloods[1]) maxRunSpeed = 2;
            if (Temperature > 40) maxRunSpeed -= 0.5f;
        }
        
        public void killnpc(bool isnpc = false)
        {
            if (isnpc)
            {
                Zombie npc = new Zombie(position, new Race(getcolor(), null), clslot, colors);
                npc.drop.Clear();
                for (int i = 0; i < slotmax + 3; i++)
                {
                    if (!slot[i].IsEmpty)
                    {
                        npc.drop.Add(slot[i]);
                    }

                }
                Program.game.dimension[Program.game.currentD].Zombies.Add(npc);
            }
            else
            {

                for (int i = 0; i < slotmax + 3; i++)
                {
                    if (!slot[i].IsEmpty)
                    {
                        Program.game.adddrop((int)position.X, (int)position.Y, slot[i]);
                    }

                }
            }

            spawn();
        }

        public void update(float delta, float seconds)
        {
            if (Quests.Count >= 1) QuestUpdate();
            if (Program.game.dimension[Program.game.currentD].civil.Count >= 1) dialog();
            keyupdate();
            foreach (Bind bind in GameInput.BindKey)
            {
                bind.update(Program.game.keyState);
            }
            move();
            foreach (GUI.FlashText ft in FTlist)
            {
                ft.Update();
                if (ft.IsInvisible())
                {
                    FTlist.Remove(ft);
                    break;
                }
            }
            killerUpdate(delta, seconds);
            mouseUpdate();
        }

        private void mouseUpdate()
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            tileTargetX = (int)Util.cameraMouse(Program.game.mouseState.Position.ToVector2()).X / ITile.TileMaxSize;
            tileTargetY = (int)Util.cameraMouse(Program.game.mouseState.Position.ToVector2()).Y / ITile.TileMaxSize;
            if (ms.ScrollWheelValue > Program.game.mouseState.ScrollWheelValue)
            {
                if (selectedItem > 0)
                    selectedItem--;
            }
            else if (ms.ScrollWheelValue < Program.game.mouseState.ScrollWheelValue)
            {
                if (selectedItem < currentMaxSlot - 1)
                    selectedItem++;
            }
            if (timeSlotUse > 0) timeSlotUse -= 1;
            if (Util.MouseInCube(0, 0, Program.game.graphics.PreferredBackBufferWidth, Program.game.graphics.PreferredBackBufferHeight))
            {
                if (GameInput.MouseButtonIsDown((int)GameInput.MouseButton.LeftButton))
                {
                    int r = -1;
                    if (itemframe >= 1) itemto = true;
                    else if (itemframe < 0) itemto = false;
                    if (itemto) itemframe -= 10f * Program.game.seconds;
                    else itemframe += 10f * Program.game.seconds;
                    if (!dmgnpc())
                    {
                        for (int i = 0; i < currentMaxSlot; i++)
                        {
                            if (!controlJ && i > 9) break;
                            if (i % 9 == 0) r++;
                            if (!slot[i].IsEmpty)
                            {
                                if (Util.MouseInCube((i % 9) * 32, 32 * r, 32, 32))
                                {
                                    selectedItem = i;
                                }
                            }
                        }
                    }
                }
                else if (GameInput.MouseButtonIsPressed((int)GameInput.MouseButton.LeftButton))
                {
                    if (itemframe >= 1) itemto = true;
                    else if (itemframe < 0) itemto = false;
                    if (itemto) itemframe -= 10f * Program.game.seconds;
                    else itemframe += 10f * Program.game.seconds;
                    if (!recept() && !bloods[2])
                    {
                        if (Util.directional((int)position.X / ITile.TileMaxSize, tileTargetX, 7) &&
                        Util.directional((int)position.Y / ITile.TileMaxSize, tileTargetY, 7) && 
                        Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].active )
                        {
                            destoryBlock(tileTargetX, tileTargetY);
                        }
                        dmgnpc();
                    }
                    
                }
                else if (GameInput.MouseButtonIsDown((int)GameInput.MouseButton.RightButton))
                {
                    bool t = false;
                    if (!t)
                    {
                        if (!MouseItem.IsEmpty)
                        {
                            dropplayer(ref MouseItem);
                        }
                    }
                }
                else if(GameInput.MouseButtonIsPressed((int)GameInput.MouseButton.RightButton)){
                    chatnpc();
                    draganddrop(false);
                    if (!slot[selectedItem].IsEmpty && !bloods[3])
                    {
                        if (timeSlotUse <= 0)
                            slotuse(selectedItem);
                        if (timeSlotUse <= 0 &&  Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].active &&
                            blockuse(Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY]) )
                        {
                            timeSlotUse = 50;
                        }

                    }
                }
                if (GameInput.MouseButtonIsReleased((int)GameInput.MouseButton.LeftButton) || GameInput.MouseButtonIsReleased((int)GameInput.MouseButton.RightButton))
                {
                    itemto = false;
                    itemframe = 0;
                }
            }
            ms = Program.game.mouseState;
        }
        private float getMassFactor()
        {

            return slot[slotmax].getMassFactor() + slot[slotmax + 1].getMassFactor() + slot[slotmax + 2].getMassFactor();
        }
        private void killerUpdate(float delta, float seconds)
        {

            for (int i = 0; i < 4; i++)
            {
                if (bloods[i]) blood -= 100 * delta;
            }
            drought = Math.Max(0, drought - ((25 + 10 * getMassFactor()) * delta));

            hunger = Math.Max(0, hunger - ((25 ) * delta));

            if (zombie) Temperature += 1 * seconds;
            if (blood <= 4000 || hunger < 10 || drought < 10 || Temperature >= 45)
            {
                typeKill = 1;
            }
        }

        private void destoryBlock(int BlockX, int BlockY)
        {
            ITile tl = Program.game.dimension[Program.game.currentD].map[BlockX, BlockY];
            removeblock();
        }

        private void dropplayer(int dropslot)
        {
            float dirx = 0;
            if (tileTargetX < (position.X) / 16) dirx = 1;
            if (tileTargetX > (position.X ) / 16) dirx = -1;
            Program.game.adddrop((int)(position.X) + (int)(-dirx * width), (int)(position.Y), slot[dropslot], dirx);
            slot[dropslot] = new Item();
        }

        private void dropplayer(ref Item dropslot)
        {
            float dirx = 0;
            if (tileTargetX < (position.X) / 16) dirx = 1;
            if (tileTargetX > (position.X) / 16) dirx = -1;
            Program.game.adddrop((int)(position.X) + (int)(-dirx * width), (int)(position.Y), dropslot, dirx);
            dropslot = new Item();
        }
        public int currentMaxSlot = 1+(9 * 4);
        private bool draganddrop(bool t)
        {
            if (controlJ)
            {
                t = dragItemSlot(t, currentMaxSlot);
                for (int i = slotmax; i < slotmax + 3; i++)
                {
                    if (Util.MouseInCube((i - slotmax) * 32, Program.game.graphics.PreferredBackBufferHeight - 32, 32, 32))
                    {
                        if (MouseItem.getArmorSlot() - 1 == (i - slotmax))
                        {
                            t = MouseItemSlot(t, ref slot[i]);
                        }
                        t = true;
                        break;
                    }
                }
            }
            else t = dragItemSlot(t, 9);
            return t;
        }
        private void pickItem(ref Item i)
        {
            MouseItem = new Item(i.ammount, i.iditem);
            MouseItem.HP = i.HP;
            i = new Item();
        }
        private bool dragItemSlot(bool t,int maxSlot)
        {
            int r = -1;
            int centerSlot = Program.game.graphics.PreferredBackBufferWidth - (9 * 32);
            for (int i = 0; i < maxSlot; i++)
            {

                if (i % 9 == 0) r++;
                int xslot = (i % 9) * 32;
                int yslot = 32 * r;
                if (Util.MouseInCube(xslot+centerSlot,yslot, 32, 32))
                {
                    t = MouseItemSlot(t, ref slot[i]);
                    break;
                }
            }
            return t;
        }

        private bool MouseItemSlot(bool t, ref Item item)
        {
            if (!item.IsEmpty && MouseItem.IsEmpty)
            {
                t = true;
                pickItem(ref item);
            }
            else if (!MouseItem.IsEmpty)
            {
                t = true;
                SlotChange(ref item, ref MouseItem);
                MouseItem = new Item();
            }
            return t;
        }

        private void SlotChange(int index1,int index2)
        {
            SlotChange(ref slot[index1], ref slot[index2]);
        }
        private void SlotChange(ref Item item1,ref Item item2)
        {
            Item temp = new Item(item1.ammount, item1.iditem);
            temp.HP = item1.HP;
            item1 = new Item(item2.ammount, item2.iditem);
            item1.HP = item2.HP;
            item2 = new Item(item1.ammount, item1.iditem);
            item2.HP = item1.HP;

        }
        public void TeleportToShowMap()
        {
            stats.Translition++;
            Program.game.currentD = 2;
            tp();
        }
        bool blockuse(ITile title)
        {
            return title.blockuse(tileTargetX,tileTargetY,this);
        }
        int timeSlotUse = 0;
        void slotuse(int selecteditem)
        {
            if (slot[selectedItem].getTypeItem() == (int)Item.Type.EAT && drought < 100)
            {
                int heal = 10;
                if (slot[selecteditem].iditem >= ModLoader.ModLoader.StartIdItem)
                {
                    int a = ModLoader.ModLoader.getIndexItem(slot[selecteditem].iditem);
                    heal = ((ItemMod)ModLoader.ModLoader.ItemsMods[a]).param;
                }
                
                drought = Math.Min(100, drought + heal);
                RemoveItemCurrentSlot();
                timeSlotUse = 10;
            }
            else if (slot[selecteditem].getTypeItem() == (int)Item.Type.MODDINGITEM)
            {
                int a = ModLoader.ModLoader.getIndexItem(slot[selecteditem].iditem);
                string modpath = ((ItemMod)ModLoader.ModLoader.ItemsMods[a]).modPath;
                int b = ModLoader.ModLoader.getIndexMode(modpath);
                bool isDestory = (bool)((ModFile)ModLoader.ModLoader.Mods[b]).callFunction("useItem", slot[selecteditem].iditem - ModLoader.ModLoader.StartIdItem)[0];
                if (isDestory) RemoveItemCurrentSlot();
                timeSlotUse = 10;
            }
            else if (slot[selectedItem].getTypeItem() == (int)Item.Type.WATER && hunger < 100)
            {
                int heal = 10;
                if (slot[selecteditem].iditem >= ModLoader.ModLoader.StartIdItem)
                {
                    int a = ModLoader.ModLoader.getIndexItem(slot[selecteditem].iditem);
                    heal = ((ItemMod)ModLoader.ModLoader.ItemsMods[a]).param;
                }
                hunger = Math.Min(100, hunger + heal);
                timeSlotUse = 10;
                RemoveItemCurrentSlot();
            }
            else if (slot[selecteditem].getTypeItem() == (int)Item.Type.BANDAGE && (bloods[0] || bloods[1] || bloods[2] || bloods[3]))
            {
                int a = 0;
                while (!bloods[a])
                {
                    a = Program.game.rand.Next(0, 4);
                }
                bloods[a] = false;
                RemoveItemCurrentSlot();
            }
            else if (slot[selecteditem].getTypeItem() >= (int)Item.Type.HEAD && slot[selecteditem].getTypeItem() <= (int)Item.Type.LEGS)
            {
                Item sl = new Item();
                int slotid = slotmax + (slot[selecteditem].getTypeItem() - (int)Item.Type.HEAD);
                if (!slot[slotid].IsEmpty) sl = slot[slotid];
                slot[slotid] = slot[selecteditem];
                slot[selecteditem] = sl;
            }
            else if (slot[selecteditem].getTypeItem() == (int)Item.Type.WEAPONGUN)
            {
                int slotids = this.getslotitem(1);
                if (slotids != -1)
                {
                    timeSlotUse = 10;
                    Program.game.bullets.Add(new Bullet(position, Util.directional(position)));
                    RemoveItemSlot(slotids);
                }
            }
            else if (slot[selectedItem].getTypeItem() == (int)Item.Type.BLOCKICON || slot[selectedItem].getTypeItem() == (int)Item.Type.HORISONTALBLOCKICON || slot[selectedItem].getTypeItem() == (int)Item.Type.VERTICALBLOCKICON)
            {
                if (Util.directional((int)position.X / ITile.TileMaxSize, tileTargetX, 7) &&
                    Util.directional((int)position.Y / ITile.TileMaxSize, tileTargetY, 7))
                {
                    addblock();
                }
            }
        }
        
        public void AddLog(string Text)
        {
            GUI.GUI newgui = new GUI.GUI();
            GUI.Button button = new GUI.Button(Program.game.black, Program.game.Font1, new Rectangle(Program.game.graphics.PreferredBackBufferWidth / 2 - 500 / 2,
                (Program.game.graphics.PreferredBackBufferHeight - 100 - Program.game.guis.Count * 60) + 40, 500, 20), "Close");
            GUI.Label lab = new GUI.Label(Text, new Vector2(Program.game.graphics.PreferredBackBufferWidth / 2 - 500 / 2,
                Program.game.graphics.PreferredBackBufferHeight - 100 - Program.game.guis.Count * 60), Program.game.Font1);
            GUI.Image image = new GUI.Image(Program.game.achivement, new Rectangle(Program.game.graphics.PreferredBackBufferWidth / 2 - 500 / 2,
               Program.game.graphics.PreferredBackBufferHeight - 100 - Program.game.guis.Count * 60, 500, 60));
            newgui.AddButon(button);
            newgui.AddLabel(lab);
            newgui.AddImage(image);
            newgui.AddClicked((Object sender,EventArgs e) => {
                Program.game.RemoveGUI(newgui);
            },0);
            Program.game.addGui(newgui);
        }
        
        public bool AddHorisontalBlock()
        {
            if (!Program.game.dimension[Program.game.currentD].map[tileTargetX - 1, tileTargetY].active)
            {
                Program.game.dimension[Program.game.currentD].settexture(tileTargetX, tileTargetY, slot[selectedItem].getBlockId());
                Program.game.dimension[Program.game.currentD].settexture(tileTargetX - 1, tileTargetY, slot[selectedItem].getBlockId() + 1);
                Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].HP = Math.Max(1, Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].getBlockHP() * (slot[selectedItem].HP / 100));
                RemoveItemCurrentSlot();
                return true;
            }
            return false;
        }
        
        public bool AddVerticallBlock()
        {
            if (!Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY - 1].active)
            {
                Program.game.dimension[Program.game.currentD].settexture(tileTargetX, tileTargetY, slot[selectedItem].getBlockId());
                Program.game.dimension[Program.game.currentD].settexture(tileTargetX, tileTargetY - 1, slot[selectedItem].getBlockId() + 1);
                Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].HP = Math.Max(1, Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].getBlockHP() * (slot[selectedItem].HP / 100));
                RemoveItemCurrentSlot();
                return true;
            }
            return false;
        }
        
        void addblock()
        {
            if (!Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].active)
            {
                //if (Block.isLightBlock(slot[selectedItem].idblock)) type = ITile.TYPE.NO_SOILD_BLOCK; //TODO
                if (slot[selectedItem].getTypeItem() == (int)Item.Type.HORISONTALBLOCKICON)
                {

                    if (AddVerticallBlock())
                    {
                        achivement.Complete(Achivement.ACHIVEMENT.ModifedTerrain);
                    }
                }
                else if (slot[selectedItem].getTypeItem() == (int)Item.Type.VERTICALBLOCKICON)
                {
                    if (AddHorisontalBlock())
                    {
                        achivement.Complete(Achivement.ACHIVEMENT.ModifedTerrain);
                    }
                }
                else
                {
                    Program.game.dimension[Program.game.currentD].settexture(tileTargetX, tileTargetY, slot[selectedItem].getBlockId());
                    achivement.Complete(Achivement.ACHIVEMENT.ModifedTerrain);
                }

            }

        }
        
        private void RemoveItemSlot(int slotid)
        {

            slot[slotid].ammount--;
            if (slot[slotid].ammount <= 0)
            {
                slot[slotid] = new Item();
            }
        }
        
        private void RemoveItemCurrentSlot()
        {
            RemoveItemSlot(selectedItem);
        }
        
        public void setslot(Item slots)
        {
            int ammout = slots.ammount;
            for (int j = 0; j < ammout; j++)
            {
                int i = this.getslotitem(slots.iditem, slots.HP);//pos.x = 5;tiletar=6; 5-6=-1 
                if (i != -1)
                {
                    slot[i].ammount += 1;
                    slots.ammount -= 1;
                }
                else
                {
                    slot[this.getslotfull()] = slots;
                    break;
                }
            }
        }
        
        public void getDrop(int tileX, int tileY)
        {


            List<Item> itemDrop = Program.game.dimension[Program.game.currentD].map[tileX, tileY].destory(tileX,tileY,this);
            foreach (Item bs in itemDrop)
            {
                Program.game.adddrop(tileX * 16 + Program.game.dimension[Program.game.currentD].rand.Next(-8, 8), (tileY) * 16, bs);
            }
        }
        
        bool destory(int x, int y)
        {
            if (Program.game.dimension[Program.game.currentD].map[x, y].isNeadTool(slot[selectedItem]))
            {
                Program.game.dimension[Program.game.currentD].map[x, y].damageslot(
                    (slot[selectedItem].GetPower() * (slot[selectedItem].HP / 100)) * Program.game.seconds);
                slot[selectedItem].damageslot(Program.game.dimension[Program.game.currentD].map[x, y].getBlockHP()
                    * (Program.game.seconds / 2));
                if (Program.game.dimension[Program.game.currentD].map[x, y].HP <= 0)
                {
                    achivement.Complete(Achivement.ACHIVEMENT.ModifedTerrain);
                    return true;
                }
            }
            return false;
        }
        
        bool removeblock()
        {

            if (Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].isNeadTool(slot[selectedItem]))
            {
                Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].damageslot(
                    (slot[selectedItem].GetPower() * (slot[selectedItem].HP / 100)) * Program.game.seconds);
                slot[selectedItem].damageslot(Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].getBlockHP()
                    * (Program.game.seconds / 2));
                if (Program.game.dimension[Program.game.currentD].map[tileTargetX, tileTargetY].HP <= 0)
                {
                    achivement.Complete(Achivement.ACHIVEMENT.ModifedTerrain);
                    getDrop(tileTargetX, tileTargetY);
                } return true;
            }

            return false;
        }
        
        bool chatnpc()
        {
            if (Program.game.dimension[Program.game.currentD].civil.Count >= 1)
            {
                foreach (Civilian npc in Program.game.dimension[Program.game.currentD].civil)
                {
                    if (Util.ScMouseInCube((int)npc.position.X,
                    (int)npc.position.Y, npc.width, npc.height) &&
                    Util.directional((int)position.X, (int)npc.position.X, 32 + 8) &&
                    Util.directional((int)position.Y, (int)npc.position.Y, 32 + 8))
                    {
                        achivement.Complete(Achivement.ACHIVEMENT.TalkingNPC);
                        npc.changeenable();
                        return true;
                    }
                }
            }
            return false;
        }
        bool dmgNpc(BaseNpc npc)
        {
            Vector2 posEnemy = new Vector2((int)npc.position.X, (int)npc.position.Y);
            //System.Console.WriteLine(posEnemy.ToString());
            if (Util.ScMouseInCube((int)posEnemy.X, (int)posEnemy.Y, npc.width, npc.height) &&
            Util.directional((int)position.X, (int)npc.position.X, 32 + 8) &&
            Util.directional((int)position.Y, (int)npc.position.Y, 32 + 8))
            {
                float hpdamage = (slot[selectedItem].Getdamage() * (slot[selectedItem].HP / 100)) * Program.game.seconds;
                GUI.FlashText ft = new GUI.FlashText(new Vector2(posEnemy.X, posEnemy.Y - 20), hpdamage.ToString(), Program.game.Font1, Color.Red);
                npc.hp -= hpdamage;
                FTlist.Add(ft);
                slot[selectedItem].damageslot(1 * Program.game.seconds);
                Program.game.console.addLog(Program.game.seconds.ToString());
                if ((int)npc.hp <= 0)
                {
                    Civilian civ = (Civilian)(Program.game.dimension[Program.game.currentD].civil[0]);
                    if (civ.activedialog == 1) { zombiekill = true; }
                    if (npc.type == 5) ((Boss)(npc)).kill();
                    else npc.kill();
                    Program.game.dimension[Program.game.currentD].Zombies.Remove(npc); return true;
                }
                Program.game.console.addLog("NPC DROP:" + npc.drop.Count);
            }
            return false;
        }
        bool dmgnpc()
        {
            if (!bloods[2])
            {
                if (Program.game.dimension[Program.game.currentD].Zombies.Count >= 1)
                {
                    foreach (Zombie npc in Program.game.dimension[Program.game.currentD].Zombies)
                    {
                        if (dmgNpc(npc)) return true;
                    }
                    if (Program.game.dimension[Program.game.currentD] is NormalWorld)
                    {
                        foreach (Wolf npc in ((NormalWorld) Program.game.dimension[Program.game.currentD]).wolfs)
                        {
                            if (dmgNpc(npc)) return true;
                        }
                    }
                }
            }
            return false;
        }
        
        bool recept()
        {
            if (controlJ)
            {
                int a = 0;
                for (int i = 0; i < Program.game.recipes.Count; i++)
                {
                    if (getvalidrecipes(i))
                    {
                        if (Util.MouseInCube(a * 32, 35 + 32 * -1, 32, 32))
                        {
                            removerecipes(i);
                            stats.AddRecipe(((Recipe)Program.game.recipes[i]).slot.iditem, ((Recipe)Program.game.recipes[i]).slot.ammount);
                            giveItem(((Recipe)Program.game.recipes[i]).slot);

                            return true;
                        }
                        if (a >= 9) break;
                        a++;
                    }
                }

            }
            return false;
        }

        private void giveItem(Item item)
        {
            int i2 = this.getslotitem(item.iditem, 100);//pos.x = 5;tiletar=6; 5-6=-1 
            if (i2 != -1)
            {
                slot[i2].ammount += item.ammount;
                slot[i2].IsEmpty = false;
            }
            else
            {
                i2 = this.getslotfull();
                slot[i2] = item;
                slot[i2].IsEmpty = false;
            }
        }

        public void activespecial(int p)
        {
            special = p;
        }

        public bool isHorisontal { get; set; }
    }
}
