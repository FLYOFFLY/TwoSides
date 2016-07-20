using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI.Scene;
using TwoSides.GUI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.GameContent.GUI.Scene
{
    public class SettingMenu : IScene
    {
        public bool lastSceneRender { get; set; }
        public bool lastSceneUpdate { get; set; }
        ControlScene scene;
        Button[] btn = new Button[2];
        public void Load(ControlScene scene)
        {
            int sizeButton = (int)Program.game.Font1.MeasureString("Exit Game").X + 50;
            for (int i = 0; i < 2; i++)
            {
                btn[i] = new Button(Program.game.button, Program.game.Font1, new Rectangle(0, 0, 400, 400), "null");
                Rectangle rect = new Rectangle(Program.game.graphics.PreferredBackBufferWidth / 2-sizeButton/2, (Program.game.heightmenu) + 35 * i, sizeButton, 30);
                btn[i].SetRect(rect);
            }
            btn[0].Text = "Graphics";
            btn[1].Text = "Music";
            this.scene = scene;
        }
        public void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 2; i++)
            {
                btn[i].Draw(spriteBatch);
            }
        }
        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < 2; i++)
            {
                btn[i].Update();
            }
            if (btn[0].IsClicked())
            {
                scene.changeScene(new GraphicsMenu());
            }
            if (btn[1].IsClicked())
            {
                scene.changeScene(new MusicMenu());
            }
        }
        public void tryExit()
        {
            scene.returnScene();
        }
    }
}
