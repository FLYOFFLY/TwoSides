using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GUI;
using TwoSides.GUI.Scene;

using Console = System.Console;

namespace TwoSides.GameContent.GUI.Scene
{
    public class GraphicsMenu : IScene
    {
        public bool LastSceneRender { get; set; }
        public bool LastSceneUpdate { get; set; }
        ControlScene _scene;
        Label _labelFullScreen;
        RadioButton _activeFullScreen;

        DisplayModeCollection _colection;
        readonly List<Button> _displayMode;
        List<DisplayMode> _disp;
        public GraphicsMenu()
        {
            _displayMode = new List<Button>();
            _disp = new List<DisplayMode>();
        }

        public void Load(ControlScene scene)
        {
           _labelFullScreen = new Label("Full screen",new Vector2(Program.Game.Resolution.X/ 2.0f, Program.Game.Resolution.Y / 2.0f),
               Program.Game.Font1);

            _activeFullScreen = new RadioButton(Program.Game.IsFullScreen,Program.Game.Galka,Program.Game.Ramka,Program.Game.Font1,
                    new Rectangle(Program.Game.Resolution.X/2+(int)Program.Game.Font1.MeasureString("Full screen").X+16,
                                    Program.Game.Resolution.Y/2-16,32,32));
            _scene = scene;
            _colection = Program.Game.GraphicsDevice.Adapter.SupportedDisplayModes;
            _disp = _colection.ToList();
            _displayMode.Clear();
            while (_disp[0].Width != 800 || _disp[0].Height != 600)
            {
                _disp.RemoveAt(0);
            }
            for (int i = 0; i < _disp.Count; i++)
            {
                int j = i;
                string a = _disp[i].Width.ToString(CultureInfo.CurrentCulture) + "x" + _disp[i].Height;
                Button but = new Button(Program.Game.Button, Program.Game.Font1, new Rectangle(200, Program.Game.Resolution.Y / 2 + 20 * i, 200, 20), a);
                but.OnClicked += (_, _a) =>
                {
                    Console.WriteLine(_displayMode[j].Text);
                    Console.WriteLine(_disp[j].ToString());
                    Program.Game.DisplayMode = _disp[j];
                    Load(_scene);
                };
                _displayMode.Add(but);
                
            }
        }
        public void Render(SpriteBatch spriteBatch)
        {
            _labelFullScreen.Draw(spriteBatch);
            _activeFullScreen.Draw(spriteBatch);
            foreach ( Button button in _displayMode )
                button.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            _labelFullScreen.Update();
            _activeFullScreen.Update();
            if ( Program.Game.IsFullScreen != _activeFullScreen.Status )
                Program.Game.IsFullScreen = !Program.Game.IsFullScreen;

            for (int i = 0; i < _displayMode.Count; i++)
            {
                _displayMode[i].Update();
            }
        }
        public void TryExit()
        {
            _scene.ReturnScene();
        }
    }
}
