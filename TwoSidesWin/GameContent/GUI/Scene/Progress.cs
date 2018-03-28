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
            Bar = new ProgressBar(Game1.SIZE_CARMA_HEIGHT ,Program.Game.Resolution.Y - Game1.SIZE_CARMA_HEIGHT -
                                  (int) Program.Game.Font1.MeasureString("1.1").Y ,Game1.SIZE_CARMA_WIDTH ,
                                  Program.Game.Resolution.X - Game1.SIZE_CARMA_WIDTH ,
                                  SizeGeneratior.WorldWidth , null , Color.Black);
            Version = new Label(Program.Game.GetVersion() ,
                                new Vector2(0 ,
                                            Program.Game.Resolution.Y -
                                            (int) Program.Game.Font1.MeasureString(Program.Game.GetVersion()).Y) ,
                                Program.Game.Font1 , Color.Black);

        }

        public void Render(Render render) {
            Bar.Render(Program.Game.Carma, render);
            Version.Draw(render);
        }
        public void Update(GameTime gameTime){ }

        public void TryExit(){ }
    }
}
