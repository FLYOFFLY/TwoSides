using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GUI;
using TwoSides.GUI.Scene;

namespace TwoSides.GameContent.GUI.Scene
{
    public class MainMenu : IScene
    {
        public bool LastSceneRender{get;set;}
        public bool LastSceneUpdate{get;set;}
        readonly Button[] _buttons;
        ControlScene _scene;
        Label _version;
        Image _logo;
        public MainMenu() => _buttons = new Button[4];

        public void Load(ControlScene scene)
        {
            InitElements(Program.Game.Resolution);
            _scene = scene;
        }
        void InitElements(Point resolution)
        {
            int y = resolution.Y / 3 / 2;
            int x = y * (887 / 133);
            int sizeButton = (int)Program.Game.Font1.MeasureString("StartServer").X + 50;
            int xButton = x - sizeButton;

            _logo = new Image(Program.Game.Content.Load<Texture2D>(Game1.ImageFolder + "header"),
                new Rectangle(resolution.X/ 2 - x / 2,0, x,y));
            _version = new Label(Program.Game.GetVersion(),
                                 new Vector2(0,resolution.Y -(int)Program.Game.Font1.MeasureString(Program.Game.GetVersion()).Y),
                                 Program.Game.Font1, Color.Black);

            void InitButton()
            {

                for (int i = 0; i < _buttons.Length; i++)
                {
                    _buttons[i] = new Button(Program.Game.Button, Program.Game.Font1,
                                             new Rectangle(0, 0, sizeButton, 30),
                                             "null");
                    if (i == 0) _buttons[i].SetPatern(_logo);
                    else
                    {
                        _buttons[i].SetPatern(_buttons[i - 1]);
                        _buttons[i].SetPos(i % 2 == 1 ? new Vector2(xButton, 0) : new Vector2(-xButton, 40));
                    }
                }
            }

            void LocalSetButton()
            {
                _buttons[0].SetPos(new Vector2(0, y + sizeButton / 2.0f));
                _buttons[0].OnClicked += (sender, e) => { _scene.ChangeScene(new SelectRace()); };
                _buttons[0].Text = "NewGame";

                _buttons[1].OnClicked += (sender, e) => { _scene.ChangeScene(new SettingMenu()); };
                _buttons[1].Text = "Setting";

                _buttons[2].OnClicked += (sender, e) =>
                {
                    _scene.ChangeScene(Progress.Instance);
                    Program.Game.LoadMap();
                };
                _buttons[2].Text = "LoadGame";

                _buttons[3].OnClicked += (sender, e) => { Program.Game.Exit(); };
                _buttons[3].Text = "ExitGame";
            }

            InitButton();
            LocalSetButton();
        }



        public void Render(SpriteBatch spriteBatch) {
            foreach ( Button button in _buttons )
                button.Draw(spriteBatch);
            _version.Draw(spriteBatch);
            _logo.Draw(spriteBatch);
           //bar.Render(Program.game.carma, spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            foreach ( Button button in _buttons )
                button.Update();
        }
        public void TryExit() => Program.Game.Exit();
    }
}
