using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI.Scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoSides.GUI;
using System.Threading;
using TwoSides.Network;

namespace TwoSides.GameContent.GUI.Scene
{
    public class MainMenu : IScene
    {
        public bool lastSceneRender{get;set;}
        public bool lastSceneUpdate{get;set;}
        Button[] btn = new Button[6];
        ControlScene scene;
        Label version;
        Image image;
        public void Load(ControlScene scene)
        {
            int y = (int)((Program.game.graphics.PreferredBackBufferHeight / 3) / 2);
            int x = (int)(y * (887 / 133));
            image = new Image(Program.game.Content.Load<Texture2D>(Game1.ImageFolder + "header"),
                new Rectangle(Program.game.graphics.PreferredBackBufferWidth / 2 - x / 2,
                   0,
                    (int)x,
                    (int)y));
            int sizeButton = (int)Program.game.Font1.MeasureString("Exit Game").X + 50;
            int xButton = x - sizeButton;
            btn[0] = new Button(Program.game.button, Program.game.Font1, new Rectangle(0, 0, sizeButton, 30), "null");
            btn[0].setPatern(image);
            btn[0].setPos(new Vector2(0, y+sizeButton/2));
            for (int i = 1; i < 6; i++)
            {
                btn[i] = new Button(Program.game.button, Program.game.Font1, new Rectangle(0, 0, sizeButton, 30), "null");
                if (i >= 1) btn[i].setPatern(btn[i - 1]);
                if (i % 2 == 1) btn[i].setPos(new Vector2(xButton, 0));
                else btn[i].setPos(new Vector2(-xButton, 40));
               // btn[i].SetRect(rect);
            }
            btn[0].Text = "New Game";
            btn[1].Text = "Setting";
            btn[2].Text = "Load Game";
            btn[3].Text = "Exit Game";
            btn[4].Text = "Start Server";
            btn[5].Text = "Start Client";
            version = new Label(Program.game.getVersion(), new Vector2(0, Program.game.graphics.PreferredBackBufferHeight-(int)Program.game.Font1.MeasureString(Program.game.getVersion()).Y), Program.game.Font1,Color.Black);
            this.scene = scene;
        }
        public void Render(SpriteBatch spriteBatch) {

            for (int i = 0; i < 6; i++)
            {
                btn[i].Draw(spriteBatch);
            }
            version.Draw(spriteBatch);
            image.Draw(spriteBatch);
           //bar.Render(Program.game.carma, spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < 6; i++)
            {
                btn[i].Update();
            }
            if (btn[0].IsClicked())
            {
                scene.changeScene(new SelectRace());
            }
            if (btn[1].IsClicked())
            {
                scene.changeScene(new SettingMenu());
            }
            if (btn[2].IsClicked())
            {
                scene.changeScene(Progress.instance);
                Program.game.loadMap();
            }
            if (btn[3].IsClicked())
            {
                Program.game.Exit();
            }
            if (btn[4].IsClicked())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(NetPlay.startServer));
                scene.changeScene(new SelectRace());
            }
            if (btn[5].IsClicked())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(NetPlay.startClient));
                scene.changeScene(new SelectRace());
            }
        }
        public void tryExit()
        {
            Program.game.Exit();
        }
    }
}
