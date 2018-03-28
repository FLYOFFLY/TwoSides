using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GUI;
using TwoSides.GUI.Scene;

namespace TwoSides.GameContent.GUI.Scene
{
    public class SettingMenu : IScene
    {
        public bool LastSceneRender { get; set; }
        public bool LastSceneUpdate { get; set; }
        ControlScene _scene;
        readonly Button[] _buttons = new Button[2];
        public void Load(ControlScene scene)
        {
            var sizeButton = (int)Program.Game.Font1.MeasureString("Exit Game").X + 50;
            for (var i = 0; i < _buttons.Length; i++)
            {
                _buttons[i] = new Button(Program.Game.Button, Program.Game.Font1, new Rectangle(0, 0, 400, 400), "null");
                Rectangle rect = new Rectangle(Program.Game.Resolution.X / 2-sizeButton/2, Program.Game.HeightMenu + 35 * i, sizeButton, 30);
                _buttons[i].SetRect(rect);
            }
            _buttons[0].Text = "Graphics";
            _buttons[0].OnClicked += (_ , __) => {_scene.ChangeScene(new GraphicsMenu());};
            _buttons[1].Text = "Music";
            _buttons[1].OnClicked += (_, __) => { _scene.ChangeScene(new MainMenu()); };
            _scene = scene;
        }
        public void Render(Render render)
        {
            foreach ( Button button in _buttons )
                button.Draw(render);
        }
        public void Update(GameTime gameTime)
        {
            foreach ( Button button in _buttons )
                button.Update();
        }
        public void TryExit() => _scene.ReturnScene();
    }
}
