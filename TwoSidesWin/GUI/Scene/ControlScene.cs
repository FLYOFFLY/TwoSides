using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace TwoSides.GUI.Scene
{
    public class ControlScene
    {
        IScene sceneCurrent;
        IScene sceneNew;
        List<IScene> sceneBack = new List<IScene>();
        public void Render(SpriteBatch spriteBatch)
        {
            if (sceneCurrent.lastSceneRender && sceneBack.Count>=1)
                sceneBack[sceneBack.Count - 1].Render(spriteBatch);
            sceneCurrent.Render(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            if (sceneCurrent != sceneNew)
            {
                sceneCurrent = sceneNew;
                sceneCurrent.Load(this);
            }
            if (sceneCurrent.lastSceneUpdate && sceneBack.Count >= 1) sceneBack[sceneBack.Count - 1].Update(gameTime);
            sceneCurrent.Update(gameTime);
        }
        public void changeScene(IScene newScene)
        {
            this.sceneNew = newScene;
            if(sceneCurrent != null) sceneBack.Add(sceneCurrent);             
            if (sceneCurrent == null)
            {
                sceneCurrent = sceneNew;
                sceneCurrent.Load(this);
            }
        }
        public void tryExit()
        {
            sceneCurrent.tryExit();
        }
        public void returnScene()
        {
            this.sceneNew = sceneBack[sceneBack.Count-1];
            sceneBack.RemoveAt(sceneBack.Count - 1);
        }
    }
}
