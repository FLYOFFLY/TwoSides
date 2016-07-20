using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.GUI;
using TwoSides.GameContent.GenerationResources;

namespace TwoSides.GameContent.GUI.Scene
{
    public class Progress : IScene
    {
        public static Progress instance = new Progress();
        public bool lastSceneRender{get;set;}
        public bool lastSceneUpdate { get; set; }
        Label version { get; set; }
        public ProgressBar bar;
        public void Load(ControlScene scene)
        {
            bar = new ProgressBar(Game1.SizeCarmaHeight,
                       Program.game.graphics.PreferredBackBufferHeight - Game1.SizeCarmaHeight-(int)Program.game.Font1.MeasureString("1.1").Y,
                      Game1.SizeCarmaWidth,
                      Program.game.graphics.PreferredBackBufferWidth - Game1.SizeCarmaWidth,
                      SizeGeneratior.WorldWidth, null,Color.Black);
            version = new Label(Program.game.getVersion(), new Vector2(0, Program.game.graphics.PreferredBackBufferHeight - (int)Program.game.Font1.MeasureString(Program.game.getVersion()).Y), Program.game.Font1, Color.Black);

        }
        public void Render(SpriteBatch spriteBatch) {
            bar.Render(Program.game.carma, spriteBatch);
            version.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
        }
        public void tryExit()
        {
        }
    }
}
