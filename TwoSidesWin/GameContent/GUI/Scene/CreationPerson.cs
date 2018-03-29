using System;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TwoSides.GUI;
using TwoSides.GUI.Scene;
using TwoSides.Utils;

using static System.GC;

namespace TwoSides.GameContent.GUI.Scene
{
    internal class CreationPerson : IScene,IDisposable
    {
        public bool LastSceneRender { get; set; }
        public bool LastSceneUpdate { get; set; }
        Button _buttonCreatePerson;
        TextField _nameInput;
        Label _nameLabel;
        bool _keyDownVertical;
        Label Version { get; set; }
        ControlScene Scene { get; set; }
        Image _image;
        Texture2D _pallete;
        public void Load(ControlScene scene)
        {
            _pallete = Program.Game.Content.Load<Texture2D>(Game1.IMAGE_FOLDER + "pallete");
            _image = new Image(_pallete,
                new Rectangle(0,
                   0,
                    _pallete.Width,
                    _pallete.Height));

            _nameLabel = new Label("InputName",
                new Vector2(150, Program.Game.HeightMenu + 35),
                Program.Game.Font1, Color.Black);
            _nameInput = new TextField(new Vector2(20, 0),
                Program.Game.Font1, Color.Black);
            _nameInput.SetPatern(_nameLabel);
            _nameInput.SetPos(new Vector2(0, 27));

            _buttonCreatePerson = new Button(Program.Game.Button, Program.Game.Font1, new Rectangle(0, 0, 100, 30), "null");
            _buttonCreatePerson.SetPatern(_nameInput);
            _buttonCreatePerson.SetRect(new Rectangle(0, 35, (int)Program.Game.Font1.MeasureString(_buttonCreatePerson.Text).X+30, 30));
            _image.SetPatern(_nameInput);
            _image.SetPos(new Vector2((int)Program.Game.Font1.MeasureString(_buttonCreatePerson.Text).X +60, 0));

            Scene = scene;
            Program.Game.LoadSlots();
            Program.Game.Player.Position.X = 100;
            Program.Game.Player.Position.Y = 255;
            Version = new Label(Program.Game.GetVersion(), new Vector2(0, Program.Game.Resolution.Y - (int)Program.Game.Font1.MeasureString(Program.Game.GetVersion()).Y), Program.Game.Font1, Color.Black);

            void CreateWorld(object o , EventArgs args)
            {
                Program.Game.Player.Name = _nameInput.GetText() ?? "Player";
                ThreadPool.QueueUserWorkItem(state => Program.Game.NewGeneration());
            }

            _buttonCreatePerson.OnClicked += CreateWorld;
        }

        int _currentSlot;
        bool _keyDownHorizontal;
        public void Render(Render render)
        {
            Version.Draw(render);
            _nameInput.Draw(render);
            _nameLabel.Draw(render);
            _buttonCreatePerson.Draw(render);
            render.Start();
            render.DrawString(Program.Game.Font1, Localisation.GetName("tip0"), new Vector2(150, 180), Color.Black);
            render.DrawString(Program.Game.Font1, Localisation.GetName("tip1"), new Vector2(150, 200), Color.Black);
            Program.Game.PlayerRender(2);
            Program.Game.PlayerRenderTexture(Program.Game.Slots[_currentSlot],2);
            
           render.End();
            _image.Draw(render);
        }
        public void TryExit() => Scene.ReturnScene();
        public void Update(GameTime gameTime)
        {
           KeyboardState keyState = Keyboard.GetState();
            _nameInput.Update();

            //  NameInput.rightsort(); 
            if (keyState.IsKeyDown(Keys.Down) && !_keyDownVertical && _currentSlot < 5)
            {
                _currentSlot++; _keyDownVertical = true;
            }
            else if (keyState.IsKeyDown(Keys.Up) && !_keyDownVertical && _currentSlot > 0)
            {
                _currentSlot--; _keyDownVertical = true;
            }
            else if (keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.Down)) _keyDownVertical = false;
            //

            if (keyState.IsKeyDown(Keys.Right) && !_keyDownHorizontal)
            {
                var type = false;
                var currentClothesSlot = TryChange(ref type);
                if (type)
                {
                    _keyDownHorizontal = true;
                    Program.Game.Player.Clslot[_currentSlot] = new Clothes(currentClothesSlot + 1);
                }
            }
            if (keyState.IsKeyDown(Keys.Left) && !_keyDownHorizontal)
            {
                var currentitem = Program.Game.Player.Clslot[_currentSlot].GetId();
                if ( currentitem > 0 ) Program.Game.Player.Clslot[_currentSlot] = new Clothes(currentitem - 1);
                else Program.Game.Player.Clslot[_currentSlot] = new Clothes();
                _keyDownHorizontal = true;
            }
            if (keyState.IsKeyUp(Keys.Right) && keyState.IsKeyUp(Keys.Left)) _keyDownHorizontal = false;
            //
            _buttonCreatePerson.Update();
            MouseState ms = Mouse.GetState();
            if ( !_image.InHover(ms) || ms.LeftButton != ButtonState.Pressed || _currentSlot < 1 ) return;

            Vector2 pos = ms.Position.ToVector2()-_image.GetPos();
            Program.Game.Player.Colors[_currentSlot-1] =new  ColorScheme(_image[(int)pos.X, (int)pos.Y]);
        }

        int TryChange(ref bool type)
        {
            var currentitem = Program.Game.Player.Clslot[_currentSlot].GetId();
            switch ( _currentSlot ) {
                case 0 when currentitem + 1 < Clothes.MaxHair:
                case 1 when currentitem + 1 < Clothes.MaxShirt:
                case 2 when currentitem + 1 < Clothes.MaxPants:
                case 3 when currentitem + 1 < Clothes.MaxShoes:
                case 4 when currentitem + 1 < Clothes.MaxBelt:
                case 5 when currentitem + 1 < Clothes.MaxGlove:
                    type = true;
                    break;
            }
            return currentitem;
        }

        #region IDisposable Support

        bool _disposedValue; 

        protected virtual void Dispose(bool disposing)
        {
            if ( _disposedValue ) return;

            if (disposing)
            {
                _pallete.Dispose();
            }
            _disposedValue = true;
        }
        
        public void Dispose()
        {
            Dispose(true);
            SuppressFinalize(this);
        }
        #endregion
    }
}
