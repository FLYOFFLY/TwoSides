using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI.Scene
{
    public interface IScene
    {
        bool LastSceneRender { get; set; }
        bool LastSceneUpdate{ get; set; }
        void Load(ControlScene scene);
        void Render(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void TryExit();
    }
}
