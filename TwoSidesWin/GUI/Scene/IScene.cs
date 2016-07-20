using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoSides.GUI.Scene
{
    public interface IScene
    {
        bool lastSceneRender { get; set; }
        bool lastSceneUpdate{ get; set; }
        void Load(ControlScene scene);
        void Render(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void tryExit();
    }
}
