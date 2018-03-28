using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using TwoSides.GUI;
using TwoSides.GUI.Scene;

namespace TwoSides.GameContent.GUI.Scene
{
    internal class MusicMenu: IScene
    {
        public bool LastSceneRender { get; set; }
        public bool LastSceneUpdate { get; set; }
        Label LableMusic { get; set; }
        ControlScene _scene;
        RadioButton _radioButtonMusicOn;
        public void Load(ControlScene scene)
        {
            LableMusic = new Label("Music on", new Vector2(Program.Game.Resolution.X,Program.Game.Resolution.Y)/2, Program.Game.Font1);

            _radioButtonMusicOn = new RadioButton(!MediaPlayer.IsMuted, Program.Game.Galka,
                Program.Game.Ramka, Program.Game.Font1,
                new Rectangle(Program.Game.Resolution.X / 2 + (int)Program.Game.Font1.MeasureString("Music On").X + 16,
                                Program.Game.Resolution.Y / 2 - 16, 32, 32));
            _scene = scene;
        }
        public void Render(Render render)
        {
            LableMusic.Draw(render);
            _radioButtonMusicOn.Draw(render);
        }
        public void Update(GameTime gameTime)
        {
            _radioButtonMusicOn.Update();
            MediaPlayer.IsMuted = !_radioButtonMusicOn.Status;
        }
        public void TryExit() => _scene.ReturnScene();
    }
}
