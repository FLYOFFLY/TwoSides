﻿using Microsoft.Xna.Framework;
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

            for (var i = 0; i < Racelist.Count; i++)
            {
                var j = i;
                // ReSharper disable once ExceptionNotDocumentedOptional
                Racelist[i].GetButton().OnClicked += (_, _a) =>
                {

                    Program.Game.Player.SetColorRace((RaceType)(j + 1));
                    Program.Game.Player.ClearClothes();
                    Scene.ChangeScene(new CreationPerson());
                };
            }
        }
        public void Render(Render render)
        {
            foreach ( Race race in Racelist ) {
                // spriteBatch.DrawString(Font1, (i+1)+"-"+race.getName(), new Vector2(120, 120+10*i), Color.White);
                race.GetButton().Draw(render);
            }

            Version.Draw(render);
        }
        public void Update(GameTime gameTime)
        {
            foreach (Race race in Racelist)
            {
                race.GetButton().Update();
            }
        }
        public void TryExit() => Scene.ReturnScene();

        Label Version { get; set; }
    }
}
