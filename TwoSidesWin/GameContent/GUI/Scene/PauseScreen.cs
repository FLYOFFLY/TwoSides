using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI.Scene;
using TwoSides.GUI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TwoSides.GameContent.GUI.Scene
{
    public class PauseScreen : IScene
    {
        public bool lastSceneRender { get; set; }
        public bool lastSceneUpdate { get; set; }
        ControlScene scene;
        public Button[] btn = new Button[6];
        public void Load(ControlScene scene)
        {
            for (int i = 0; i < 6; i++)
            {
                btn[i] = new Button(Program.game.button, Program.game.Font1, new Rectangle(0, 0, 400, 400), "null");
                Rectangle rect = new Rectangle(120, (Program.game.heightmenu) + 27 * i, (int)Program.game.Font1.MeasureString("Exit Game").X + 50, 25);

                btn[i].SetRect(rect);
            }
            lastSceneRender = true;
            this.scene = scene;
            btn[0].Text = "Continue Game";
            btn[1].Text = "New Game";
            btn[2].Text = "Load Game";
            btn[3].Text = "Save Game";
            btn[4].Text = "Exit";
        }
        public void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 6; i++) btn[i].Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up)) Program.game.camera.pos.Y -= 8;
            if (keyState.IsKeyDown(Keys.Down)) Program.game.camera.pos.Y += 8;
            if (keyState.IsKeyDown(Keys.Left)) Program.game.camera.pos.X -= 8;
            if (keyState.IsKeyDown(Keys.Right)) Program.game.camera.pos.X += 8;
            for (int i = 0; i < 6; i++) btn[i].Update();
            if (btn[0].IsClicked()) scene.returnScene();
            else if (btn[1].IsClicked())
            {
                scene.changeScene(new MainMenu());
            }
            else if (btn[2].IsClicked())
            {
                scene.changeScene(Progress.instance); 
                Program.game.loadMap();
            }
            else if (btn[3].IsClicked())
            {
                scene.changeScene(Progress.instance); 
                Program.game.saveMap();
            }
            else if (btn[4].IsClicked()) Program.game.Exit();
        }
        public void tryExit()
        {
            scene.returnScene();
        }
    }
}
