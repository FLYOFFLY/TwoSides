using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.GUI;

namespace TwoSides.GameContent.GUI.Scene
{
    public class SelectRace : IScene
    {
        public bool lastSceneRender { get; set; }
        public bool lastSceneUpdate { get; set; }
        ControlScene scene { get; set; }
        public void Load(ControlScene scene)
        {
            this.scene = scene;
            version = new Label(Program.game.getVersion(), new Vector2(0, Program.game.graphics.PreferredBackBufferHeight - (int)Program.game.Font1.MeasureString(Program.game.getVersion()).Y), Program.game.Font1, Color.Black);

        }
        public void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Race.racelist.Count; i++)
            {
                Race race = (Race)Race.racelist[i];
                // spriteBatch.DrawString(Font1, (i+1)+"-"+race.getName(), new Vector2(120, 120+10*i), Color.White);
                race.getButton().Draw(spriteBatch);
            }
            version.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Race.racelist.Count; i++)
            {

                Race race = (Race)Race.racelist[i];
                race.getButton().Update();
                if (race.getButton().IsClicked())
                {
                    Program.game.player.setCR(i + 1);
                    Program.game.player.clearclothes();
                    scene.changeScene(new CreationPerson());
                }
            }
        }
        public void tryExit()
        {
            scene.returnScene();
        }

        Label version { get; set; }
    }
}
