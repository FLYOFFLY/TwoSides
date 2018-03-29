using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using TwoSides.GameContent.Dimensions;
using TwoSides.GameContent.Entity;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.GUI.Scene;
using TwoSides.GameContent.Tiles;
using TwoSides.GUI;
using TwoSides.GUI.Scene;
using TwoSides.Utils;
using TwoSides.World;
using TwoSides.World.Generation;
using TwoSides.World.Tile;

using Console = TwoSides.GUI.HUD.Console;
using Drop = TwoSides.Physics.Entity.Drop;

namespace TwoSides
{
    /// <summary>
    /// Дальше идёт код без комментарий, поэтому читайте код, только про качки навыка догадливасти до максиума
    /// А лучше вообще не читайте, а то будете меня оскорблять 
    /// </summary>
    public sealed class Game1 : Game
    {
        #region Константы
        const string NAME_SUB_VERSION = "Alpha";
        const string NAME_VERSION = "Hammer & Anvil";
        const byte ID_SUB_VERSION = 0;
        const byte ID_VERSION = 0;
        const byte MAJOR = 0;
        const byte MINOR = 0;
        const byte BUILD = 3;
        const byte REVESION = 0;
        const int BRIGHTNESS = 51;

        public const int MAX_FRAME = 100;
        public const int MAX_SPECIAL_TEXTURE = 13;
        public const int MAX_POSTERS = 1;
        public const int MAX_DIMENSION = 3;
        public const int RECIPE_TYP = 8;
        public const int SIZE_CARMA_HEIGHT = 16;
        public const int SIZE_CARMA_WIDTH = 80;
        public const string IMAGE_FOLDER = @"Images/";
        public const string EFFECT_FOLDER = @"Effects\";
        public const bool IS_DEBUG = false;

        public const string CONFIG_FOLDER = @"Data/Config";
        public const string MODS_FOLDER = @"Data/Mods";
        public const string FONT_FOLDER = @"Fonts\";
        #endregion
        #region Поля

        static XnaLayout _guiRemove;
        static readonly IList<FlashText> Flash = new List<FlashText>();
        static readonly IList<Drop> Drops = new List<Drop>();
        readonly GraphicsDeviceManager _graphics;

        public int HeightMenu;
        public int Type = 0;
        public int CurrentDimension;
        public float Seconds;
        public Console Console;
        public IList<Bullet> Bullets = new List<Bullet>();
        public IList<XnaLayout> Guis = new List<XnaLayout>();
        public BaseDimension[] Dimension = new BaseDimension[MAX_DIMENSION];
        //Song music;
        public SpriteFont Font2;
        public Effect Effects;
        public KeyboardState KeyState = Keyboard.GetState();
        public SpriteFont Font1;
        public Random Rand = new Random();
        public Player Player = new Player();


        readonly Render _render;
        Texture2D _fog;
        Texture2D _cursor;
        Texture2D _shadow;
        Texture2D _head2;
        Texture2D _blood;
        static Texture2D _head;
        static Texture2D _eye;
        static Texture2D _body;
        static Texture2D _legs;
        readonly Texture2D[] _background = new Texture2D[3];
        readonly ControlScene _controlScene;
        readonly Log _log = new Log("time");
        readonly NamingVersion _versionThis = new NamingVersion(ID_SUB_VERSION, ID_VERSION, MAJOR, MINOR, BUILD, REVESION);
        public Texture2D Carma;
        public Texture2D[] Slots = new Texture2D[6];
        public Texture2D Achivement;
        public Texture2D Inv;
        public Texture2D Button;
        public Texture2D Black;
        public Texture2D Shootgun;
        public Texture2D Ramka;
        public Texture2D Galka;
        public TileTwoSides Tiles;
        Song _music;
        bool _activeConsole;
        bool _day = true;
        public Camera Camera = new Camera();
        public int SpawnTime;
        bool _isSnowing;
        Texture2D _snowTexture;
        Random _random;
        Texture2D _pigSkin;
        Texture2D _explosion;
        readonly Point[] _snow = new Point[50];
        readonly NamingVersion _versionMap = new NamingVersion(0, 0, 0, 0, 0, 0);
        #endregion

        #region свойства
        public Point Resolution
        {
            get => new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            set
            {
                _graphics.PreferredBackBufferWidth = value.X;
                _graphics.PreferredBackBufferHeight = value.Y;
                _graphics.ApplyChanges();
            }
        }

        public DisplayMode DisplayMode
        {
            get => _graphics.GraphicsDevice.DisplayMode;
            set
            {
                _graphics.PreferredBackBufferWidth = value.Width;
                _graphics.PreferredBackBufferHeight = value.Height;
                _graphics.PreferredBackBufferFormat = value.Format;
                _graphics.ApplyChanges();
            }
        }

        public bool IsFullScreen
        {
            get => _graphics.IsFullScreen;
            set
            {
                _graphics.IsFullScreen = value;
                _graphics.ApplyChanges();
            }
        }

        public bool InScreen(Vector2 coord) => new Rectangle(Point.Zero, Resolution).Contains(coord);

        public Texture2D FullDrougt { get; set; }

        public Texture2D EmptyDrougt { get; set; }

        public Texture2D EatHunger { get; set; }
        public SpriteFont Font3 { get; set; }

        public Texture2D BloodBody { get; set; }

        public Texture2D[] BloodTexture = new Texture2D[4];

        public Texture2D Hand { get; set; }

        public Texture2D WolfSkin { get; set; }

