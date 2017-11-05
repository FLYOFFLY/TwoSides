using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GUI;
using TwoSides.GUI.Scene;

using static TwoSides.Race;

namespace TwoSides.GameContent.GUI.Scene
{
    public class SelectRace : IScene
    {
        public bool LastSceneRender { get; set; }
        public bool LastSceneUpdate { get; set; }
        ControlScene Scene { get; set; }
        public void Load(ControlScene scene)
        {
            Scene = scene;
            Version = new Label(Program.Game.GetVersion(), new Vector2(0, Program.Game.Resolution.Y - (int)Program.Game.Font1.MeasureString(Program.Game.GetVersion()).Y), Program.Game.Font1, Color.Black);

        }
        public void Render(SpriteBatch spriteBatch)
        {
            foreach ( Race race in Racelist ) {
                // spriteBatch.DrawString(Font1, (i+1)+"-"+race.getName(), new Vector2(120, 120+10*i), Color.White);
                race.GetButton().Draw(spriteBatch);
            }

            Version.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Racelist.Count; i++)
            {
                // ReSharper disable once ExceptionNotDocumentedOptional
                Racelist[i].GetButton().Update();
                // ReSharper disable once ExceptionNotDocumentedOptional
                // ReSharper disable once ExceptionNotDocumentedOptional
                if ( !Racelist[i].GetButton().IsClicked() ) continue;

                Program.Game.Player.SetColorRace((RaceType)(i + 1));
                Program.Game.Player.ClearClothes();
                Scene.ChangeScene(new CreationPerson());
            }
        }
        public void TryExit() => Scene.ReturnScene();

        Label Version { get; set; }
    }
}
