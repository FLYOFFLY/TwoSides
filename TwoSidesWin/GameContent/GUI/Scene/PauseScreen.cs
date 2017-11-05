using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TwoSides.GUI;
using TwoSides.GUI.Scene;

namespace TwoSides.GameContent.GUI.Scene
{
    public class PauseScreen : IScene
    {
        public bool LastSceneRender { get; set; }
        public bool LastSceneUpdate { get; set; }
        ControlScene _scene;
        public Button[] Buttons = new Button[6];
        public void Load(ControlScene scene)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = new Button(Program.Game.Button, Program.Game.Font1, new Rectangle(0, 0, 400, 400), "null");
                Rectangle rect = new Rectangle(120, Program.Game.HeightMenu + 27 * i, (int)Program.Game.Font1.MeasureString("Exit Game").X + 50, 25);

                Buttons[i].SetRect(rect);
            }
            LastSceneRender = true;
            _scene = scene;
            Buttons[0].Text = "Continue Game";
            Buttons[1].Text = "New Game";
            Buttons[2].Text = "Load Game";
            Buttons[3].Text = "Save Game";
            Buttons[4].Text = "Exit";
        }
        public void Render(SpriteBatch spriteBatch)
        {
            foreach ( Button button in Buttons )
                button.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up)) Program.Game.Camera.Pos.Y -= 8;
            else if (keyState.IsKeyDown(Keys.Down)) Program.Game.Camera.Pos.Y += 8;
            else if (keyState.IsKeyDown(Keys.Left)) Program.Game.Camera.Pos.X -= 8;
            else if (keyState.IsKeyDown(Keys.Right)) Program.Game.Camera.Pos.X += 8;
            foreach ( Button button in Buttons )
                button.Update();

            if (Buttons[0].IsClicked()) _scene.ReturnScene();
            else if (Buttons[1].IsClicked())
            {
                _scene.ChangeScene(new MainMenu());
            }
            else if (Buttons[2].IsClicked())
            {
                _scene.ChangeScene(Progress.Instance); 
                Program.Game.LoadMap();
            }
            else if (Buttons[3].IsClicked())
            {
                _scene.ChangeScene(Progress.Instance); 
                Program.Game.SaveMap();
            }
            else if (Buttons[4].IsClicked()) Program.Game.Exit();
        }
        public void TryExit() => _scene.ReturnScene();
    }
}