        public Effect PostEffect { get; set; }
        public Texture2D StoneTile { get; set; }
        public Texture2D Dialogtex { get; set; }
        #endregion
        #region методы
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1366,
                PreferredBackBufferHeight = 768,
                IsFullScreen = true
            };
            _controlScene = new ControlScene();
            LoadSetting();
            Content.RootDirectory = "Content";
            _log.HasData = true;
            Window.AllowAltF4 = true;
            _render = new Render();
        }

        /// <summary>
        /// Выводит на экран меню игрока в определенном маштабе
        /// </summary>
        /// <param name="scale">Маштаб</param>
        public void PlayerRender(float scale = 1.0f)
        {
            Player.RenderLeftPart(Hand, _render, scale);
            Player.Render(_eye, _head, _body, _legs, _render, scale);
        }

        /// <summary>
        /// Рисует на игроке, заданную текстуру с заданым маштабам
        /// </summary>
        /// <param name="texture">Текстура</param>
        /// <param name="scale">Маштаб</param>
        public void PlayerRenderTexture(Texture2D texture, float scale = 1.0f)
        {
            _render.Draw(texture, new Rectangle((int)(
                Player.Position.X + (Player.Width - _head.Width) - 0), (int)(Player.Position.Y - 0),
                (int)(_body.Width * scale), (int)(_body.Height * scale)), new Rectangle(0, 0, _body.Width, _body.Height),
                ColorScheme.BloodColor);
        }
        //Базовые методы игрового цикла
        protected override void Initialize()
        {
            IsMouseVisible = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _render.SetBatch(new SpriteBatch(GraphicsDevice));
            Localisation.LoadLocalisation(new[] { @"data/lang/en.lang", @"data/lang/ru.lang" });
            Localisation.SetLanguage(Localisation.Language.RU);
            LoadedGui();
            LoadedHuman();
            Clothes.Loadclothes(Content);
            Dust.LoadContent();
            LoadBackImage();
            Tiles = new TileTwoSides();
            Tiles.Loadtiles(Content);
            Item.LoadedItems(Content);
            Bullet.LoadBullet(Content);
            HeightMenu = _graphics.PreferredBackBufferHeight / 2 - 3 * 27;
            LoadStdRaces();
            CreateConsole();
            Recipe.LoadRecipe();
            //music = Content.Load<Song>("music");
            _fog = Content.Load<Texture2D>(IMAGE_FOLDER + "fog");
            //  postEffect = base.Content.Load<Effect>(EffectFolder + "NewSpriteEffect");
            Dimension[0] = new NormalWorld();
            Dimension[1] = new Hell();
            Dimension[2] = new SnowWorld();
            _controlScene.ChangeScene(new MainMenu());
        }
        protected override void Update(GameTime gameTime)
        {
            if (Player.TypeKill >= 0) Player.KillNpc(Player.TypeKill == 1);
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !KeyState.IsKeyDown(Keys.Escape))
                _controlScene.TryExit();
            KeyState = Keyboard.GetState();
            GameInput.UpdateMouse();
            _controlScene.Update(gameTime);
            foreach (XnaLayout mygui in Guis)
                mygui.Update();
            foreach (FlashText fl in Flash)
            {
                if (fl.IsInvisible())
                {
                    Flash.Remove(fl);
                    break;
                }
                fl.Update();
            }
            Guis.Remove(_guiRemove);
            base.Update(gameTime);
        }
        public void DrawText(string text, int x, int y)
        {
            DrawText(text, x, y, Color.White);
        }
        public void DrawText(string text, int x, int y, Color color)
        {
            _render.DrawString(Font1, text, new Vector2(x, y), color);
        }
        public void AddFlash(int x, int y, string text, Color color)
        {
            Flash.Add(new FlashText(new Vector2(x, y), text, Font1, color));
        }
        protected override void Draw(GameTime gameTime)
        {
            // ModLoader.callFunction("PreDraw");
            GraphicsDevice.Clear(new Color(228, 241, 255));
            //SpriteEffects effect = SpriteEffects.None;
            foreach (FlashText fl in Flash)
                fl.Draw(_render);
            /*     if (state == (int)State.Game || state == (int)State.Pause)
            {
               
            }
            else if (state != (int)State.Rpgsystem)
            {
                menu.Draw(spriteBatch);
            }
            if (state == (int)State.Game) spriteBatch.Begin();
            if (state == (int)State.MENU) MenuDraw(effect);
            if (state == (int)State.Game) GameDraw(effect);
            if (state == (int)State.Progress) GenerationsDraw(effect);
            if (state == (int)State.SelectRace) SRDraw(effect);
            if (state == (int)State.CreateGame) CreatePersonDraw(effect);
            if (state == (int)State.Pause) PauseDraw(effect);
            if (state == (int)State.Rpgsystem) rpgSystem(effect);
            if (state == (int)State.Game) spriteBatch.End();
            */
            _controlScene.Render(_render);
            foreach (XnaLayout mygui in Guis)
                mygui.Draw(_render);

            _render.Start();
            _render.Draw(_cursor, GameInput.GetMousePos(), ColorScheme.InterfaceColor);
            _render.End();
            base.Draw(gameTime);
        }
        protected override void UnloadContent()
        {
            SaveSetting();
            Content.Unload();
        }
        //Методы связанные с Initalize
        void LoadSetting()
        {
            Program.StartSw();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(CONFIG_FOLDER + @"/Graphics.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null, nameof(xRoot) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                switch (xnode.Name)
                {
                    case "Width":
                        _graphics.PreferredBackBufferWidth = int.Parse(xnode.InnerText, CultureInfo.InvariantCulture);
                        break;
                    case "Height":
                        _graphics.PreferredBackBufferHeight = int.Parse(xnode.InnerText, CultureInfo.InvariantCulture);
                        break;
                    case "FullScreen":

                        if (int.Parse(xnode.InnerText, CultureInfo.InvariantCulture) == 0)
                            _graphics.IsFullScreen = false;
                        break;
                    default:
                        System.Console.WriteLine(xnode.Name);
                        break;
                }
            }
            Program.StopSw("Init Graphics Setting");

            Program.StartSw();
            xDoc = new XmlDocument();
            xDoc.Load(CONFIG_FOLDER + @"/Sound.xml");
            xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null, nameof(xRoot) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                // ReSharper disable once InvertIf
                if (xnode.Name == "Mutted")
                {
                    System.Console.WriteLine(xnode.InnerText);
                    if (int.Parse(xnode.InnerText, CultureInfo.InvariantCulture) != 0) MediaPlayer.IsMuted = true;
                }
            }
            Program.StopSw("Init Sound Setting");
            LoadInput();
        }
        void SaveSetting()
        {
            Program.StartSw();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(CONFIG_FOLDER + @"/Graphics.xml");
            // получим корневой элемент

            XmlElement xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null, nameof(xRoot) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                switch (xnode.Name)
                {
                    case "Width":
                        xnode.InnerText = _graphics.PreferredBackBufferWidth.ToString(CultureInfo.InvariantCulture);
                        break;
                    case "Height":
                        xnode.InnerText = _graphics.PreferredBackBufferHeight.ToString(CultureInfo.InvariantCulture);
                        break;
                    case "FullScreen":
                        xnode.InnerText = _graphics.IsFullScreen ? "1" : "0";
                        break;
                    default:
                        System.Console.WriteLine(xnode.Name);
                        break;
                }
            }

            File.Delete(CONFIG_FOLDER + @"/Graphics.xml");
            xDoc.Save(CONFIG_FOLDER + @"/Graphics.xml");
            Program.StopSw("Save Graphics Setting");
            Program.StartSw();
            xDoc = new XmlDocument();
            xDoc.Load(CONFIG_FOLDER + @"/Sound.xml");
            // получим корневой элемент

            xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null, nameof(xRoot) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                // если узел - company
                // если узел - company
                if (xnode.Name != "Mutted") continue;

                xnode.InnerText = MediaPlayer.IsMuted ? "1" : "0";
            }

            File.Delete(CONFIG_FOLDER + @"/Sound.xml");
            xDoc.Save(CONFIG_FOLDER + @"/Sound.xml");
            System.Console.WriteLine("TEST");
            Program.StopSw("Save Graphics Setting");
        }

        static void LoadInput()
        {
            Program.StartSw();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(CONFIG_FOLDER + @"/Input.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null, nameof(xRoot) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine(xnode.Name);
                // если узел - company
                switch (xnode.Name)
                {
                    case "Jump":
                        GameInput.Jump = int.Parse(xnode.InnerText, CultureInfo.InvariantCulture);
                        break;
                    case "Left":
                        GameInput.MoveLeft = int.Parse(xnode.InnerText, CultureInfo.InvariantCulture);
                        break;
                    case "Right":
                        GameInput.MoveRight = int.Parse(xnode.InnerText, CultureInfo.InvariantCulture);
                        break;
                    case "ActiveIventory":
                        GameInput.ActiveIventory = int.Parse(xnode.InnerText, CultureInfo.InvariantCulture);
                        break;
                    case "Drop":
                        GameInput.Drop = int.Parse(xnode.InnerText, CultureInfo.InvariantCulture);
                        break;
                    default:
                        System.Console.WriteLine(xnode.Name);
                        break;
                }
            }
            Program.StopSw("Init Input Setting");
        }
        //Методы связанные с LoadContent
        void CreateConsole()
        {
            Program.StartSw();
            Console = new Console(Font1);
            Console.AddLog("Maximum:" + (Item.ITEM_MAX + TileTwoSides.TILE_MAX));
            Program.StopSw("Creating Console");
        }

        void LoadBackImage()
        {
            Black = Content.Load<Texture2D>(IMAGE_FOLDER + "black");
            Program.StartSw();
            for (var i = 0; i < 3; i++)
                _background[i] = Content.Load<Texture2D>(IMAGE_FOLDER + "background_" + i);

            StoneTile = Content.Load<Texture2D>(IMAGE_FOLDER + "stonetiles");
            Program.StopSw("Loaded Background");
        }
        void LoadedHuman()
        {
            Program.StartSw();
            _blood = Content.Load<Texture2D>(IMAGE_FOLDER + "blood");
            _head = Content.Load<Texture2D>(IMAGE_FOLDER + "head");
            Hand = Content.Load<Texture2D>(IMAGE_FOLDER + "hand");
            _eye = Content.Load<Texture2D>(IMAGE_FOLDER + "eyes");
            _head2 = Content.Load<Texture2D>(IMAGE_FOLDER + "head2");
            _body = Content.Load<Texture2D>(IMAGE_FOLDER + "body");
            _legs = Content.Load<Texture2D>(IMAGE_FOLDER + "legs");
            _shadow = Content.Load<Texture2D>(IMAGE_FOLDER + "shadow");
            Program.StopSw("Loaded Human");
        }
        void LoadedGui()
        {
            Program.StartSw();
            Button = Content.Load<Texture2D>(IMAGE_FOLDER + "button");
            Carma = Content.Load<Texture2D>(IMAGE_FOLDER + "carma");
            Achivement = Content.Load<Texture2D>(IMAGE_FOLDER + "achivement");
            _cursor = Content.Load<Texture2D>(IMAGE_FOLDER + "cursor");
            Inv = Content.Load<Texture2D>(IMAGE_FOLDER + "invsphere");
            Galka = Content.Load<Texture2D>(IMAGE_FOLDER + "galka");
            Ramka = Content.Load<Texture2D>(IMAGE_FOLDER + "ramka");
            EmptyDrougt = Content.Load<Texture2D>(IMAGE_FOLDER + @"hud\emptyDrougt");
            FullDrougt = Content.Load<Texture2D>(IMAGE_FOLDER + @"hud/fullDrougt");
            WolfSkin = Content.Load<Texture2D>(IMAGE_FOLDER + @"NPC/wolf");
            _pigSkin = Content.Load<Texture2D>(IMAGE_FOLDER + @"NPC/pig");
            EatHunger = Content.Load<Texture2D>(IMAGE_FOLDER + @"hud/hunger");
            _explosion = Content.Load<Texture2D>(IMAGE_FOLDER + @"explosion");
            _music = Content.Load<Song>("DestinationUnknown");
            _snowTexture = Content.Load<Texture2D>(IMAGE_FOLDER + "snow");
            BloodBody = Content.Load<Texture2D>(IMAGE_FOLDER + @"skelet/body");
            for (var i = 0; i < 4; i++)
                BloodTexture[i] = Content.Load<Texture2D>(IMAGE_FOLDER + @"skelet/" + i);
            Dialogtex = Carma;
            Program.StopSw("Loaded Image Gui (50 milisecond First load Content)");

            Program.StartSw();
            Font1 = Content.Load<SpriteFont>(FONT_FOLDER + "myfont");
            Font2 = Content.Load<SpriteFont>(FONT_FOLDER + "myfont2");
            Font3 = Content.Load<SpriteFont>(FONT_FOLDER + "myfont3");
            Program.StopSw("Loaded Font Gui");
        }
        //Методы меню
        public void LoadSlots()
        {
            for (var i = 0; i < 6; i++)
                Slots[i] = Content.Load<Texture2D>(IMAGE_FOLDER + "slot\\slot_" + i);
        }

        public void NewGeneration()
        {
            _controlScene.ChangeScene(Progress.Instance);
            for (var i = 0; i < 1; i++)
            {
                CurrentDimension = i;
                Dimension[i].Clear();
                Dimension[i].Start(Progress.Instance.Bar);
            }
            StartGame();
        }

        void StartGame()
        {
            Dimension[1].Globallighting = 1;
            Dimension[0].Globallighting = 51;
            CurrentDimension = 0;
            Player.Spawn();
            _controlScene.ChangeScene(new GameScene());
            MediaPlayer.Play(_music);
        }

        //Методы выборы Расы
        void LoadStdRaces()
        {
            Program.StartSw();
            Race.LoadRace(Font1, HeightMenu, Button, CONFIG_FOLDER + @"/racelist.xml", -1);
            Program.StopSw("Loaded default Race's");
        }
        //Методы игры
        public void GameUpdate(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalMinutes;
            Player.Update(delta, Seconds);
            Dimension[CurrentDimension].Update(gameTime, Camera);
            Camera.Pos.X = (int)Player.Position.X;
            Camera.Pos.Y = (int)Player.Position.Y;
            if (Camera.Pos.Y > (SizeGeneratior.WorldHeight - 1) * Tile.TILE_MAX_SIZE) Camera.Pos.Y = (SizeGeneratior.WorldHeight - 1) * Tile.TILE_MAX_SIZE;
            if (Camera.Pos.Y < _graphics.PreferredBackBufferHeight / 2.0f) Camera.Pos.Y = _graphics.PreferredBackBufferHeight / 2.0f;
            if (Camera.Pos.X > (SizeGeneratior.WorldWidth - 1) * Tile.TILE_MAX_SIZE) Camera.Pos.X = (SizeGeneratior.WorldWidth - 1) * Tile.TILE_MAX_SIZE;
            if (Camera.GetLeftUpper.X < 0) Camera.GetLeftUpper = new Vector2(0, Camera.GetLeftUpper.Y);
            Seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!_activeConsole && KeyState.IsKeyDown(Keys.OemTilde))
            {
                Console.Changeactive();
                _activeConsole = true;
            }
            else if (KeyState.IsKeyUp(Keys.OemTilde))
                _activeConsole = false;

            if (Console.Isactive) Console.Update();
            if (_day) Dimension[0].Globallighting += BRIGHTNESS / 24.0f * delta;
            else Dimension[0].Globallighting -= BRIGHTNESS / 24.0f * delta;
            if (Dimension[0].Globallighting >= BRIGHTNESS && _day) _day = false;
            else if (Dimension[0].Globallighting <= 0 && !_day) _day = true;
            if (SpawnTime > 0)
                SpawnTime--;
            else if (Rand.Next(0, 5 * ((NormalWorld)Dimension[0]).Npcs.Count + 1) == 0)
            {
                var spawnX = (int)(Player.Position.X + _graphics.PreferredBackBufferWidth) / Tile.TILE_MAX_SIZE;
                SpawnTime = 1000;
                if (Rand.Next(0, 100) == 0) ((NormalWorld)Dimension[0]).Npcs.Add(new Wolf(spawnX, WolfSkin));
                else ((NormalWorld)Dimension[0]).Npcs.Add(new Pig(spawnX, _pigSkin));
            }
            for (var i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].Move()) continue;

                Bullets[i].Destory(Dimension[CurrentDimension]);
                Bullets.RemoveAt(i);
                break;
            }
            for (var i = 0; i < Drops.Count; i++)
            {
                Drops[i].Update();
                if (!Player.Rect.Intersects(new Rectangle((int)Drops[i].Position.X, (int)Drops[i].Position.Y, 16,
                                                           16))) continue;

                Drops[i].GetSlot().IsEmpty = false;
                Player.SetSlot(Drops[i].GetSlot());
                Drops.RemoveAt(i);
                break;
            }
        }
        public void AddBulletToMouse(int x, int y)
        {
            Bullets.Add(new Bullet(new Vector2(x, y), Tools.Distance(new Vector2(x, y))));
        }
        //RenderTarget2D shadowSprite;
        public void GameDraw()
        {
            DrawBackgrounds();
            _render.Start(Camera, _graphics);
            var i0 = (int)(Camera.GetLeftUpper.X / Tile.TILE_MAX_SIZE - 1f);
            var i1 = (int)(Camera.GetRightDown.X / Tile.TILE_MAX_SIZE) + 2;
            var j0 = (int)(Camera.GetLeftUpper.Y / Tile.TILE_MAX_SIZE - 1f);
            var j1 = (int)(Camera.GetRightDown.Y / Tile.TILE_MAX_SIZE) + 2;
            Dimension[CurrentDimension].Draw(_render, Tiles, new Rectangle(i0, j0, i1 - i0, j1 - j0));
            // spriteBatch.DrawString(Font1, onetwo.ToString(), new Vector2(120, 130), Color.White)
            Player.Render(Hand, _eye, _head, _body, _legs, _render, _shadow);

            //  spriteBatch.Draw(legs2, new Rectangle((int)(player.position.X + (player.width - head.Width) - ScreenPosX), (int)(player.position.Y - ScreenPosY), legs.Width, legs.Height), new Rectangle(0, 0, legs.Width, legs.Height), Color.Red, 0, Vector2.Zero, effect, 0);

            foreach (Zombie npc in Program.Game.Dimension[Program.Game.CurrentDimension].Zombies)
                npc.RenderNpc(_render, Font1, _head, _head2, _body, _legs, _blood, _eye, Hand, _shadow);
            foreach (Civilian npc in Program.Game.Dimension[Program.Game.CurrentDimension].Civil)
                npc.RenderNpc(_render, Font1, _head, _body, _legs, Hand, _shadow);
            foreach (Drop drops in Drops)
                drops.Render(_render, (int)drops.Position.X, (int)drops.Position.Y);

            _isSnowing = ReferenceEquals(Dimension[CurrentDimension].MapBiomes[(int)Player.Position.X / Tile.TILE_MAX_SIZE], ArrayResource.Snow);
            DrawSnow();
            RenderLight(i0, i1, j0, j1);
            Player.RenderFog(_fog, _render);
            _render.Start();
            DrawHud();
            _render.End();
        }

        void DrawBackgrounds()
        {
            _render.Start(SamplerState.LinearWrap);
            Rectangle dest2 = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Rectangle src2 = new Rectangle((int)Camera.GetLeftUpper.X, (int)Camera.GetLeftUpper.Y, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _render.Draw(StoneTile, dest2, src2);
            //  spriteBatch.Draw(background[1], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.5f),Genworld.rocklayer-background[1].Height, background[1].Width, graphics.PreferredBackBufferHeight), Color.White);
            //   spriteBatch.Draw(background[2], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.8f), Genworld.rocklayer, background[2].Width, graphics.PreferredBackBufferHeight), Color.White);
            //spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            _render.End();
            // else if (CurrentDimension >= 1) GraphicsDevice.Clear(Color.Red);
            _render.Start(SamplerState.AnisotropicClamp);
            //spriteBatch.Draw(background[CurrentDimension], new Vector2(-ScreenPosX,-ScreenPosY), new Rectangle(0, 0, (CWorld.TileMaxX * Tile.TileMaxSize), (CWorld.TileMaxY * Tile.TileMaxSize)), Color.White);
            //  Rectangle dest = new Rectangle(-ScreenPosX, -ScreenPosY + CWorld.TileMaxY / 3 * Tile.TileMaxSize, CWorld.TileMaxX * Tile.TileMaxSize, (CWorld.TileMaxY - CWorld.TileMaxY / 3 - CWorld.TileMaxY / 3) * Tile.TileMaxSize);
            Rectangle dest = new Rectangle(0, (int)-Camera.Pos.Y + SizeGeneratior.WorldHeight / 3 * Tile.TILE_MAX_SIZE + 600 * Tile.TILE_MAX_SIZE,
                _graphics.PreferredBackBufferWidth, (SizeGeneratior.WorldHeight - SizeGeneratior.WorldHeight / 3 - SizeGeneratior.WorldHeight / 3 - 600) * Tile.TILE_MAX_SIZE);
            Rectangle src = new Rectangle(0, 0, 48, 1300);
            _render.Draw(_background[CurrentDimension], dest, src);
            //  spriteBatch.Draw(background[1], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.5f),Genworld.rocklayer-background[1].Height, background[1].Width, graphics.PreferredBackBufferHeight), Color.White);
            //   spriteBatch.Draw(background[2], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.8f), Genworld.rocklayer, background[2].Width, graphics.PreferredBackBufferHeight), Color.White);
            //spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            _render.End();
            DrawBackground(0.9f);
        }


        public void DrawSnow()
        {
            Point quitPoint = new Point(0,
                                        Dimension[CurrentDimension].MapHeight[0] * Tile.TILE_MAX_SIZE + _snowTexture.Height);

            //Set the snow's start point. It's the top of the screen minus the texture's height so it looks like it comes from somewhere, rather than appearing
            Point startPoint = new Point(0,
                                         (int)Camera.GetLeftUpper.Y - _snowTexture.Height);
            //If it's not supposed to be snowing, exit
            if (!_isSnowing)
                return;

            //This will be used as the index within our snow array
            int i;

            //NOTE: The following conditional is not exactly the best "initializer."
            //If snow has not been initialized
            if (_snow[0] == new Point(0, 0))
            {
                //Make the random a new random
                _random = new Random();

                //For every snow particle within our snow array,
                for (i = 0; i < _snow.Length; i++)
                {
                    //Give it a new, random x and y. This will give the illusion that it was already snowing
                    //and won't cluster the particles
                    _snow[i] = new Point(
                                         _random.Next((int)Camera.GetLeftUpper.X,
                                                        (int)(Camera.GetLeftUpper.X +
                                                            _graphics.PreferredBackBufferWidth
                                                                * Camera.Zoom - _snowTexture.Width)),
                                         _random.Next((int)Camera.GetLeftUpper.Y,
                                                        (int)(Camera.GetLeftUpper.Y +
                                                                _graphics.PreferredBackBufferHeight *
                                                                    Camera.Zoom))
                               );
                }
            }

            //Make the random a new random (again, if just starting)
            _random = new Random();

            //Go back to the beginning of the snow array
            i = 0;

            //Begin displaying the snow
            foreach (Point snowPnt in _snow)
            {
                //Get the exact rectangle for the snow particle
                Rectangle snowParticle = new Rectangle(
                    snowPnt.X, snowPnt.Y, _snowTexture.Width, _snowTexture.Height);

                _render.Draw(_snowTexture, snowParticle);

                _snow[i].Y += _random.Next(0, 5);

                if (_snow[i].Y >= quitPoint.Y)
                {
                    _snow[i] = new Point(
                       _random.Next((int)Camera.GetLeftUpper.X, (int)(Camera.GetLeftUpper.X + _graphics.PreferredBackBufferWidth * Camera.Zoom - _snowTexture.Width)),
                       startPoint.Y);
                }

                i++;
            }
        }

        void DrawBackground(float zoom)
        {
            _render.Start(SamplerState.AnisotropicWrap, Camera, _graphics);
            Rectangle src = new Rectangle(0, 0, (int)(SizeGeneratior.WorldWidth * Tile.TILE_MAX_SIZE / zoom), _background[2].Height);
            Rectangle dest = new Rectangle(-src.Width, Dimension[CurrentDimension].MapBiomes[(int)Player.Position.X / Tile.TILE_MAX_SIZE].MaxHeight * Tile.TILE_MAX_SIZE - (int)(src.Height * zoom), src.Width, src.Height);
            src.Width *= 2;
            dest.Width = src.Width;
            _render.Draw(_background[2], dest, src);
            _render.End();
        }
        //Сообственные методы

        void Load(object thread)
        {
            FileStream stream = File.OpenRead("save.sav");
            BinaryReader reader = new BinaryReader(stream);
            _versionMap.Load(reader);
            for (var d = 0; d < MAX_DIMENSION; d++)
            {
                CurrentDimension = d;
                Dimension[d].Clear();
                Dimension[d].Load(reader, Progress.Instance.Bar, _versionMap.GetCode());
            }
            Player.Load(reader);
            StartGame();
            reader.Close();
        }
        public void LoadMap()
        {
            ThreadPool.QueueUserWorkItem(Load);
        }

        void Save(object thread)
        {
            FileStream stream = File.Create("save.sav");
            BinaryWriter writer = new BinaryWriter(stream);
            _versionThis.Save(writer);
            for (var d = 0; d < MAX_DIMENSION; d++)
                Dimension[d].Save(writer, Progress.Instance.Bar);
            Player.Save(writer);
            writer.Close();
            _controlScene.ReturnScene();
        }
        public string GetVersion() => NAME_SUB_VERSION + " " + _versionThis.GetVersion();

        public string GetFullVersion() => "Two sides " + NAME_VERSION + " " + NAME_SUB_VERSION + " " + _versionThis.GetVersion();

        public void SaveMap()
        {
            ThreadPool.QueueUserWorkItem(Save);
        }

        void RenderLight(int i0, int i1, int j0, int j1)
        {
            for (var i = i0; i < i1; i++)
            {
                if (i < SizeGeneratior.WorldWidth && i >= 0) Dimension[Program.Game.CurrentDimension].UpdateMaxY(i);
                for (var j = j0; j < j1; j++)
                {
                    if (i > SizeGeneratior.WorldWidth - 1 || i < 0) continue;
                    if (j > SizeGeneratior.WorldHeight - 1 || j < 0) continue;
                    //dimension[CurrentDimension].globallighting = 1;
                    Dimension[CurrentDimension].MapTile[i, j].SetLight(i, j, (short)Dimension[CurrentDimension].Globallighting, SizeGeneratior.RockLayer);
                    Dimension[CurrentDimension].MapTile[i, j].UpdateLight(i, j);
                    var lighting = 255 - Dimension[CurrentDimension].MapTile[i, j].Light * 5;
                    _render.Draw(Black, new Vector2(Tile.TILE_MAX_SIZE * i, Tile.TILE_MAX_SIZE * j), ColorScheme.GenAlphaGradient(lighting/255.0f));
                }
            }
        }

        void DrawHud()
        {
            if (Player.ControlJ)
            {
                DrawSlotItem(Player.CurrentMaxSlot);
                for (var i = Player.SLOTMAX; i < Player.SLOTMAX + 3; i++)
                {
                    _render.Draw(Inv, new Rectangle((i - Player.SLOTMAX) * 32, _graphics.PreferredBackBufferHeight - 32, 32, 32));
                    if (!Player.Slot[i].IsEmpty)
                        Player.Slot[i].Render(_render, (i - Player.SLOTMAX) * 32 + 8, _graphics.PreferredBackBufferHeight - 32 + 8);
                }

                var a = 0;
                for (var i = 0; i < Recipe.Recipes.Count; i++)
                {
                    if (!Player.GetValidRecipes(i)) continue;

                    _render.Draw(Inv, new Rectangle(a * 32, 32 + -32, 32, 32), ColorScheme.NotActiveRecipe);
                    Recipe.Recipes[i].Slot.Render(_render, a * 32 + 16 - 8, 35 + (16 - 8) + -32);
                    for (var j = 0; j < Recipe.Recipes[i].GetSize(); j++)
                    {
                        _render.Draw(Inv, new Rectangle(a * 32, (j + 2) * 35 + -32, 32, 32), ColorScheme.ActiveRecipe);
                        Item.Render(_render, Recipe.Recipes[i].GetIngrident(j), a * 32 + 16 - 8, (j + 2) * 35 + (16 - 8) + -32);
                        _render.DrawString(Font1, Recipe.Recipes[i].GetSize(j).ToString(CultureInfo.CurrentCulture), new Vector2((a + 1) * 32 - 10, (j + 2) * 35 + (16 - 8) + -32));
                    }
                    a++;
                }
                DrawBloodBody(a * 32, 0);
            }
            else
            {
                DrawSlotItem(9);
                DrawBloodBody(0, 0);
            }
            var wcarma = (int)(10 + SIZE_CARMA_WIDTH * (0.01 * Player.Carma));
            var hptext = (int)(Player.Width * (Player.Blood / 5000));
            var color = (int)(200 * (0.01 * Player.Carma));
            if (!Player.MouseItem.IsEmpty) Player.MouseItem.Render(_render, (int)GameInput.GetMousePos().X + 16, (int)GameInput.GetMousePos().Y + 5);
            _render.Draw(Carma, new Rectangle(_graphics.PreferredBackBufferWidth - wcarma, _graphics.PreferredBackBufferHeight - SIZE_CARMA_HEIGHT, wcarma, SIZE_CARMA_HEIGHT));
            _render.DrawString(Font3, "Karma", new Vector2(_graphics.PreferredBackBufferWidth - wcarma, _graphics.PreferredBackBufferHeight - Font3.MeasureString("Karma").Y));

            _render.Draw(Carma, new Rectangle((int)Player.Position.X + Player.Width / 2 - hptext / 2, (int)Player.Position.Y - SIZE_CARMA_HEIGHT / 2, hptext, SIZE_CARMA_HEIGHT / 2), ColorScheme.BloodColor);
            // spriteBatch.DrawString(Font1, "Blood", new Vector2(graphics.PreferredBackBufferWidth / 2 - hptext / 2, Font1.MeasureString("Blood").Y/2), Color.White);

            //spriteBatch.Draw(carma, new Rectangle(graphics.PreferredBackBufferWidth - 16, 16, 16, 16 + (int)player.drought), Color.Blue);
            _render.Draw(FullDrougt, new Rectangle(_graphics.PreferredBackBufferWidth - 16, _graphics.PreferredBackBufferHeight - SIZE_CARMA_HEIGHT - 16, 16, 16), new Rectangle(0, 0, FullDrougt.Width, (int)(Player.Drought / 100 * FullDrougt.Height)));
            _render.Draw(EmptyDrougt, new Rectangle(_graphics.PreferredBackBufferWidth - 16, _graphics.PreferredBackBufferHeight - SIZE_CARMA_HEIGHT - 16, 16, 16));
            Vector2 sizeHunger = new Vector2(EatHunger.Width * (Player.Hunger / 100.0f), EatHunger.Height);
            _render.Draw(EatHunger, new Vector2(_graphics.PreferredBackBufferWidth - wcarma - sizeHunger.X, _graphics.PreferredBackBufferHeight - SIZE_CARMA_HEIGHT), new Rectangle(EatHunger.Width - (int)sizeHunger.X, 0, (int)sizeHunger.X, (int)sizeHunger.Y));

            foreach (Civilian npc in Dimension[CurrentDimension].Civil)
                npc.RenderDialog(_render);

            // string temperature = "Player Temperature:" + player.Temperature + "*c";
            // spriteBatch.DrawString(Font1, temperature, new Vector2(graphics.PreferredBackBufferWidth - (Font1.MeasureString(temperature).X) - 5, SizeCarmaHeight + 120), Color.White);
            _render.End();
            var y = 0;
            foreach (Quest quest in Player.Quests)
            {
                quest.Render(_render, new Vector2(_graphics.PreferredBackBufferWidth - 10, 150 + y));
                y += quest.GetQuestHeight();
            }
            _render.Start();
            if (!Console.Isactive) return;

            _render.End();
            Console.Draw(_render);
            _render.Start();
        }

        void DrawBloodBody(int xBody, int yBody)
        {
            _render.Draw(BloodBody, new Vector2(xBody, yBody));
            for (var i = 0; i < 4; i++)
                if (Player.Bloods[i]) _render.Draw(BloodTexture[i], new Vector2(xBody, yBody), ColorScheme.BloodColor);
        }

        void DrawSlotItem(int maxSlot)
        {
            var r = -1;
            var slotHover = -1;
            var centerSlot = _graphics.PreferredBackBufferWidth - 9 * 32;
            for (var i = 0; i < maxSlot; i++)
            {
                if (i % 9 == 0) r++;
                var xslot = i % 9 * 32;
                var yslot = 32 * r;
                ColorScheme cl = ColorScheme.BaseColor;
                if (i == Player.SelectedItem) cl = ColorScheme.Shadow;
                _render.Draw(Inv, new Rectangle(centerSlot + xslot, yslot, 32, 32), cl);
                if (Player.Slot[i].IsEmpty) continue;

                Player.Slot[i].Render(_render, centerSlot + xslot + 16 / 2, 16 / 2 + yslot);
                _render.DrawString(Font3, Player.Slot[i].Ammount.ToString(CultureInfo.CurrentCulture), new Vector2(centerSlot + (32 * (i % 9 + 1) - 5 - Font3.MeasureString(Player.Slot[i].Ammount.ToString(CultureInfo.CurrentCulture)).X), 32 - Font3.MeasureString(Player.Slot[i].Ammount.ToString(CultureInfo.CurrentCulture)).Y + yslot), Color.Black);

                if (new Rectangle(centerSlot + xslot, yslot, 32, 32).Contains(GameInput.GetMousePos()))
                    slotHover = i;
            }//одежда
            if (slotHover >= 1)
                DrawItemHelp(slotHover);
        }

        void DrawItemHelp(int i)
        {
            _render.DrawString(Font2, Player.Slot[i].GetName(), new Vector2(GameInput.GetMousePos().X + 16,
                    GameInput.GetMousePos().Y + 5),
                Player.Slot[i].GetColor());
            _render.DrawString(Font2, "DPS:" + Player.Slot[i].GetDamage() +
                "(" + Player.Slot[i].GetDamage() *
                (Player.Slot[i].Hp / 100) + ")",
                new Vector2(GameInput.GetMousePos().X + 16, GameInput.GetMousePos().Y + 15),
                Player.Slot[i].GetColor());
            _render.DrawString(Font2, "Power:" + Player.Slot[i].GetPower() +
                "(" + Player.Slot[i].GetPower() *
               (Player.Slot[i].Hp / 100) + ")",
                new Vector2(GameInput.GetMousePos().X + 16, GameInput.GetMousePos().Y + 25),
                Player.Slot[i].GetColor());
        }

        // ReSharper disable once UnusedMember.Local
        void Drawdebug()
        {
            var glint = (int)Dimension[CurrentDimension].Globallighting;
            var text = "brightness world:" + glint.ToString(CultureInfo.CurrentCulture);

            var height = "height:" + (SizeGeneratior.WorldHeight - (int)(Player.Position.Y / Tile.TILE_MAX_SIZE)).ToString(CultureInfo.CurrentCulture);
            _render.DrawString(Font1, text, new Vector2(_graphics.PreferredBackBufferWidth - text.Length * 7 - 5, SIZE_CARMA_HEIGHT + 20), Color.White);
            //_spriteBatch.DrawString(Font1, "FPS:" + GLOBAL_FPS.ToString(CultureInfo.CurrentCulture), new Vector2(_graphics.PreferredBackBufferWidth - 50, SizeCarmaHeight + 40), Color.White);
            _render.DrawString(Font1, height, new Vector2(_graphics.PreferredBackBufferWidth - height.Length * 7 - 5, SIZE_CARMA_HEIGHT + 60), Color.White);
            string dworld;
            switch (CurrentDimension)
            {
                case 0:
                    dworld = "World : Normal World";
                    break;
                case 1:
                    dworld = "World : Hell";
                    break;
                case 2:
                    dworld = "World : Snow World";
                    break;
                default:
                    dworld = "World : ???";
                    break;
            }
            _render.DrawString(Font1, dworld, new Vector2(_graphics.PreferredBackBufferWidth - dworld.Length * 7 - 5, SIZE_CARMA_HEIGHT + 80));
            var biotemp = "World Temperature:" + Dimension[CurrentDimension].GetTemperature((int)Player.Position.X / Tile.TILE_MAX_SIZE,
                (int)Player.Position.Y / Tile.TILE_MAX_SIZE) + "*c";
            _render.DrawString(Font1, biotemp, new Vector2(_graphics.PreferredBackBufferWidth - biotemp.Length * 7 - 5, SIZE_CARMA_HEIGHT + 100));
            Vector2 vec = Tools.GetTile((int)Player.Position.X, (int)Player.Position.Y);
            glint = Dimension[CurrentDimension].MapTile[(int)vec.X, (int)vec.Y].Light;
            text = "brightness Block:" + glint.ToString(CultureInfo.CurrentCulture);
            _render.DrawString(Font1, text, new Vector2(_graphics.PreferredBackBufferWidth - text.Length * 7 - 5, SIZE_CARMA_HEIGHT + 140));

            var position = "Player X:" + Player.Position.X + " position.y: " + Player.Position.Y;
            _render.DrawString(Font1, position, new Vector2(_graphics.PreferredBackBufferWidth - position.Length * 7 - 5, SIZE_CARMA_HEIGHT + 160));
        }

        public void AddGui(XnaLayout gui)
        {
            Guis.Add(gui);
        }

        public void RemoveGui(XnaLayout gui)
        {
            _guiRemove = gui;
        }

        public void AddDrop(int x, int y, Item slot, float dirx = 0)
        {
            Drops.Add(new Drop(slot, x, y, dirx));
        }

        public void AddExplosion(Vector2 pos)
        {
            Dimension[CurrentDimension].AddExplosion(pos, _explosion);
        }
        #endregion
    }
}