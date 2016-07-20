using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI.Scene;
using TwoSides.GUI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using TwoSides.Network;

namespace TwoSides.GameContent.GUI.Scene
{
    class CreationPerson : IScene
    {
        public bool lastSceneRender { get; set; }
        public bool lastSceneUpdate { get; set; }
        Button btn;
        TextField inp;
        Label lab;
        bool keydownup = false;
        Label version { get; set; }
        ControlScene scene { get; set; }
        public void Load(ControlScene scene)
        {

            lab = new Label("Input Name:",
                new Vector2(150, (Program.game.heightmenu) + 35),
                Program.game.Font1, Color.Black);
            inp = new TextField(new Vector2(20, 0),
                Program.game.Font1, Color.Black);
            inp.setPatern(lab);
            inp.setPos(new Vector2(0, 27));

            btn = new Button(Program.game.button, Program.game.Font1, new Rectangle(0, 0, 100, 30), "null");
            btn.setPatern(inp);
            btn.SetRect(new Rectangle(0, 35, 100, 30));
            btn.Text = "Start";


            this.scene = scene;
            Program.game.loadSlots();
            Program.game.player.position.X = 100;
            Program.game.player.position.Y = 255;
            version = new Label(Program.game.getVersion(), new Vector2(0, Program.game.graphics.PreferredBackBufferHeight - (int)Program.game.Font1.MeasureString(Program.game.getVersion()).Y), Program.game.Font1, Color.Black);

        }
        int currentslot = 0;
        private bool isdownleft;
        public void Render(SpriteBatch spriteBatch)
        {
            version.Draw(spriteBatch);
            inp.Draw(spriteBatch);
            lab.Draw(spriteBatch);
            btn.Draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(Program.game.Font1, "Up,Down-Select Type Clothes", new Vector2(150, 180), Color.Black);
            spriteBatch.DrawString(Program.game.Font1, "Left,Right - Select clothes", new Vector2(150, 200), Color.Black);
            Program.game.PlayerRender(2);
            Program.game.PlayerRenderTexture(Program.game.slots[currentslot],2);
            
           spriteBatch.End();
        }
        public void tryExit()
        {
            scene.returnScene();
        }
        public void Update(GameTime gameTime)
        {
           KeyboardState keyState = Keyboard.GetState();
            inp.updateKey();

            //  inp.rightsort(); 
            if (keyState.IsKeyDown(Keys.Down) && !keydownup)
            {
                if (currentslot < 5) { currentslot++; keydownup = true; }
            }


            if (keyState.IsKeyDown(Keys.Up) && !keydownup)
            {
                if (currentslot > 0) { currentslot--; keydownup = true; }
            }
            if (keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.Down)) keydownup = false;
            //

            if (keyState.IsKeyDown(Keys.Right) && !isdownleft)
            {
                bool type = false;
                int currentitem = Program.game.player.clslot[currentslot].getid();
                if (currentslot == 0 && currentitem + 1 < Clothes.maxHair) type = true;
                else if (currentslot == 1 && currentitem + 1 < Clothes.maxShirt + Clothes.shirtMods.Count) type = true;
                else if (currentslot == 2 && currentitem + 1 < Clothes.maxPants + Clothes.pantsMods.Count) type = true;
                else if (currentslot == 3 && currentitem + 1 < Clothes.maxShoes) type = true;
                else if (currentslot == 4 && currentitem + 1 < Clothes.maxBelt + Clothes.beltMods.Count) type = true;
                else if (currentslot == 5 && currentitem + 1 < Clothes.maxGlove) type = true;
                if (type)
                {
                    isdownleft = true;
                    Program.game.player.clslot[currentslot] = new Clothes(currentitem + 1);
                }
            }
            if (keyState.IsKeyDown(Keys.Left) && !isdownleft)
            {

                bool type = false;
                int currentitem = Program.game.player.clslot[currentslot].getid();
                if (currentitem > 0) type = true;
                isdownleft = true;
                if (type)
                {
                    Program.game.player.clslot[currentslot] = new Clothes(currentitem - 1);
                }
                else
                {
                    Program.game.player.clslot[currentslot] = new Clothes();
                }
            }
            if (keyState.IsKeyUp(Keys.Right) && keyState.IsKeyUp(Keys.Left)) isdownleft = false;
            //
            btn.Update();
            if (btn.IsClicked())
            {
                if (inp.getText() != null)
                    Program.game.player.Name = inp.getText();
                else Program.game.player.Name = "Player";
                if (NetPlay.typeNet != 2)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Program.game.newGeneration));
                else
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Program.game.loadClient));
                }
            }
        }
    }
}
