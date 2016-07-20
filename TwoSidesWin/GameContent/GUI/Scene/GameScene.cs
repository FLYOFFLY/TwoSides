using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoSides.GUI.Scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.GameContent.GUI.Scene
{
    public class GameScene : IScene
    {
        public bool lastSceneRender { get; set; }
        public bool lastSceneUpdate { get; set; }
        ControlScene scene;
        public void Load(ControlScene scene)
        {
            this.scene = scene;
        }
        public void Render(SpriteBatch spriteBatch)
        {
            Program.game.GameDraw(SpriteEffects.None);
        }
        public void Update(GameTime gameTime)
        {
            Program.game.GameUpdate(gameTime);
        }
        public void tryExit()
        {
            scene.changeScene(new PauseScreen());
        }
    }
}
