using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GUI.Scene;

namespace TwoSides.GameContent.GUI.Scene
{
    public class GameScene : IScene
    {
        ControlScene _scene;
        public bool LastSceneRender { get; set; }
        public bool LastSceneUpdate { get; set; }
        public void Load(ControlScene scene) =>_scene = scene;
        public void Render(Render render) => Program.Game.GameDraw();
        public void Update(GameTime gameTime) => Program.Game.GameUpdate(gameTime);
        public void TryExit() => _scene.ChangeScene(new PauseScreen());
    }
}
