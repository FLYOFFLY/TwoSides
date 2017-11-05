using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI.Scene
{
    public class ControlScene
    {
        IScene _sceneCurrent;
        IScene _sceneNew;
        readonly Stack<IScene> _sceneBack = new Stack< IScene >();

        public void Render(SpriteBatch spriteBatch)
        {
            if (_sceneCurrent.LastSceneRender && _sceneBack.Count>=1)
                _sceneBack.Peek().Render(spriteBatch);
            _sceneCurrent.Render(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            if (_sceneCurrent != _sceneNew)
            {
                _sceneCurrent = _sceneNew;
                _sceneCurrent.Load(this);
            }
            if (_sceneCurrent.LastSceneUpdate && _sceneBack.Count >= 1) _sceneBack.Peek().Update(gameTime);
            _sceneCurrent.Update(gameTime);
        }
        public void ChangeScene(IScene newScene)
        {
            _sceneNew = newScene;
            if(_sceneCurrent != null) _sceneBack.Push(_sceneCurrent);
            if ( _sceneCurrent != null ) return;

            _sceneCurrent = _sceneNew;
            _sceneCurrent.Load(this);
        }
        public void TryExit() => _sceneCurrent.TryExit();

        public void ReturnScene() => _sceneNew = _sceneBack.Pop();
    }
}
