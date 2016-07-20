using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI;
using TwoSides.GUI.Scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.GameContent.GUI.Scene
{
    public class GraphicsMenu : IScene
    {
        public bool lastSceneRender { get; set; }
        public bool lastSceneUpdate { get; set; }
        ControlScene scene;
        Label lab;
        RadioButton rb;

        DisplayModeCollection colection;
        List<Button> displayMode = new List<Button>();
        List<DisplayMode> disp = new List<DisplayMode>();
        public void Load(ControlScene scene)
        {

           lab = new Label("Full screen",new Vector2(Program.game.graphics.PreferredBackBufferWidth/2,Program.game.graphics.PreferredBackBufferHeight/2),Program.game.Font1);
           
            rb = new RadioButton(Program.game.graphics.IsFullScreen,Program.game.galka,
                Program.game.ramka,Program.game.Font1,
                new Rectangle(Program.game.graphics.PreferredBackBufferWidth/2+(int)Program.game.Font1.MeasureString("Full screen").X+16,Program.game.graphics.PreferredBackBufferHeight/2-16,32,32));
            this.scene = scene;
            colection = Program.game.GraphicsDevice.Adapter.SupportedDisplayModes;
            disp = colection.ToList<DisplayMode>();
            System.Console.WriteLine(colection.Count());
            displayMode.Clear();
            while (disp[0].Width != 800 || disp[0].Height != 600)
            {
                disp.RemoveAt(0);
            }
            System.Console.WriteLine(disp.Count());
            for (int i = 0; i < disp.Count(); i++)
            {
                string a = disp[i].Width.ToString() + "x" + disp[i].Height;
                Button but = new Button(Program.game.button, Program.game.Font1, new Rectangle(200, Program.game.graphics.PreferredBackBufferHeight / 2 + 20 * i, 200, 20), a);
                displayMode.Add(but);
            }
            
        }
        public void Render(SpriteBatch spriteBatch)
        {
            lab.Draw(spriteBatch);
            rb.Draw(spriteBatch);
            for (int i = 0; i < displayMode.Count(); i++)
            {
                displayMode[i].Draw(spriteBatch);   
            }
        }
        void SettingMenu_e_onClicked()
        {
        }
        public void Update(GameTime gameTime)
        {
            lab.Update();
            rb.Update();
            if(Program.game.graphics.IsFullScreen != rb.Status)
            Program.game.graphics.ToggleFullScreen();

            for (int i = 0; i < displayMode.Count(); i++)
            {
                displayMode[i].Update();
                if (displayMode[i].IsClicked())
                {
                    System.Console.WriteLine(displayMode[i].Text);
                    System.Console.WriteLine(disp[i].ToString());
                    Program.game.changeDisplayMode(disp[i]);
                    Load(scene);
                    break;
                }
            }
        }
        public void tryExit()
        {
            scene.returnScene();
        }

    }
}
