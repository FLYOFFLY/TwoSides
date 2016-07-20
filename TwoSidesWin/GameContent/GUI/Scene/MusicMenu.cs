using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI;
using TwoSides.GUI.Scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace TwoSides.GameContent.GUI.Scene
{
    class MusicMenu: IScene
    {
        public bool lastSceneRender { get; set; }
        public bool lastSceneUpdate { get; set; }
        Label lab { get; set; }
        ControlScene scene;
        RadioButton rbtn;
        public void Load(ControlScene scene)
        {
            lab = new Label("Music on", new Vector2(Program.game.graphics.PreferredBackBufferWidth / 2, Program.game.graphics.PreferredBackBufferHeight / 2), Program.game.Font1);

            rbtn = new RadioButton(!MediaPlayer.IsMuted, Program.game.galka,
                Program.game.ramka, Program.game.Font1,
                new Rectangle(Program.game.graphics.PreferredBackBufferWidth / 2 + (int)Program.game.Font1.MeasureString("Music On").X + 16, Program.game.graphics.PreferredBackBufferHeight / 2 - 16, 32, 32));
            this.scene = scene;
        }
        public void Render(SpriteBatch spriteBatch)
        {
            lab.Draw(spriteBatch);
            rbtn.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            rbtn.Update();
            MediaPlayer.IsMuted = !rbtn.Status;
        }
        public void tryExit()
        {
            scene.returnScene();
        }

    }
}
