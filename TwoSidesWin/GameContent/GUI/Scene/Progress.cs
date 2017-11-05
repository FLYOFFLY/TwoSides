using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GameContent.GenerationResources;
using TwoSides.GUI;
using TwoSides.GUI.Scene;

namespace TwoSides.GameContent.GUI.Scene
{
    public class Progress : IScene
    {
        static Progress _instance;

        public static Progress Instance => _instance ?? (_instance = new Progress());

        public bool LastSceneRender{get;set;}

        public bool LastSceneUpdate { get; set; }

        Label Version { get; set; }

        public ProgressBar Bar { get; private set; }

        public void Load(ControlScene scene)
        {
            Bar = new ProgressBar(Game1.SizeCarmaHeight ,Program.Game.Resolution.Y - Game1.SizeCarmaHeight -
                                  (int) Program.Game.Font1.MeasureString("1.1").Y ,Game1.SizeCarmaWidth ,
                                  Program.Game.Resolution.X - Game1.SizeCarmaWidth ,
                                  SizeGeneratior.WorldWidth , null , Color.Black);
            Version = new Label(Program.Game.GetVersion() ,
                                new Vector2(0 ,
                                            Program.Game.Resolution.Y -
                                            (int) Program.Game.Font1.MeasureString(Program.Game.GetVersion()).Y) ,
                                Program.Game.Font1 , Color.Black);

        }

        public void Render(SpriteBatch spriteBatch) {
            Bar.Render(Program.Game.Carma, spriteBatch);
            Version.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime){ }

        public void TryExit(){ }
    }
}
