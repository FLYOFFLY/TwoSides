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
        //Константы
        const int BRIGHTNESS = 51;
        public const int MaxFrame = 100;
        public const int MaxSpecialTexture = 13;
        public const int MaxPosters = 1;
        public const int MaxDimension = 3;
        public const int RecipeTyp = 8;
        public const int SizeCarmaHeight = 16;
        public const int SizeCarmaWidth = 80;

        //Переменные
        public int HeightMenu;
        public int Type = 0;
        public int CurrentDimension;
        public float Seconds;
        public Console Console;

        //Листы
        static readonly List<FlashText> Flash = new List<FlashText>();
        static readonly List<Drop> Drops = new List<Drop>();
        public List<Bullet> Bullets = new List<Bullet>();
        public List<XnaLayout> Guis = new List<XnaLayout>();
        //Массивы
        public BaseDimension[] Dimension = new BaseDimension[MaxDimension];
        //Объекты
        SpriteBatch _spriteBatch;
        //Song music;
        public SpriteFont Font2;

        static XnaLayout _guiRemove;
        public Effect Effects;
        public KeyboardState KeyState = Keyboard.GetState();
        readonly GraphicsDeviceManager _graphics;
        public SpriteFont Font1;
        public Random Rand = new Random();
        public Player Player = new Player();
        public MouseState MouseState = Mouse.GetState();

        //Текстуры
        Texture2D _fog;
        Texture2D _cursor;
        Texture2D _shadow;
        static Texture2D _head;
        static Texture2D _eye;
        static Texture2D _body;
        static Texture2D _legs;
        public Texture2D Carma;
        Texture2D _head2;
        Texture2D _blood;
        public Texture2D[] Slots = new Texture2D[6];
        readonly Texture2D[] _background = new Texture2D[3];
        public Texture2D StoneTile { get; set; }
        public Texture2D Dialogtex { get; set; }
        public Texture2D Achivement;
        public Texture2D Inv;
        public Texture2D Button;
        public Texture2D Black;
        public Texture2D Shootgun;
        public const string ImageFolder = @"Images/";
        public Texture2D Ramka;
        public Texture2D Galka;
        readonly ControlScene _controlScene;
        readonly Log _log = new Log("time");

        public Point Resolution
        {
            get => new Point(_graphics.PreferredBackBufferWidth,_graphics.PreferredBackBufferHeight);
            set {
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

        public bool InScreen(Vector2 coord) =>  new Rectangle(Point.Zero,Resolution).Contains(coord);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
                       {
                           PreferredBackBufferWidth = 1366 ,
                           PreferredBackBufferHeight = 768 ,
                           IsFullScreen = true
                       };
            _controlScene = new ControlScene();
            LoadSetting();
            Content.RootDirectory = "Content"; 
            _log.HasData = true;
            Window.AllowAltF4 = true;
        }

        /// <summary>
        /// Выводит на экран меню игрока в определенном маштабе
        /// </summary>
        /// <param name="scale">Маштаб</param>
        public void PlayerRender(float scale = 1.0f)
        {
            Player.RenderLeftPart(Hand, _spriteBatch, scale);
            Player.Render(_eye, _head, _body, _legs, _spriteBatch,scale);
        }

        /// <summary>
        /// Рисует на игроке, заданную текстуру с заданым маштабам
        /// </summary>
        /// <param name="texture">Текстура</param>
        /// <param name="scale">Маштаб</param>
        public void PlayerRenderTexture(Texture2D texture,float scale = 1.0f)
        {
            _spriteBatch.Draw(texture, new Rectangle((int)(
                Player.Position.X + (Player.Width - _head.Width) - 0), (int)(Player.Position.Y - 0),
                (int)(_body.Width*scale), (int)(_body.Height*scale)), new Rectangle(0, 0, _body.Width, _body.Height),
                Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
        //Базовые методы игрового цикла
        protected override void Initialize()
        {
            IsMouseVisible = false;
            base.Initialize();
        }

        public TileTwoSides Tiles;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Localisation.LoadLocalisation(new[]{ @"data/lang/en.lang", @"data/lang/ru.lang" });
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
            LoadRecipe();
            //music = Content.Load<Song>("music");
            _fog = Content.Load<Texture2D>(ImageFolder + "fog");
            //  postEffect = base.Content.Load<Effect>(EffectFolder + "NewSpriteEffect");
            Dimension[0] = new NormalWorld();
            Dimension[1] = new Hell();
            Dimension[2] = new SnowWorld();
            _controlScene.ChangeScene(new MainMenu());
        }
        public const string EffectFolder = @"Effects\";
        protected override void Update(GameTime gameTime)
        {
            if (Player.TypeKill >= 0) Player.KillNpc(Player.TypeKill==1);
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !KeyState.IsKeyDown(Keys.Escape))
                _controlScene.TryExit();
            MouseState = Mouse.GetState();
            KeyState = Keyboard.GetState();
            GameInput.UpdateMouse(MouseState);
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
        public void DrawText(string text, int x,int y,Color color)
        {
            _spriteBatch.DrawString(Font1,text,new Vector2(x,y),color);
        }
        public void AddFlash(int x,int y,string text, Color color)
        {
            Flash.Add(new FlashText(new Vector2(x, y), text, Font1, color));
        }
        protected override void Draw(GameTime gameTime)
        {
           // ModLoader.callFunction("PreDraw");
            GraphicsDevice.Clear(new Color(228,241,255));
            //SpriteEffects effect = SpriteEffects.None;
            foreach (FlashText fl in Flash)
                fl.Draw(_spriteBatch);
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
            _controlScene.Render(_spriteBatch);
            _spriteBatch.Begin();
           // ModLoader.callFunction("Draw");
            _spriteBatch.End();
            foreach (XnaLayout mygui in Guis)
                mygui.Draw(_spriteBatch);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_cursor, new Vector2(MouseState.X, MouseState.Y), Color.Blue);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        protected override void UnloadContent()
        {
            SaveSetting();
            Content.Unload();
        }
        //Методы связанные с Initalize
        public const string ConfigFolder = @"Data/Config";
        public const string ModsFolder = @"Data/Mods";
        void LoadSetting()
        {
            Program.StartSw();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(ConfigFolder+@"/Graphics.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null , nameof( xRoot ) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                switch ( xnode.Name ) {
                    case "Width":
                        _graphics.PreferredBackBufferWidth = int.Parse(xnode.InnerText,CultureInfo.InvariantCulture);
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
            xDoc.Load(ConfigFolder + @"/Sound.xml");
            xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null , nameof( xRoot ) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                // ReSharper disable once InvertIf
                if (xnode.Name == "Mutted")
                {
                    System.Console.WriteLine(xnode.InnerText);
                    if (int.Parse(xnode.InnerText, CultureInfo.InvariantCulture) != 0)MediaPlayer.IsMuted = true;
                }
            }
            Program.StopSw("Init Sound Setting");
            LoadInput();
        }
        void SaveSetting()
        {
            Program.StartSw();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(ConfigFolder + @"/Graphics.xml");
            // получим корневой элемент
          
            XmlElement xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null , nameof( xRoot ) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                switch ( xnode.Name ) {
                    case "Width":
                        xnode.InnerText = _graphics.PreferredBackBufferWidth.ToString( CultureInfo.InvariantCulture);
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

            File.Delete(ConfigFolder + @"/Graphics.xml");
            xDoc.Save(ConfigFolder + @"/Graphics.xml");
            Program.StopSw("Save Graphics Setting");
            Program.StartSw();
            xDoc = new XmlDocument();
            xDoc.Load(ConfigFolder + @"/Sound.xml");
            // получим корневой элемент

            xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null , nameof( xRoot ) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                // если узел - company
                // если узел - company
                if ( xnode.Name != "Mutted" ) continue;

                xnode.InnerText = MediaPlayer.IsMuted ? "1" : "0";
            }

            File.Delete(ConfigFolder + @"/Sound.xml");
            xDoc.Save(ConfigFolder + @"/Sound.xml");
            System.Console.WriteLine("TEST");
            Program.StopSw("Save Graphics Setting");
        }

        static void LoadInput()
        {
            Program.StartSw();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(ConfigFolder + @"/Input.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null , nameof( xRoot ) + " != null");
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine(xnode.Name);
                // если узел - company
                switch ( xnode.Name ) {
                    case "Jump":
                        GameInput.Jump = int.Parse(xnode.InnerText,CultureInfo.InvariantCulture);
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
                        GameInput.Drop = int.Parse(xnode.InnerText,CultureInfo.InvariantCulture);
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
            Console.AddLog("Maximum:" + (Item.ItemMax + TileTwoSides.TileMax));
            Program.StopSw("Creating Console");
        }

        void LoadBackImage()
        {
            Black = Content.Load<Texture2D>(ImageFolder + "black");
            Program.StartSw();
            for (int i = 0; i < 3; i++)
                _background[i] = Content.Load<Texture2D>(ImageFolder + "background_" + i);

            StoneTile = Content.Load<Texture2D>(ImageFolder + "stonetiles");
            Program.StopSw("Loaded Background");
        }
        void LoadedHuman()
        {
            Program.StartSw();
            _blood = Content.Load<Texture2D>(ImageFolder + "blood");
            _head = Content.Load<Texture2D>(ImageFolder + "head");
            Hand = Content.Load<Texture2D>(ImageFolder + "hand");
            _eye = Content.Load<Texture2D>(ImageFolder + "eyes");
            _head2 = Content.Load<Texture2D>(ImageFolder + "head2");
            _body = Content.Load<Texture2D>(ImageFolder + "body");
            _legs = Content.Load<Texture2D>(ImageFolder + "legs");
            _shadow = Content.Load<Texture2D>(ImageFolder + "shadow");
            Program.StopSw("Loaded Human");
        }
        public  const string FontFolder = @"Fonts\";
        void LoadedGui()
        {
            Program.StartSw();
            Button = Content.Load<Texture2D>(ImageFolder + "button");
            Carma = Content.Load<Texture2D>(ImageFolder + "carma");
            Achivement = Content.Load<Texture2D>(ImageFolder + "achivement");
            _cursor = Content.Load<Texture2D>(ImageFolder + "cursor");
            Inv = Content.Load<Texture2D>(ImageFolder + "invsphere");
            Galka = Content.Load<Texture2D>(ImageFolder + "galka");
            Ramka = Content.Load<Texture2D>(ImageFolder + "ramka");
            EmptyDrougt = Content.Load<Texture2D>(ImageFolder + @"hud\emptyDrougt");
            FullDrougt = Content.Load<Texture2D>(ImageFolder + @"hud/fullDrougt");
            WolfSkin = Content.Load<Texture2D>(ImageFolder + @"NPC/wolf");
            _pigSkin = Content.Load<Texture2D>(ImageFolder + @"NPC/pig");
            EatHunger = Content.Load<Texture2D>(ImageFolder + @"hud/hunger");
            _explosion = Content.Load<Texture2D>(ImageFolder + @"explosion");
            _music = Content.Load<Song>("DestinationUnknown");
            _snowTexture = Content.Load<Texture2D>(ImageFolder + "snow");
            BloodBody = Content.Load<Texture2D>(ImageFolder + @"skelet/body");
            for(int i = 0;i<4;i++)
            BloodTexture[i] = Content.Load<Texture2D>(ImageFolder + @"skelet/"+i);
            Dialogtex = Carma;
            Program.StopSw("Loaded Image Gui (50 milisecond First load Content)");

            Program.StartSw();
            Font1 = Content.Load<SpriteFont>(FontFolder+ "myfont");
            Font2 = Content.Load<SpriteFont>(FontFolder + "myfont2");
            Font3 = Content.Load<SpriteFont>(FontFolder + "myfont3");
            Program.StopSw("Loaded Font Gui");
        }

        static void LoadRecipe()
        {
            Program.StartSw();
            Recipe test = new Recipe(new Item(1, 6), 100.0f);

            Recipe.AddRecipe(test, new[] { new Item(2, 7),new Item(1,5) });
            
            Recipe.AddRecipe(new Recipe(new Item( 5,    19),    20.0f),new[] {new Item(1,5)});
            Recipe.AddRecipe(new Recipe(new Item(1,    8),     20.0f), new[] { new Item(2, 19), new Item(3, 1) });
            Recipe.AddRecipe(new Recipe(new Item(1,22),   20.0f), new[] { new Item(3,1) });
            Recipe.AddRecipe(new Recipe(new Item(1,23),    20.0f), new[] { new Item(2, 1) });
            Recipe.AddRecipe(new Recipe(new Item(1, 24), 20.0f), new[] { new Item(3, 1) });
            //NEW UPDATE
            Recipe.AddRecipe(new Recipe(new Item(1, 26), 20.0f),new[] { new Item(6, 19), new Item(6, 5) });
            Recipe.AddRecipe(new Recipe(new Item(1, 27), 20.0f),new[] { new Item(4, 19), new Item(3, 5) });
            Recipe.AddRecipe(new Recipe(new Item(1, 28), 20.0f), new[] { new Item(1,10) });
            Recipe.AddRecipe(new Recipe(new Item(1, 29),22, 20.0f),new[] { new Item(4,3)});
            Recipe.AddRecipe(new Recipe(new Item(1, 30),22, 20.0f),new[] {new Item(4,2) });
            Recipe.AddRecipe(new Recipe(new Item(1, 31), 20.0f), new[] { new Item(5, 29) });
            Recipe.AddRecipe(new Recipe(new Item(1, 32), 20.0f), new[] { new Item(4, 29) });
            Recipe.AddRecipe(new Recipe(new Item(1, 33), 20.0f), new[] { new Item(5, 29) });
            Recipe.AddRecipe(new Recipe(new Item(1, 34), 20.0f), new[] { new Item(1, 29), new Item(2, 19) }); // sword
            Recipe.AddRecipe(new Recipe(new Item(1, 38),22, 20.0f), new[] { new Item(4, 36) });
            Recipe.AddRecipe(new Recipe(new Item(1, 43), 20.0f), new[] { new Item(6, 1), new Item(1, 19) }); // hammer
            Recipe.AddRecipe(new Recipe(new Item(1, 41), 22, 20.0f), new[] { new Item(3, 29), new Item(1, 10) }); // pickaxe
            Recipe.AddRecipe(new Recipe(new Item(1, 44), 22, 20.0f), new[] { new Item(1, 2), new Item(2, 19) });
            Recipe.AddRecipe(new Recipe(new Item(1, 39), 20.0f), new[] { new Item(4, 38) });
            Recipe.AddRecipe(new Recipe(new Item(1, 45), 22, 20.0f), new[] { new Item(1, 1), new Item(2, 19) });
            Recipe.AddRecipe(new Recipe(new Item(4, 47), 20.0f), new[] { new Item(1, 46) });
            Recipe.AddRecipe(new Recipe(new Item(1, 48), 20.0f), new[] { new Item(1, 46), new Item(1, 49) });

            Recipe.AddRecipe(new Recipe(new Item(1, 49), 20.0f), new[] { new Item(1, 19), new Item(1, 47) });
            //EAT
            Recipe.AddRecipe(new Recipe(new Item(1, 52), 19, 20.0f), new[] { new Item(4, 51) });
            //EAT
            Recipe.AddRecipe(new Recipe(new Item(1, 53), 22, 20.0f), new[] { new Item(1, 52), new Item(1, 10) });

            Program.StopSw("Created Recipe");
        }
        const string NAME_SUB_VERSION = "Alpha";
        const string NAME_VERSION = "Hammer & Anvil";
        const byte ID_SUB_VERSION = 0;
        const byte ID_VERSION = 0;
        const byte MAJOR = 0;
        const byte MINOR = 0;
        const byte BUILD = 3;
        const byte REVESION = 0;
        readonly NamingVersion _versionThis = new NamingVersion(ID_SUB_VERSION,ID_VERSION,MAJOR,MINOR,BUILD,REVESION);
        Song _music;
        //Методы меню
        public void LoadSlots()
        {
            for (int i = 0; i < 6; i++)
                Slots[i] = Content.Load<Texture2D>(ImageFolder + "slot\\slot_" + i);
        }
        
        public void NewGeneration()
        {
            _controlScene.ChangeScene(Progress.Instance);
            for (int i = 0; i < MaxDimension; i++)
            {
                CurrentDimension = i;
                Dimension[i].Clear();
                Dimension[i].Start(Progress.Instance.Bar);
            }
            StartGame();
        }

        void StartGame() {
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
            Race.LoadRace(Font1,HeightMenu,Button,ConfigFolder+@"/racelist.xml", -1);
            Program.StopSw("Loaded default Race's");
        }
        bool _activeConsole;
        bool _day = true;
        public Camera Camera = new Camera();
        //Методы игры
        public void GameUpdate(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalMinutes;
            Player.Update(delta, Seconds);
            Dimension[CurrentDimension].Update(gameTime,Camera);
            Camera.Pos.X = (int)Player.Position.X;
            Camera.Pos.Y = (int)Player.Position.Y;
            if (Camera.Pos.Y> (SizeGeneratior.WorldHeight - 1) * Tile.TileMaxSize) Camera.Pos.Y = (SizeGeneratior.WorldHeight - 1) * Tile.TileMaxSize;
            if (Camera.Pos.Y < _graphics.PreferredBackBufferHeight/2.0f) Camera.Pos.Y = _graphics.PreferredBackBufferHeight/2.0f;
            if (Camera.Pos.X > (SizeGeneratior.WorldWidth - 1) * Tile.TileMaxSize) Camera.Pos.X = (SizeGeneratior.WorldWidth - 1) * Tile.TileMaxSize;
            if (Camera.GetLeftUpper.X < 0) Camera.GetLeftUpper = new Vector2(0,Camera.GetLeftUpper.Y);
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
                int spawnX = (int)(Player.Position.X + _graphics.PreferredBackBufferWidth) / Tile.TileMaxSize;
                SpawnTime = 1000;
                if (Rand.Next(0, 100) == 0) ((NormalWorld)Dimension[0]).Npcs.Add(new Wolf(spawnX, WolfSkin));
                else ((NormalWorld)Dimension[0]).Npcs.Add(new Pig(spawnX, _pigSkin));
            }
            for (int i = 0; i < Bullets.Count; i++)
            {
                if ( Bullets[i].Move() ) continue;

                Bullets[i].Destory(Dimension[CurrentDimension]);
                Bullets.RemoveAt(i);
                break;
            }
            for (int i = 0; i < Drops.Count; i++)
            {
                Drops[i].Update();
                if ( !Player.Rect.Intersects(new Rectangle((int) Drops[i].Position.X , (int) Drops[i].Position.Y , 16 ,
                                                           16)) ) continue;

                Drops[i].GetSlot().IsEmpty = false;
                Player.SetSlot(Drops[i].GetSlot());
                Drops.RemoveAt(i);
                break;
            }
        }
        public int SpawnTime;
        public void AddBulletToMouse(int x,int y)
        {
            Bullets.Add(new Bullet(new Vector2(x, y), Tools.Distance(new Vector2(x, y))));
        }
        //RenderTarget2D shadowSprite;
        public void GameDraw()
        {
            DrawBackgrounds();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetViewTran(_graphics));
            int i0 = (int)(Camera.GetLeftUpper.X / Tile.TileMaxSize - 1f);
            int i1 = (int)(Camera.GetRightDown.X / Tile.TileMaxSize) + 2;
            int j0 = (int)(Camera.GetLeftUpper.Y / Tile.TileMaxSize - 1f);
            int j1 = (int)(Camera.GetRightDown.Y / Tile.TileMaxSize) + 2;
            Dimension[CurrentDimension].Draw(_spriteBatch, Tiles, new Rectangle(i0, j0, i1 - i0, j1 - j0));
            // spriteBatch.DrawString(Font1, onetwo.ToString(), new Vector2(120, 130), Color.White)
            Player.Render(Hand, _eye, _head, _body, _legs, _spriteBatch, _shadow);

            //  spriteBatch.Draw(legs2, new Rectangle((int)(player.position.X + (player.width - head.Width) - ScreenPosX), (int)(player.position.Y - ScreenPosY), legs.Width, legs.Height), new Rectangle(0, 0, legs.Width, legs.Height), Color.Red, 0, Vector2.Zero, effect, 0);

            foreach (Zombie npc in Program.Game.Dimension[Program.Game.CurrentDimension].Zombies)
                npc.RenderNpc(_spriteBatch, Font1, _head, _head2, _body, _legs, _blood, _eye, Hand, _shadow);
            foreach (Civilian npc in Program.Game.Dimension[Program.Game.CurrentDimension].Civil)
                npc.RenderNpc(_spriteBatch, Font1, _head, _body, _legs, Hand, _shadow);
            foreach (Drop drops in Drops)
                drops.Render(_spriteBatch, (int)drops.Position.X, (int)drops.Position.Y);

            _isSnowing = ReferenceEquals(Dimension[CurrentDimension].MapBiomes[(int)Player.Position.X / Tile.TileMaxSize] , ArrayResource.Snow);
            DrawSnow();
            RenderLight(i0, i1, j0, j1);
            Player.RenderFog(_fog, _spriteBatch);
            _spriteBatch.Begin();
            DrawHud();
            _spriteBatch.End();
        }

        void DrawBackgrounds()
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap);
            Rectangle dest2 = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Rectangle src2 = new Rectangle((int)Camera.GetLeftUpper.X, (int)Camera.GetLeftUpper.Y, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _spriteBatch.Draw(StoneTile, dest2, src2, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            //  spriteBatch.Draw(background[1], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.5f),Genworld.rocklayer-background[1].Height, background[1].Width, graphics.PreferredBackBufferHeight), Color.White);
            //   spriteBatch.Draw(background[2], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.8f), Genworld.rocklayer, background[2].Width, graphics.PreferredBackBufferHeight), Color.White);
            //spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            _spriteBatch.End();
            // else if (CurrentDimension >= 1) GraphicsDevice.Clear(Color.Red);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp);
            //spriteBatch.Draw(background[CurrentDimension], new Vector2(-ScreenPosX,-ScreenPosY), new Rectangle(0, 0, (CWorld.TileMaxX * Tile.TileMaxSize), (CWorld.TileMaxY * Tile.TileMaxSize)), Color.White);
            //  Rectangle dest = new Rectangle(-ScreenPosX, -ScreenPosY + CWorld.TileMaxY / 3 * Tile.TileMaxSize, CWorld.TileMaxX * Tile.TileMaxSize, (CWorld.TileMaxY - CWorld.TileMaxY / 3 - CWorld.TileMaxY / 3) * Tile.TileMaxSize);
            Rectangle dest = new Rectangle(0, (int)-Camera.Pos.Y + SizeGeneratior.WorldHeight / 3 * Tile.TileMaxSize + 600 * Tile.TileMaxSize,
                _graphics.PreferredBackBufferWidth, (SizeGeneratior.WorldHeight - SizeGeneratior.WorldHeight / 3 - SizeGeneratior.WorldHeight / 3 - 600) * Tile.TileMaxSize);
            Rectangle src = new Rectangle(0, 0, 48, 1300);
            _spriteBatch.Draw(_background[CurrentDimension], dest, src, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            //  spriteBatch.Draw(background[1], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.5f),Genworld.rocklayer-background[1].Height, background[1].Width, graphics.PreferredBackBufferHeight), Color.White);
            //   spriteBatch.Draw(background[2], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.8f), Genworld.rocklayer, background[2].Width, graphics.PreferredBackBufferHeight), Color.White);
            //spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            _spriteBatch.End();
            DrawBackground(0.9f);
        }

        bool _isSnowing;
        Texture2D _snowTexture;
        Random _random;
        readonly Point[] _snow = new Point[50];
        public void DrawSnow()
        {
            Point quitPoint = new Point(0,
                                        Dimension[CurrentDimension].MapHeight[0]*Tile.TileMaxSize+ _snowTexture.Height);

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
                                         _random.Next((int) Camera.GetLeftUpper.X ,
                                                        (int) (Camera.GetLeftUpper.X +
                                                            _graphics.PreferredBackBufferWidth
                                                                * Camera.Zoom -_snowTexture.Width)) ,
                                         _random.Next((int) Camera.GetLeftUpper.Y ,
                                                        (int) (Camera.GetLeftUpper.Y +
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

                _spriteBatch.Draw(_snowTexture, snowParticle, Color.White);

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
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicWrap, null, null, null, Camera.GetViewTran(_graphics, zoom));
            Rectangle src = new Rectangle(0, 0, (int)(SizeGeneratior.WorldWidth * Tile.TileMaxSize / zoom), _background[2].Height);
            Rectangle dest = new Rectangle(-src.Width, Dimension[CurrentDimension].MapBiomes[(int)Player.Position.X/Tile.TileMaxSize].MaxHeight * Tile.TileMaxSize - (int)(src.Height * zoom), src.Width, src.Height);
            src.Width *= 2;
            dest.Width = src.Width;
            _spriteBatch.Draw(_background[2], dest, src, Color.White);
            _spriteBatch.End();
        }
        public const bool IsDebug = false;
     
        //Сообственные методы

        readonly NamingVersion _versionMap = new NamingVersion(0, 0, 0, 0, 0, 0);
        void Load(object thread)
        {
            FileStream stream = File.OpenRead("save.sav");
            BinaryReader reader = new BinaryReader(stream);
            _versionMap.Load(reader);
            for (int d = 0; d < MaxDimension; d++)
            {
                CurrentDimension = d;
                Dimension[d].Clear();
                Dimension[d].Load(reader,Progress.Instance.Bar,_versionMap.GetCode());
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
            FileStream stream =  File.Create("save.sav");
            BinaryWriter writer = new BinaryWriter(stream);
            _versionThis.Save(writer);
            for (int d = 0; d < MaxDimension; d++)
                Dimension[d].Save(writer, Progress.Instance.Bar);
            Player.Save(writer);
            writer.Close();
            _controlScene.ReturnScene();
        }
        public string GetVersion() => NAME_SUB_VERSION + " " + _versionThis.GetVersion();

        public string GetFullVersion() => "Two sides " + NAME_VERSION+" "+NAME_SUB_VERSION + " " + _versionThis.GetVersion();

        public void SaveMap()
        {
            ThreadPool.QueueUserWorkItem(Save);
        }
        
        void RenderLight(int i0, int i1, int j0, int j1)
        {
            for (int i = i0; i < i1; i++)
            {
                if (i < SizeGeneratior.WorldWidth && i >= 0) Dimension[Program.Game.CurrentDimension].UpdateMaxY(i);
                for (int j = j0; j < j1; j++)
                {
                    if (i > SizeGeneratior.WorldWidth - 1 || i < 0) continue;
                    if (j > SizeGeneratior.WorldHeight - 1 || j < 0) continue;
                    //dimension[CurrentDimension].globallighting = 1;
                    Dimension[CurrentDimension].MapTile[i, j].SetLight(i, j, (short)Dimension[CurrentDimension].Globallighting,SizeGeneratior.RockLayer);
                    Dimension[CurrentDimension].MapTile[i, j].UpdateLight(i, j);
                    int lighting = 255 - Dimension[CurrentDimension].MapTile[i, j].Light * 5;
                     _spriteBatch.Draw(Black, new Vector2(Tile.TileMaxSize * i, Tile.TileMaxSize * j), new Color(0, 0, 0, lighting));
                }
            }
        }
        
        void DrawHud()
        {
            if (Player.ControlJ)
            {
                DrawSlotItem(Player.CurrentMaxSlot);
                for (int i = Player.Slotmax; i < Player.Slotmax + 3; i++)
                {
                    _spriteBatch.Draw(Inv, new Rectangle((i - Player.Slotmax) * 32, _graphics.PreferredBackBufferHeight - 32, 32, 32), Color.White);
                    if (!Player.Slot[i].IsEmpty)
                        Player.Slot[i].Render(_spriteBatch, (i - Player.Slotmax) * 32 + 8, _graphics.PreferredBackBufferHeight - 32 + 8);
                }

                int a = 0;
                for (int i = 0; i < Recipe.Recipes.Count; i++)
                {
                    if ( !Player.GetValidRecipes(i) ) continue;

                    _spriteBatch.Draw(Inv, new Rectangle(a * 32, 32 + -32, 32, 32), Color.Black);
                    Recipe.Recipes[i].Slot.Render(_spriteBatch, a * 32 + 16 - 8, 35 + (16 - 8) + -32);
                    for (int j = 0; j < Recipe.Recipes[i].GetSize(); j++)
                    {
                        _spriteBatch.Draw(Inv, new Rectangle(a * 32, (j + 2) * 35 + -32, 32, 32), Color.Green);
                        Item.Render(_spriteBatch, Recipe.Recipes[i].GetIngrident(j), a * 32 + 16 - 8, (j + 2) * 35 + (16 - 8) + -32);
                        _spriteBatch.DrawString(Font1, Recipe.Recipes[i].GetSize(j).ToString(CultureInfo.CurrentCulture), new Vector2((a + 1) * 32 - 10, (j + 2) * 35 + (16 - 8) + -32), Color.White);
                    }
                    a++;
                }
                DrawBloodBody(a*32, 0);
            }
            else
            {
                DrawSlotItem(9);
                DrawBloodBody(0, 0);
            } int wcarma = (int)(10 + SizeCarmaWidth * (0.01 * Player.Carma));
            int hptext = (int)(Player.Width * (Player.Blood/5000));
            int color = (int)(200 * (0.01 * Player.Carma));
            if (!Player.MouseItem.IsEmpty) Player.MouseItem.Render(_spriteBatch, MouseState.X + 16, MouseState.Y + 5);
            _spriteBatch.Draw(Carma, new Rectangle(_graphics.PreferredBackBufferWidth - wcarma, _graphics.PreferredBackBufferHeight - SizeCarmaHeight, wcarma, SizeCarmaHeight), new Color(color * (color / 255), color * (color / 255), color, color));
            _spriteBatch.DrawString(Font3, "Karma", new Vector2(_graphics.PreferredBackBufferWidth - wcarma, _graphics.PreferredBackBufferHeight - Font3.MeasureString("Karma").Y ), Color.White);

            _spriteBatch.Draw(Carma, new Rectangle((int)Player.Position.X+Player.Width/2-hptext/2, (int)Player.Position.Y-SizeCarmaHeight/2, hptext, SizeCarmaHeight/2), Color.Red);
           // spriteBatch.DrawString(Font1, "Blood", new Vector2(graphics.PreferredBackBufferWidth / 2 - hptext / 2, Font1.MeasureString("Blood").Y/2), Color.White);

            //spriteBatch.Draw(carma, new Rectangle(graphics.PreferredBackBufferWidth - 16, 16, 16, 16 + (int)player.drought), Color.Blue);
            _spriteBatch.Draw(FullDrougt, new Rectangle(_graphics.PreferredBackBufferWidth - 16, _graphics.PreferredBackBufferHeight-SizeCarmaHeight-16, 16, 16),new Rectangle(0,0,FullDrougt.Width,(int)(Player.Drought/100*FullDrougt.Height)), Color.White);
            _spriteBatch.Draw(EmptyDrougt, new Rectangle(_graphics.PreferredBackBufferWidth - 16, _graphics.PreferredBackBufferHeight - SizeCarmaHeight - 16, 16, 16), Color.White);
            Vector2 sizeHunger = new Vector2(EatHunger.Width * (Player.Hunger / 100.0f), EatHunger.Height);
            _spriteBatch.Draw(EatHunger, new Vector2(_graphics.PreferredBackBufferWidth - wcarma-sizeHunger.X, _graphics.PreferredBackBufferHeight - SizeCarmaHeight),new Rectangle(EatHunger.Width-(int)sizeHunger.X,0,(int)sizeHunger.X,(int)sizeHunger.Y), Color.White);

            foreach (Civilian npc in Program.Game.Dimension[Program.Game.CurrentDimension].Civil)
                npc.RenderDialog(_spriteBatch);

            // string temperature = "Player Temperature:" + player.Temperature + "*c";
             // spriteBatch.DrawString(Font1, temperature, new Vector2(graphics.PreferredBackBufferWidth - (Font1.MeasureString(temperature).X) - 5, SizeCarmaHeight + 120), Color.White);
            _spriteBatch.End();
            int y = 0;
            foreach ( Quest quest in Player.Quests ) {
                quest.Render(_spriteBatch, new Vector2(_graphics.PreferredBackBufferWidth - 10, 150 + y));
                y += quest.GetQuestHeight();
            } _spriteBatch.Begin();
            if ( !Console.Isactive ) return;

            _spriteBatch.End();
            Console.Draw(_spriteBatch);
            _spriteBatch.Begin();
        }

        void DrawBloodBody(int xBody, int yBody)
        {
            _spriteBatch.Draw(BloodBody, new Vector2(xBody, yBody), Color.White);
            for (int i = 0; i < 4; i++)
                if (Player.Bloods[i]) _spriteBatch.Draw(BloodTexture[i], new Vector2(xBody, yBody), Color.Red);
        }

        void DrawSlotItem(int maxSlot)
        {
            int r = -1;
            int slotHover = -1;
            int centerSlot = _graphics.PreferredBackBufferWidth - 9*32;
            for (int i = 0; i < maxSlot; i++)
            {
                if (i % 9 == 0) r++;
                int xslot = i % 9 * 32;
                int yslot = 32 * r;
                Color cl = Color.White;
                if (i == Player.SelectedItem) cl = Color.Black;
                _spriteBatch.Draw(Inv, new Rectangle(centerSlot+xslot,yslot , 32, 32), cl);
                if ( Player.Slot[i].IsEmpty ) continue;

                Player.Slot[i].Render(_spriteBatch,centerSlot+xslot + 16/2, 16/2 + yslot);
                _spriteBatch.DrawString(Font3, Player.Slot[i].Ammount.ToString(CultureInfo.CurrentCulture), new Vector2(centerSlot+(32 * (i % 9 + 1) - 5 - Font3.MeasureString(Player.Slot[i].Ammount.ToString(CultureInfo.CurrentCulture)).X), 32 - Font3.MeasureString(Player.Slot[i].Ammount.ToString(CultureInfo.CurrentCulture)).Y + yslot), Color.Black);

                if (new Rectangle(centerSlot+xslot, yslot, 32, 32).Contains(new Point(MouseState.X, MouseState.Y)))
                    slotHover = i;
            }//одежда
            if (slotHover >= 1)
                DrawItemHelp(slotHover);
        }

        void DrawItemHelp(int i)
        {
            _spriteBatch.DrawString(Font2, Player.Slot[i].GetName(), new Vector2(MouseState.X + 16,
                MouseState.Y + 5),
                Player.Slot[i].GetColor());
            _spriteBatch.DrawString(Font2, "DPS:" + Player.Slot[i].GetDamage() +
                "(" + Player.Slot[i].GetDamage() *
                (Player.Slot[i].Hp / 100) + ")",
                new Vector2(MouseState.X + 16, MouseState.Y + 15),
                Player.Slot[i].GetColor());
            _spriteBatch.DrawString(Font2, "Power:" + Player.Slot[i].GetPower() +
                "(" + Player.Slot[i].GetPower() *
               (Player.Slot[i].Hp / 100) + ")",
                new Vector2(MouseState.X + 16, MouseState.Y + 25),
                Player.Slot[i].GetColor());
        }

        // ReSharper disable once UnusedMember.Local
        void Drawdebug()
        {
            int glint = (int)Dimension[CurrentDimension].Globallighting;
            string text = "brightness world:" + glint.ToString(CultureInfo.CurrentCulture);

            string height = "height:" + (SizeGeneratior.WorldHeight - (int)(Player.Position.Y / Tile.TileMaxSize)).ToString(CultureInfo.CurrentCulture);
            _spriteBatch.DrawString(Font1, text, new Vector2(_graphics.PreferredBackBufferWidth - text.Length * 7 - 5, SizeCarmaHeight + 20), Color.White);
            //_spriteBatch.DrawString(Font1, "FPS:" + GLOBAL_FPS.ToString(CultureInfo.CurrentCulture), new Vector2(_graphics.PreferredBackBufferWidth - 50, SizeCarmaHeight + 40), Color.White);
            _spriteBatch.DrawString(Font1, height, new Vector2(_graphics.PreferredBackBufferWidth - height.Length * 7 - 5, SizeCarmaHeight + 60), Color.White);
            string dworld;
            switch ( CurrentDimension ) {
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
            _spriteBatch.DrawString(Font1, dworld, new Vector2(_graphics.PreferredBackBufferWidth - dworld.Length * 7 - 5, SizeCarmaHeight + 80), Color.White);
            string biotemp = "World Temperature:" + Dimension[CurrentDimension].GetTemperature((int)Player.Position.X / Tile.TileMaxSize,
                (int)Player.Position.Y / Tile.TileMaxSize) + "*c";
            _spriteBatch.DrawString(Font1, biotemp, new Vector2(_graphics.PreferredBackBufferWidth - biotemp.Length * 7 - 5, SizeCarmaHeight + 100), Color.White);
            Vector2 vec = Tools.GetTile((int)Player.Position.X, (int)Player.Position.Y);
            glint = Dimension[CurrentDimension].MapTile[(int)vec.X,(int)vec.Y].Light;
            text = "brightness Block:" + glint.ToString(CultureInfo.CurrentCulture);
            _spriteBatch.DrawString(Font1, text, new Vector2(_graphics.PreferredBackBufferWidth - text.Length * 7 - 5, SizeCarmaHeight + 140), Color.White);

            string position = "Player X:" + Player.Position.X + " position.y: " + Player.Position.Y;
            _spriteBatch.DrawString(Font1, position, new Vector2(_graphics.PreferredBackBufferWidth - position.Length * 7 - 5, SizeCarmaHeight + 160), Color.White);
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

        public Texture2D FullDrougt { get; set; }

        public Texture2D EmptyDrougt { get; set; }

        public Texture2D EatHunger { get; set; }
        Texture2D _explosion;
        public void AddExplosion(Vector2 pos) {
            Dimension[CurrentDimension].AddExplosion(pos, _explosion);
        }

        public SpriteFont Font3 { get; set; }

        public Texture2D BloodBody { get; set; }

        public Texture2D[] BloodTexture = new Texture2D[4];
        Texture2D _pigSkin;

        public Texture2D Hand { get; set; }

        public Texture2D WolfSkin { get; set; }

        public Effect PostEffect { get; set; }
    }
}
