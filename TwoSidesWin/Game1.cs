using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//Именно это пространство имен поддерживает многопоточность
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TwoSides.GUI;
using TwoSides.Physics.Entity;
using TwoSides.Physics.Entity.NPC;

using TwoSides.World;
using System.Threading;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.Dimensions;
using TwoSides.GameContent.Entity.NPC;
using TwoSides.GameContent.Entity;
using TwoSides.GameContent.Tiles;
using TwoSides.World.Tile;
using TwoSides.World.Generation;
using TwoSides.Utils;
using TwoSides.GUI.Scene;
using TwoSides.GameContent.GUI.Scene;
using TwoSides.ModLoader;
using TwoSides.Physics;
using TwoSides.Network;
namespace TwoSides
{
    /// <summary>
    /// Дальше идёт код без комментарий, поэтому читайте код, только про качки навыка догадливасти до максиума
    /// А лучше вообще не читайте, а то будете меня оскорблять 
    /// </summary>
    sealed public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Константы
        const int brightness = 51;
        public const int maxframe = 100;
        public const int addmaxt = 13;
        public const int maxPosters = 1;
        public const int MaxD = 3;
        public const int RecipeTyp = 8;
        public const int SizeCarmaHeight = 16;
        public const int SizeCarmaWidth = 80;
      
        //Переменные
        int globalfps = 0;
        public int heightmenu;
        public int type = 0;
        public int currentD = 0;
        public float seconds;
        public TwoSides.GUI.Console console;


        //Листы
        static List<FlashText> flash = new List<FlashText>();
        static List<Drop> drop = new List<Drop>();
        public List<Bullet> bullets = new List<Bullet>();
        public List<GUI.GUI> guis = new List<GUI.GUI>();
        //Массивы
        Race[] racestd = new Race[2];
        public BaseDimension[] dimension = new BaseDimension[MaxD];
        public List<Recipe> recipes = new List<Recipe>();

        //Объекты
        private SpriteBatch spriteBatch;
        //Song music;
        public SpriteFont font2;

        static GUI.GUI guiremove;
        public Effect effects;
        public KeyboardState keyState = Keyboard.GetState();
        public GraphicsDeviceManager graphics;
        public SpriteFont Font1;
        public Random rand = new Random();
        public Player player = new Player();
        public MouseState mouseState = Mouse.GetState();

        //Текстуры
        Texture2D fog;
        Texture2D cursor;
        Texture2D shadow;
        static Texture2D head;
        static Texture2D eye;
        static Texture2D body;
        static Texture2D legs;
        public Texture2D carma;
        Texture2D head2;
        Texture2D blood;
        public Texture2D[] slots = new Texture2D[6];
        Texture2D[] background = new Texture2D[3];
        public Texture2D texturestonetile { get; set; }
        public Texture2D dialogtex { get; set; }
        public Texture2D achivement;
        public Texture2D inv;
        public Texture2D button;
        public Texture2D black;
        public Texture2D shootgun;
        public const string ImageFolder = @"Images/";
        public Texture2D ramka;
        public Texture2D galka;
        ControlScene controlScene = new ControlScene();
        Log log = new Log("time");
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = true; 
            LoadSetting();
            Content.RootDirectory = "Content"; 
            log.hasData = true;
            this.Window.AllowAltF4 = true;
            
        }
        public void PlayerRender(float scale = 1.0f)
        {
            player.render(eye, head, body, legs, spriteBatch,scale);
            player.renderleft(hand, spriteBatch, scale);
        }
        public void PlayerRenderTexture(Texture2D texture,float scale = 1.0f)
        {
            spriteBatch.Draw(texture, new Rectangle((int)(
                player.position.X + (player.width - head.Width) - 0), (int)(player.position.Y - 0),
                (int)(body.Width*scale), (int)(body.Height*scale)), new Rectangle(0, 0, body.Width, body.Height),
                Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
        //Базовые методы игрового цикла
        protected override void Initialize()
        {
            IsMouseVisible = false;
            base.Initialize();
        }
        public TileTwoSides tiles;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadedGui();
            LoadedHuman();
            Clothes.Loadclothes(base.Content);
            LoadBackImage();
            tiles = new TileTwoSides();
            tiles.Loadtiles(base.Content);
            Item.LoadedItems(base.Content);
            Bullet.LoadBullet(base.Content);
            heightmenu = (int)(graphics.PreferredBackBufferHeight / 2 - 3 * 27);
            LoadStdRaces();
            CreateConsole();
            LoadRecipe();
            LoadMods();
            LoadModeClothes();
            LoadModeBlocks();
            //music = Content.Load<Song>("music");
            fog = base.Content.Load<Texture2D>(ImageFolder + "fog");
            postEffect = base.Content.Load<Effect>(EffectFolder + "NewSpriteEffect");
            dimension[0] = new NormalWorld();
            dimension[1] = new Hell();
            dimension[2] = new SnowWorld();
            controlScene.changeScene(new MainMenu());
            //blocksis[1].itemnead = (int)Block.tool.pickaxe;
            // recipes[8] = new Recipe(new Slot(5000, 1, 10, 25), 20.0f);
            //   recipes[8].addigr(1, 3);
            //recipes[1].addigr(5, 1);
            //recipes[2].addigr(5, 1);
            //recipes[3].addigr(5, 1);
            // TODO: use this.Content to load your game content here
        }
        public const string EffectFolder = @"Effects\";
        protected override void Update(GameTime gameTime)
        {
            if (player.typeKill == 0) player.killnpc(false);
            else if (player.typeKill == 1) player.killnpc(true);
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !keyState.IsKeyDown(Keys.Escape))
            {
                controlScene.tryExit();
            }
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();
            GameInput.updateMouse(mouseState);
            // TODO: Add your update logic here
            controlScene.Update(gameTime);
         /*   switch (state)
            {
                case (int)State.MENU: MenuUpdate(gameTime); break;
                case (int)State.Game: GameUpdate(gameTime); break;
                case (int)State.SelectRace: SRUpdate(gameTime); break;
                case (int)State.CreateGame: CreatePersonUpdate(gameTime); break;
                case (int)State.Rpgsystem: RpgSystemUpdate(gameTime); break;
                case (int)State.Pause: PauseUpdate(gameTime); break;
                default: break;
                //case (int)State.Progress: GenerationsUpdate(gameTime); break;
            }*/
            foreach (GUI.GUI mygui in guis)
            {
                mygui.Update();
            }
            foreach (FlashText fl in flash)
            {
                if (fl.IsInvisible())
                {
                    flash.Remove(fl);
                    break;
                }
                fl.Update();
            }
            guis.Remove(guiremove);
            base.Update(gameTime);
        }
        public void DrawText(string text, int x, int y)
        {
            DrawText(text, x, y, Color.White);
        }
        public void DrawText(string text, int x,int y,Color color)
        {
            spriteBatch.DrawString(Font1,text,new Vector2(x,y),color);
        }
        public void AddFlash(int x,int y,string text, Color color)
        {
            flash.Add(new FlashText(new Vector2(x, y), text, Font1, color));
        }
        protected override void Draw(GameTime gameTime)
        {
           // ModLoader.callFunction("PreDraw");
            GraphicsDevice.Clear(new Color(228,241,255));
            //SpriteEffects effect = SpriteEffects.None;
            foreach (FlashText fl in flash)
            {
                fl.Draw(spriteBatch);
            }
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
            controlScene.Render(spriteBatch);
            spriteBatch.Begin();
           // ModLoader.callFunction("Draw");
            spriteBatch.End();
            foreach (GUI.GUI mygui in guis)
            {
                mygui.Draw(spriteBatch);
            }
            spriteBatch.Begin();
            spriteBatch.Draw(cursor, new Vector2(mouseState.X, mouseState.Y), Color.Blue);
            spriteBatch.End();
            ModLoader.ModLoader.callFunction("Draw");
            base.Draw(gameTime);
        }
        protected override bool BeginDraw()
        {
            ModLoader.ModLoader.callFunction("preDraw");
            return base.BeginDraw();
        }
        protected override void EndDraw()
        {
            ModLoader.ModLoader.callFunction("endDraw");
            base.EndDraw();
        }
        protected override void UnloadContent()
        {
            SaveSetting();
            base.Content.Unload();
        }
        //Методы связанные с Initalize
        public const string ConfigFolder = @"Data/Config";
        public const string ModsFolder = @"Data/Mods";
        public void changeDisplayMode(DisplayMode disp)
        {
            graphics.PreferredBackBufferHeight = disp.Height;
            graphics.PreferredBackBufferFormat = disp.Format;
            graphics.PreferredBackBufferWidth = disp.Width;
            graphics.ApplyChanges();
            
        }
        void LoadSetting()
        {
            Program.StartSw();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(ConfigFolder+@"/Graphics.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine(xnode.Name);
                // если узел - company
                if (xnode.Name == "Width")
                {
                    graphics.PreferredBackBufferWidth = int.Parse(xnode.InnerText);
                }
                // если узел - company
                if (xnode.Name == "Height")
                {
                    graphics.PreferredBackBufferHeight = int.Parse(xnode.InnerText);
                }
                // если узел - company
                System.Console.WriteLine(xnode.Name.ToString());
                if (xnode.Name == "FullScreen")
                {
                    System.Console.WriteLine(int.Parse(xnode.InnerText));
                    if (int.Parse(xnode.InnerText) == 0) graphics.IsFullScreen = false;
                }
            }
            Program.StopSw("Init Graphics Setting");

            Program.StartSw();
            xDoc = new XmlDocument();
            xDoc.Load(ConfigFolder + @"/Sound.xml");
            // получим корневой элемент
            xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine(xnode.Name);
                // если узел - company
                if (xnode.Name == "Mutted")
                {
                    System.Console.WriteLine(int.Parse(xnode.InnerText));
                    if (int.Parse(xnode.InnerText) != 0)MediaPlayer.IsMuted = true;
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
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine(xnode.Name);
                // если узел - company
                if (xnode.Name == "Width")
                {
                    xnode.InnerText = graphics.PreferredBackBufferWidth.ToString();
                }
                // если узел - company
                if (xnode.Name == "Height")
                {
                    xnode.InnerText = graphics.PreferredBackBufferHeight.ToString();
                }
                // если узел - company
                System.Console.WriteLine(xnode.Name.ToString());
                if (xnode.Name == "FullScreen")
                {
                    System.Console.WriteLine(int.Parse(xnode.InnerText));
                    if (graphics.IsFullScreen) xnode.InnerText = "1";
                    else xnode.InnerText = "0";
                }
            }

            File.Delete(ConfigFolder + @"/Graphics.xml");
            xDoc.Save(ConfigFolder + @"/Graphics.xml");
            System.Console.WriteLine("TEST");
            Program.StopSw("Save Graphics Setting");
            ///
            Program.StartSw();
            xDoc = new XmlDocument();
            xDoc.Load(ConfigFolder + @"/Sound.xml");
            // получим корневой элемент

            xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine(xnode.Name);
                // если узел - company
                // если узел - company
                System.Console.WriteLine(xnode.Name.ToString());
                if (xnode.Name == "Mutted")
                {
                    if (MediaPlayer.IsMuted) xnode.InnerText = "1";
                    else xnode.InnerText = "0";
                }
            }

            File.Delete(ConfigFolder + @"/Sound.xml");
            xDoc.Save(ConfigFolder + @"/Sound.xml");
            System.Console.WriteLine("TEST");
            Program.StopSw("Save Graphics Setting");
        }
        void LoadInput()
        {
            Program.StartSw();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(ConfigFolder + @"/Input.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine(xnode.Name);
                // если узел - company
                if (xnode.Name == "Jump")
                {
                    GameInput.Jump = int.Parse(xnode.InnerText);
                }
                // если узел - company
                else if (xnode.Name == "Left")
                {
                    GameInput.MoveLeft = int.Parse(xnode.InnerText);
                }
                else if (xnode.Name == "Right")
                {
                    GameInput.MoveRight = int.Parse(xnode.InnerText);
                }
                else if(xnode.Name == "ActiveIventory"){
                    GameInput.ActiveIventory = int.Parse(xnode.InnerText);
                }
                if (xnode.Name == "Drop")
                {
                    GameInput.Drop = int.Parse(xnode.InnerText);
                }
            }
            Program.StopSw("Init Input Setting");
        }
        //Методы связанные с LoadContent
        private void CreateConsole()
        {
            Program.StartSw();
            console = new TwoSides.GUI.Console(Font1);
            console.addLog("Maximum:" + (Item.itemmax + TileTwoSides.TileMax));
            Program.StopSw("Creating Console");
        }


  
        void LoadBackImage()
        {
            black = base.Content.Load<Texture2D>(ImageFolder + "black");
            Program.StartSw();
            for (int i = 0; i < 3; i++)
            {
                background[i] = Content.Load<Texture2D>(ImageFolder + "background_" + i);
            }
            texturestonetile = Content.Load<Texture2D>(ImageFolder + "stonetiles");
            Program.StopSw("Loaded Background");
        }
        void LoadedHuman()
        {
            Program.StartSw();
            blood = Content.Load<Texture2D>(ImageFolder + "blood");
            head = base.Content.Load<Texture2D>(ImageFolder + "head");
            hand = base.Content.Load<Texture2D>(ImageFolder + "hand");
            eye = base.Content.Load<Texture2D>(ImageFolder + "eyes");
            head2 = base.Content.Load<Texture2D>(ImageFolder + "head2");
            body = base.Content.Load<Texture2D>(ImageFolder + "body");
            legs = base.Content.Load<Texture2D>(ImageFolder + "legs");
            shadow = base.Content.Load<Texture2D>(ImageFolder + "shadow");
            Program.StopSw("Loaded Human");
        }
        public  const string fontFolder = @"Fonts\";
        void LoadedGui()
        {
            Program.StartSw();
            button = base.Content.Load<Texture2D>(ImageFolder + "button");
            carma = base.Content.Load<Texture2D>(ImageFolder + "carma");
            achivement = base.Content.Load<Texture2D>(ImageFolder + "achivement");
            cursor = base.Content.Load<Texture2D>(ImageFolder + "cursor");
            inv = base.Content.Load<Texture2D>(ImageFolder + "invsphere");
            galka = base.Content.Load<Texture2D>(ImageFolder + "galka");
            ramka = base.Content.Load<Texture2D>(ImageFolder + "ramka");
            emptyDrougt = base.Content.Load<Texture2D>(ImageFolder + @"hud/emptyDrougt");
            fullDrougt = base.Content.Load<Texture2D>(ImageFolder + @"hud/fullDrougt");
            wolfskin = base.Content.Load<Texture2D>(ImageFolder + @"NPC/wolf");
            eatHunger = base.Content.Load<Texture2D>(ImageFolder + @"hud/hunger");
            explosion = base.Content.Load<Texture2D>(ImageFolder + @"explosion");
            music = Content.Load<Song>("DestinationUnknown");
            snowTexture = base.Content.Load<Texture2D>(ImageFolder + "snow");
            bloodBody = base.Content.Load<Texture2D>(ImageFolder + @"skelet/body");
            for(int i = 0;i<4;i++)
            bloodTexture[i] = base.Content.Load<Texture2D>(ImageFolder + @"skelet/"+i);
            dialogtex = carma;
            Program.StopSw("Loaded Image Gui (50 milisecond First load Content)");

            Program.StartSw();
            Font1 = Content.Load<SpriteFont>(fontFolder+ "myfont");
            font2 = Content.Load<SpriteFont>(fontFolder + "myfont2");
            Font3 = Content.Load<SpriteFont>(fontFolder + "myfont3");
            Program.StopSw("Loaded Font Gui");
        }
        void LoadRecipe()
        {
            Program.StartSw();
            Recipe test = new Recipe(new Item(1, 6), 100.0f);

            test.addigr(7, 2);
            test.addigr(5, 1);
            recipes.Add(test);
            recipes.Add(new Recipe(new Item( 5,    19),    20.0f));
            recipes.Add(new Recipe(new Item(1,    8),     20.0f));
            recipes.Add(new Recipe(new Item(1,22),   20.0f));
            recipes.Add(new Recipe(new Item(1,23),    20.0f));
            recipes.Add(new Recipe(new Item(1, 24), 20.0f));
            ((Recipe)recipes[1]).addigr(5, 1);
            ((Recipe)recipes[2]).addigr(19, 2);
            ((Recipe)recipes[2]).addigr(1, 3);
            ((Recipe)recipes[3]).addigr(1, 3);
            ((Recipe)recipes[4]).addigr(1, 2);
            ((Recipe)recipes[5]).addigr(1, 3);
            //NEW UPDATE

            recipes.Add(new Recipe(new Item(1, 26), 20.0f));
            ((Recipe)recipes[6]).addigr(19, 6);
            ((Recipe)recipes[6]).addigr(5, 6);
            recipes.Add(new Recipe(new Item(1, 27), 20.0f));
            ((Recipe)recipes[7]).addigr(19, 4);
            ((Recipe)recipes[7]).addigr(5, 3);
            recipes.Add(new Recipe(new Item(1, 28), 20.0f));
            ((Recipe)recipes[8]).addigr(1, 10);
           
            recipes.Add(new Recipe(new Item(1, 29),22, 20.0f));
            ((Recipe)recipes[9]).addigr(3, 4);
            
            recipes.Add(new Recipe(new Item(1, 30),22, 20.0f));
            ((Recipe)recipes[10]).addigr(2, 4);
            //Anvil
            recipes.Add(new Recipe(new Item(1, 31), 20.0f));
            ((Recipe)recipes[11]).addigr(29, 5);
            recipes.Add(new Recipe(new Item(1, 32), 20.0f));
            ((Recipe)recipes[12]).addigr(29, 4);
            recipes.Add(new Recipe(new Item(1, 33), 20.0f));
            ((Recipe)recipes[13]).addigr(29, 5);
            recipes.Add(new Recipe(new Item(1, 34), 20.0f)); // sword
            ((Recipe)recipes[14]).addigr(29, 1);
            ((Recipe)recipes[14]).addigr(19, 2);
            //End Anvil
            recipes.Add(new Recipe(new Item(1, 38),22, 20.0f));
            ((Recipe)recipes[15]).addigr(36, 4);

            recipes.Add(new Recipe(new Item(1, 43), 20.0f)); // hammer
            ((Recipe)recipes[16]).addigr(1, 6);
            ((Recipe)recipes[16]).addigr(19, 1);
            //Anvil
            recipes.Add(new Recipe(new Item(1, 41), 22, 20.0f)); // pickaxe
            ((Recipe)recipes[17]).addigr(29, 3);
            ((Recipe)recipes[17]).addigr(19, 1);
            recipes.Add(new Recipe(new Item(1, 44), 22, 20.0f));
            ((Recipe)recipes[18]).addigr(2, 1);
            ((Recipe)recipes[18]).addigr(19, 2);
            //End Anvil

            recipes.Add(new Recipe(new Item(1, 39), 20.0f));
            ((Recipe)recipes[19]).addigr(38, 4);
            recipes.Add(new Recipe(new Item(1, 45), 22, 20.0f));
            ((Recipe)recipes[20]).addigr(1, 1);
            ((Recipe)recipes[20]).addigr(19, 2);

            recipes.Add(new Recipe(new Item(4, 47), 22, 20.0f));
            ((Recipe)recipes[21]).addigr(46, 1);

            recipes.Add(new Recipe(new Item(1, 48), 22, 20.0f));
            ((Recipe)recipes[22]).addigr(49, 1);
            ((Recipe)recipes[22]).addigr(46, 1);

            recipes.Add(new Recipe(new Item(1, 49), 22, 20.0f));
            ((Recipe)recipes[23]).addigr(47, 1);
            ((Recipe)recipes[23]).addigr(19, 1);

            Program.StopSw("Created Recipe");
        }
        Texture2D loadfromPngTex2D(string path)
        {
            FileStream setStream = File.Open(path, FileMode.Open);
            Texture2D newTex = Texture2D.FromStream(GraphicsDevice, setStream);
            setStream.Dispose();
            return newTex;
        }
        void LoadModeClothes()
        {
            Program.StartSw();
            foreach (string beltPath in ModLoader.ModLoader.beltMods)
            {
                Clothes.beltMods.Add(loadfromPngTex2D(beltPath));

            }
            foreach (string shirtPath in ModLoader.ModLoader.ShirtMods)
            {
                Clothes.shirtMods.Add(loadfromPngTex2D(shirtPath));

            }
            foreach (string specialPath in ModLoader.ModLoader.SpecialMods)
            {
                Clothes.specialMods.Add(loadfromPngTex2D(specialPath));

            }
            foreach (string pantsPath in ModLoader.ModLoader.PantsMods)
            {
                Clothes.pantsMods.Add(loadfromPngTex2D(pantsPath));

            }
            Program.StopSw("Load Mods Clothes");
        }
        
        void LoadModeItems()
        {
            Program.StartSw();
            foreach (ItemMod im in ModLoader.ModLoader.ItemsMods)
            {
                im.setTexture(loadfromPngTex2D(im.IMG));
            }
            Program.StopSw("Load Mods Items");
        }
        
        void LoadModeBlocks()
        {
            Program.StartSw();
            foreach (BlockMod bm in ModLoader.ModLoader.BlocksMods)
            {
                bm.setTexture(loadfromPngTex2D(bm.IMG));
            }
            Program.StopSw("Load Mods Blocks");
            LoadModeItems();
        }

        void LoadMods()
        {
            Program.StartSw();
            ModLoader.ModLoader.loadModedList(ModsFolder);
            Program.StopSw("Init Mods");
        }



        const string nameSubVersion = "Alpha";
        const string nameVersion = "Hammer & Anvil";
        const byte idSubVersion = 0;
        const byte idVersion = 0;
        const byte major = 0;
        const byte minor = 0;
        const byte build = 2;
        const byte revesion = 0;
        NamingVersion versionThis = new NamingVersion(idSubVersion,idVersion,major,minor,build,revesion);
        Song music;
        //Методы меню
        public void loadSlots()
        {
            for (int i = 0; i < 6; i++)
            {
                slots[i] = Content.Load<Texture2D>(ImageFolder + "slot\\slot_" + i);
            }
        }

        public void loadClient(object thread)
        {
            controlScene.changeScene(Progress.instance);
            currentD = 0;
            dimension[0].cleardimension();
            NetPlay.sendMsg(3,0 ,0, 20,0 , SizeGeneratior.WorldHeight);
            while (!dimension[currentD].active) { }
            startGame();
        }
        public void newGeneration(object thread)
        {
            controlScene.changeScene(Progress.instance);
            for (int i = 0; i < MaxD; i++)
            {
                currentD = i;
                dimension[i].cleardimension();
                dimension[i].start(Progress.instance.bar);
            }
            startGame();
        }

        void startGame() {

            dimension[1].globallighting = 1;
            dimension[0].globallighting = 51;
            currentD = 0;
            player.spawn();
            controlScene.changeScene(new GameScene());
            MediaPlayer.Play(music);
        }

        //Методы выборы Расы
        void LoadStdRaces()
        {
            int r = -1;
            Program.StartSw();
            Race.LoadRace(Font1,heightmenu,button,ConfigFolder+@"/racelist.xml", ref r);
            Program.StopSw("Loaded default Race's");
            Program.StartSw();
            foreach (ModFile mod in ModLoader.ModLoader.Mods)
            {
                foreach (string RLP in mod.getPathRaceList())
                {
                    string a = RLP;
                    if (new FileInfo(a).Exists) Race.LoadRace(Font1, heightmenu, button, a, ref r);
                }
            }
            Program.StopSw("Loaded User Race's");
        }

        void standartkey()
        {
            for (int i = 0; i < 2; i++)
            {
                Rectangle rect = new Rectangle(120, (120) + 27 * i, 100, 25);
                if (i == 1) racestd[i] = new Race(new Color(0, 0, 0), new Button(button, Font1, rect, "Niger"));
                else racestd[i] = new Race(new Color(0, 0, 0), new Button(button, Font1, rect, "Europian"));
            }
        }
        bool keyconsole = false;
        bool day = true;
        public Camera camera = new Camera();
        //Методы игры
        public void GameUpdate(GameTime gameTime)
        {
          
            var delta = (float)gameTime.ElapsedGameTime.TotalMinutes;
            player.update(delta, seconds);
            foreach (Zombie npc in Program.game.dimension[Program.game.currentD].Zombies) npc.update();
            foreach (Civilian npc in Program.game.dimension[Program.game.currentD].civil) npc.update();
            dimension[currentD].update(gameTime,camera);
            camera.pos.X = (int)player.position.X;
            camera.pos.Y = (int)player.position.Y;
            if (camera.pos.Y> (SizeGeneratior.WorldHeight - 1) * ITile.TileMaxSize) camera.pos.Y = (SizeGeneratior.WorldHeight - 1) * ITile.TileMaxSize;
            if (camera.pos.Y < graphics.PreferredBackBufferHeight/2) camera.pos.Y = graphics.PreferredBackBufferHeight/2;
            if (camera.pos.X > (SizeGeneratior.WorldWidth - 1) * ITile.TileMaxSize) camera.pos.X = (SizeGeneratior.WorldWidth - 1) * ITile.TileMaxSize;
            if (camera.getLeftUpper.X < 0) camera.getLeftUpper = new Vector2(0,camera.getLeftUpper.Y);
            seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!keyconsole && keyState.IsKeyDown(Keys.OemTilde))
            {
                console.changeactive();
                keyconsole = true;
            }
            else if (keyState.IsKeyUp(Keys.OemTilde)) keyconsole = false;
            if (console.isactive) console.update();
            if (day) dimension[0].globallighting += (brightness / 24) * delta;
            else dimension[0].globallighting -= (brightness / 24) * delta;
            if (dimension[0].globallighting >= brightness && day) { day = false; }
            else if (dimension[0].globallighting <= 0 && !day) { day = true; }
            if (spawnTime > 0) spawnTime--;
            else if (rand.Next(0, 5*((NormalWorld)dimension[0]).wolfs.Count+1) == 0 )
            {
                int spawnX = (int)(player.position.X+graphics.PreferredBackBufferWidth)/ITile.TileMaxSize;
                spawnTime = 100;
               //((NormalWorld)dimension[0]).wolfs.Add(new Wolf(spawnX,wolfskin));
            }
            foreach (Bullet bullet in bullets)
            {
                if (!bullet.move())
                {
                    bullet.destory();
                    bullets.Remove(bullet);
                    break;

                }
            }
            foreach (Drop drops in drop)
            {
                drops.update();
            }
        }
        public int spawnTime = 0;
        public void AddBulletToMouse(int x,int y)
        {
            bullets.Add(new Bullet(new Vector2(x, y), Util.directional(new Vector2(x, y))));
        }
        //RenderTarget2D shadowSprite;
        public void GameDraw(SpriteEffects effect)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            Rectangle dest2 = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Rectangle src2 = new Rectangle((int)camera.getLeftUpper.X, (int)camera.getLeftUpper.Y, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            spriteBatch.Draw(texturestonetile, dest2, src2, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            //  spriteBatch.Draw(background[1], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.5f),Genworld.rocklayer-background[1].Height, background[1].Width, graphics.PreferredBackBufferHeight), Color.White);
            //   spriteBatch.Draw(background[2], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.8f), Genworld.rocklayer, background[2].Width, graphics.PreferredBackBufferHeight), Color.White);

            //spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            spriteBatch.End();
            // else if (currentD >= 1) GraphicsDevice.Clear(Color.Red);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null);
            //spriteBatch.Draw(background[currentD], new Vector2(-ScreenPosX,-ScreenPosY), new Rectangle(0, 0, (CWorld.TileMaxX * Tile.TileMaxSize), (CWorld.TileMaxY * Tile.TileMaxSize)), Color.White);
            //  Rectangle dest = new Rectangle(-ScreenPosX, -ScreenPosY + CWorld.TileMaxY / 3 * Tile.TileMaxSize, CWorld.TileMaxX * Tile.TileMaxSize, (CWorld.TileMaxY - CWorld.TileMaxY / 3 - CWorld.TileMaxY / 3) * Tile.TileMaxSize);

            Rectangle dest = new Rectangle(0, (int)-camera.pos.Y + SizeGeneratior.WorldHeight / 3 * ITile.TileMaxSize + 600 * ITile.TileMaxSize,
                graphics.PreferredBackBufferWidth, (SizeGeneratior.WorldHeight - SizeGeneratior.WorldHeight / 3 - SizeGeneratior.WorldHeight / 3 - 600) * ITile.TileMaxSize);
            Rectangle src = new Rectangle(0, 0, 48, 1300);
            spriteBatch.Draw(background[currentD], dest, src, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            //  spriteBatch.Draw(background[1], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.5f),Genworld.rocklayer-background[1].Height, background[1].Width, graphics.PreferredBackBufferHeight), Color.White);
            //   spriteBatch.Draw(background[2], new Vector2(0, 0), new Rectangle((int)(ScreenPosX * 0.8f), Genworld.rocklayer, background[2].Width, graphics.PreferredBackBufferHeight), Color.White);

            //spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            spriteBatch.End();

            drawBackground(0.9f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.getViewTran(graphics));
            int i0 = (int)((camera.getLeftUpper.X) / ITile.TileMaxSize - 1f);
            int i1 = (int)((camera.getRightDown.X) / ITile.TileMaxSize) + 2;
            int j0 = (int)((camera.getLeftUpper.Y) / ITile.TileMaxSize - 1f);
            int j1 = (int)((camera.getRightDown.Y) / ITile.TileMaxSize) + 2;
            if(NetPlay.typeNet == 2) NetPlay.sendMsg(3, 0, i0, i1, j0, j1);
            tiles.renderWall(i0, i1, j0, j1,spriteBatch);
            tiles.renderPlasters(i0, i1, j0, j1, spriteBatch);
            tiles.renderTiles(i0, i1, j0, j1, spriteBatch);
            // spriteBatch.DrawString(Font1, onetwo.ToString(), new Vector2(120, 130), Color.White)
            player.render(hand,eye, head, body, legs,spriteBatch, shadow);

            //  spriteBatch.Draw(legs2, new Rectangle((int)(player.position.X + (player.width - head.Width) - ScreenPosX), (int)(player.position.Y - ScreenPosY), legs.Width, legs.Height), new Rectangle(0, 0, legs.Width, legs.Height), Color.Red, 0, Vector2.Zero, effect, 0);

            foreach (Bullet bullet in bullets)
            {
                bullet.par.draw(spriteBatch);
                spriteBatch.Draw(tiles.textures[1],
                    new Rectangle((int)(bullet.position.X), (int)(bullet.position.Y),
                        16,
                        16),
                        Color.White);
            }
            foreach(Cloud cloud in dimension[currentD].cloud)
            {
                
                spriteBatch.Draw(tiles.textures[1],
                    new Rectangle((int)(cloud.position.X), (int)(cloud.position.Y),
                        16,
                        16),
                        Color.White);
            }
            effect = SpriteEffects.None;
            foreach (Zombie npc in Program.game.dimension[Program.game.currentD].Zombies)
            {
                npc.DrawNPC(effect, spriteBatch, Font1, head, head2, body, legs,blood,eye, shadow);
            }
            foreach (Civilian npc in Program.game.dimension[Program.game.currentD].civil)
            {
                npc.DrawNPC(effect, spriteBatch, Font1, head, head2, body, legs, shadow);
            }
            foreach (Drop drops in drop)
            {
                drops.render(spriteBatch, (int)drops.position.X, (int)drops.position.Y);
            }
            foreach (Drop drops in drop)
            {
                if (player.rect.Intersects(new Rectangle((int)drops.position.X, (int)drops.position.Y, 16, 16)))
                {
                    drops.getslot().IsEmpty = false;
                    player.setslot(drops.getslot());
                    drop.Remove(drops);
                    break;
                }
            }
            isSnowing = dimension[currentD].mapB[(int)player.position.X / ITile.TileMaxSize] == ArrayResource.snow;
            DrawSnow();
            renderLight(i0, i1, j0, j1);
            dimension[currentD].Draw(spriteBatch);
            player.renderfog(fog, spriteBatch);
            spriteBatch.Begin();
            drawHud();
            if (console.isactive)
            {
                spriteBatch.End();
                console.draw(spriteBatch);
                spriteBatch.Begin();
            }
            spriteBatch.End();
        }
        private bool isSnowing = false;        
        private Texture2D snowTexture;        
        private Random random;
        Point[] snow = new Point[50];
        public void DrawSnow()
        {     
            Point quitPoint;                   
            Point startPoint;     
            quitPoint = new Point(0,
               (int)this.dimension[currentD].mapHeight[0]*ITile.TileMaxSize+ this.snowTexture.Height);

            //Set the snow's start point. It's the top of the screen minus the texture's height so it looks like it comes from somewhere, rather than appearing
            startPoint = new Point(0,
                (int)this.camera.getLeftUpper.Y - this.snowTexture.Height);
            //If it's not supposed to be snowing, exit
            if (!isSnowing)
                return;

            //This will be used as the index within our snow array
            int i;

            //NOTE: The following conditional is not exactly the best "initializer."
            //If snow has not been initialized
            if (this.snow[0] == new Point(0, 0))
            {
                //Make the random a new random
                this.random = new Random();

                //For every snow particle within our snow array,
                for (i = 0; i < this.snow.Length; i++)
                {
                    //Give it a new, random x and y. This will give the illusion that it was already snowing
                    //and won't cluster the particles
                    this.snow[i] = new Point(
                        (random.Next((int)this.camera.getLeftUpper.X, (int)(this.camera.getLeftUpper.X + graphics.PreferredBackBufferWidth * this.camera.Zoom - this.snowTexture.Width))),
                        (random.Next(0, (this.graphics.PreferredBackBufferHeight))));
                    this.snow[i].Y =random.Next((int)this.camera.getLeftUpper.Y,(int)(this.camera.getLeftUpper.Y + graphics.PreferredBackBufferHeight * this.camera.Zoom));
                }
            }

            //Make the random a new random (again, if just starting)
            this.random = new Random();

            //Go back to the beginning of the snow array
            i = 0;

            //Begin displaying the snow
            foreach (Point snowPnt in this.snow)
            {
                //Get the exact rectangle for the snow particle
                Rectangle snowParticle = new Rectangle(
                    snowPnt.X, snowPnt.Y, this.snowTexture.Width, this.snowTexture.Height);

                this.spriteBatch.Draw(this.snowTexture, snowParticle, Color.White);

                this.snow[i].Y += random.Next(0, 5);

                if (this.snow[i].Y >= quitPoint.Y)
                    this.snow[i] = new Point(
                        (random.Next((int)this.camera.getLeftUpper.X, (int)(this.camera.getLeftUpper.X + graphics.PreferredBackBufferWidth * this.camera.Zoom - this.snowTexture.Width))),
                        startPoint.Y);

                i++;
            }
        }

        private void drawBackground(float zoom)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicWrap, null, null, null, camera.getViewTran(graphics, zoom));
            Rectangle src = new Rectangle(0, 0, (int)(SizeGeneratior.WorldWidth * ITile.TileMaxSize / zoom), (int)(background[2].Height));
            Rectangle dest = new Rectangle(-src.Width, (dimension[currentD].mapB[(int)player.position.X/ITile.TileMaxSize].maxHeight * ITile.TileMaxSize) - (int)(src.Height * zoom), src.Width, src.Height);
            src.Width *= 2;
            dest.Width = src.Width;
            spriteBatch.Draw(background[2], dest, src, Color.White);
            spriteBatch.End();
        }
        public const bool isDebug = false;
     
        //Сообственные методы

        bool isnumkey()
        {
            return !(keyState.IsKeyUp(Keys.D1) && keyState.IsKeyUp(Keys.D2) && keyState.IsKeyUp(Keys.D3) && keyState.IsKeyUp(Keys.D4)
                && keyState.IsKeyUp(Keys.D5) && keyState.IsKeyUp(Keys.D6) && keyState.IsKeyUp(Keys.D7) && keyState.IsKeyUp(Keys.D8)
                && keyState.IsKeyUp(Keys.D9));
        }
        NamingVersion versionMap = new NamingVersion(0, 0, 0, 0, 0, 0);
        void load(object thread)
        {
            FileStream stream = File.OpenRead("save.sav");
            BinaryReader reader = new BinaryReader(stream);
            versionMap.load(reader);
            for (int d = 0; d < MaxD; d++)
            {
                currentD = d;
                dimension[d].cleardimension();
                dimension[d].load(reader,Progress.instance.bar);
                
            }
            player.load(reader);
            startGame();
            reader.Close();
        }
        public void loadMap()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(load));
        }

        void save(object thread)
        {
            FileStream stream =  File.Create("save.sav");
            BinaryWriter writer = new BinaryWriter(stream);
            versionThis.save(writer);
            for (int d = 0; d < MaxD; d++)
            {
                dimension[d].save(writer,Progress.instance.bar);
                
            }
            player.save(writer);
            writer.Close();
            controlScene.returnScene();
        }
        public string getVersion()
        {
            return nameSubVersion + " " + versionThis.getVersion();
        }

        public string getFullVersion()
        {
            return "Two sides " + nameVersion+" "+nameSubVersion + " " + versionThis.getVersion();
        }
        public void saveMap()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(save));
        }
        
        void renderLight(int i0, int i1, int j0, int j1)
        {
            for (int i = i0; i < i1; i++)
            {

                if (i < SizeGeneratior.WorldWidth && i >= 0) dimension[Program.game.currentD].maxy(i);
                for (int j = j0; j < j1; j++)
                {
                    if (i > SizeGeneratior.WorldWidth - 1 || i < 0) continue;
                    if (j > SizeGeneratior.WorldHeight - 1 || j < 0) continue;
                    //dimension[currentD].globallighting = 1;
                    dimension[currentD].map[i, j].lightu(i, j, (int)dimension[currentD].globallighting,SizeGeneratior.rockLayer);
                    dimension[currentD].map[i, j].lightupdate(i, j);
                    int lighting = 255 - dimension[currentD].map[i, j].light * 5;
                     spriteBatch.Draw(black, new Vector2(ITile.TileMaxSize * i, ITile.TileMaxSize * j), new Color(0, 0, 0, lighting));
                }
            }
        }
        
        void drawHud()
        {
            if (player.controlJ)
            {
                drawSlotItem(player.currentMaxSlot);
                int r = -1;
                for (int i = Player.slotmax; i < Player.slotmax + 3; i++)
                {
                    spriteBatch.Draw(inv, new Rectangle((i - Player.slotmax) * 32, graphics.PreferredBackBufferHeight - 32, 32, 32), Color.White);
                    if (!player.slot[i].IsEmpty)
                        player.slot[i].Render(spriteBatch, (i - Player.slotmax) * 32 + 8, graphics.PreferredBackBufferHeight - 32 + 8);
                }

                int a = 0;
                for (int i = 0; i < recipes.Count; i++)
                {
                    if (player.getvalidrecipes(i))
                    {
                        spriteBatch.Draw(inv, new Rectangle(a * 32, 32 + 32 * r, 32, 32), Color.Black);
                        ((Recipe)recipes[i]).slot.Render(spriteBatch, (a * 32) + 16 - 8, 35 + (16 - 8) + 32 * r);
                        for (int j = 0; j < ((Recipe)recipes[i]).getsize(); j++)
                        {
                            spriteBatch.Draw(inv, new Rectangle(a * 32, (j + 2) * 35 + 32 * r, 32, 32), Color.Green);
                            Item.Render(spriteBatch, ((Recipe)recipes[i]).getigr(j), (a * 32) + 16 - 8, (j + 2) * 35 + (16 - 8) + 32 * r);
                            spriteBatch.DrawString(Font1, ((Recipe)recipes[i]).getconut(j).ToString(), new Vector2(((a + 1) * 32) - 10, (j + 2) * 35 + (16 - 8) + 32 * r), Color.White);
                        }
                        a++;
                    }
                }
                drawBloodBody(a*32, 0);
            }
            else
            {
                drawSlotItem(9);
                drawBloodBody(0, 0);
            } int wcarma = (int)(10 + SizeCarmaWidth * (0.01 * player.carma));
            int hptext = (int)(player.width * (player.blood/5000));
            int color = (int)(200 * (0.01 * player.carma));
            if (!player.MouseItem.IsEmpty) player.MouseItem.Render(spriteBatch, mouseState.X + 16, mouseState.Y + 5);
            spriteBatch.Draw(carma, new Rectangle(graphics.PreferredBackBufferWidth - wcarma, graphics.PreferredBackBufferHeight - SizeCarmaHeight, wcarma, SizeCarmaHeight), new Color(color * (color / 255), color * (color / 255), color, color));
            spriteBatch.DrawString(Font3, "Karma", new Vector2(graphics.PreferredBackBufferWidth - wcarma, graphics.PreferredBackBufferHeight - Font3.MeasureString("Karma").Y ), Color.White);

            spriteBatch.Draw(carma, new Rectangle((int)(player.position.X)+player.width/2-hptext/2, (int)(player.position.Y)-SizeCarmaHeight/2, hptext, SizeCarmaHeight/2), Color.Red);
           // spriteBatch.DrawString(Font1, "Blood", new Vector2(graphics.PreferredBackBufferWidth / 2 - hptext / 2, Font1.MeasureString("Blood").Y/2), Color.White);

            //spriteBatch.Draw(carma, new Rectangle(graphics.PreferredBackBufferWidth - 16, 16, 16, 16 + (int)player.drought), Color.Blue);
            spriteBatch.Draw(fullDrougt, new Rectangle(graphics.PreferredBackBufferWidth - 16, graphics.PreferredBackBufferHeight-SizeCarmaHeight-16, 16, 16),new Rectangle(0,0,fullDrougt.Width,(int)(player.drought/100*fullDrougt.Height)), Color.White);
            spriteBatch.Draw(emptyDrougt, new Rectangle(graphics.PreferredBackBufferWidth - 16, graphics.PreferredBackBufferHeight - SizeCarmaHeight - 16, 16, 16), Color.White);
            Vector2 sizeHunger = new Vector2(eatHunger.Width, eatHunger.Height*(player.hunger/100));
            spriteBatch.Draw(eatHunger, new Vector2(graphics.PreferredBackBufferWidth - wcarma-sizeHunger.X, graphics.PreferredBackBufferHeight - SizeCarmaHeight),new Rectangle(eatHunger.Width-(int)sizeHunger.X,0,(int)sizeHunger.X,(int)sizeHunger.Y), Color.White);

            foreach (Civilian npc in Program.game.dimension[Program.game.currentD].civil)
            {
                npc.renderDialog(spriteBatch);
            }
            // string temperature = "Player Temperature:" + player.Temperature + "*c";
             // spriteBatch.DrawString(Font1, temperature, new Vector2(graphics.PreferredBackBufferWidth - (Font1.MeasureString(temperature).X) - 5, SizeCarmaHeight + 120), Color.White);
            spriteBatch.End();
            int y = 0;
            for (int i = 0; i < player.Quests.Count; i++)
            {
                ((Quest)(player.Quests[i])).render(spriteBatch, new Vector2(graphics.PreferredBackBufferWidth - 10, 150 + y));
                y += ((Quest)(player.Quests[i])).getQuestHeight();
            } spriteBatch.Begin();

        }

        private void drawBloodBody(int xBody, int yBody)
        {
            spriteBatch.Draw(bloodBody, new Vector2(xBody, yBody), Color.White);
            for (int i = 0; i < 4; i++)
            {
                if (player.bloods[i]) spriteBatch.Draw(bloodTexture[i], new Vector2(xBody, yBody), Color.Red);
            }
        }

        private int drawSlotItem(int maxSlot)
        {
            int r = -1;
            int slotHover = -1;
            int centerSlot = graphics.PreferredBackBufferWidth - (9*32);
            for (int i = 0; i < maxSlot; i++)
            {
                if (i % 9 == 0) r++;
                int xslot = (i % 9) * 32;
                int yslot = 32 * r;
                Color cl = Color.White;
                if (i == player.selectedItem) cl = Color.Black;
                spriteBatch.Draw(inv, new Rectangle(centerSlot+xslot,yslot , 32, 32), cl);
                if (!player.slot[i].IsEmpty)
                {
                    player.slot[i].Render(spriteBatch,centerSlot+xslot + 16/2, 16/2 + yslot);
                    spriteBatch.DrawString(Font3, player.slot[i].ammount.ToString(), new Vector2(centerSlot+(32 * ((i % 9) + 1) - 5 - Font3.MeasureString(player.slot[i].ammount.ToString()).X), 32 - Font3.MeasureString(player.slot[i].ammount.ToString()).Y + yslot), Color.Black);

                    if (new Rectangle(centerSlot+xslot, yslot, 32, 32).Contains(new Point(mouseState.X, mouseState.Y)))
                    {
                        slotHover = i;
                    }
                }
            }//одежда
            if (slotHover >= 1)
                drawItemHelp(slotHover);
            return r;
        }

        private void drawItemHelp(int i)
        {
            spriteBatch.DrawString(font2, player.slot[i].GetName(), new Vector2(mouseState.X + 16,
                mouseState.Y + 5),
                player.slot[i].GetColor());
            spriteBatch.DrawString(font2, "DPS:" + player.slot[i].Getdamage() +
                "(" + player.slot[i].Getdamage() *
                (player.slot[i].HP / 100) + ")",
                new Vector2(mouseState.X + 16, mouseState.Y + 15),
                player.slot[i].GetColor());
            spriteBatch.DrawString(font2, "Power:" + player.slot[i].GetPower() +
                "(" + player.slot[i].GetPower() *
               (player.slot[i].HP / 100) + ")",
                new Vector2(mouseState.X + 16, mouseState.Y + 25),
                player.slot[i].GetColor());
        }

        void drawdebug()
        {
            int glint = (int)dimension[currentD].globallighting;
            string text = "brightness world:" + (glint).ToString();

            string height = "height:" + (SizeGeneratior.WorldHeight - (int)(player.position.Y / ITile.TileMaxSize)).ToString();
            spriteBatch.DrawString(Font1, text, new Vector2(graphics.PreferredBackBufferWidth - (text.Length * 7) - 5, SizeCarmaHeight + 20), Color.White);
            spriteBatch.DrawString(Font1, "FPS:" + globalfps.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 50, SizeCarmaHeight + 40), Color.White);
            spriteBatch.DrawString(Font1, height, new Vector2(graphics.PreferredBackBufferWidth - (height.Length * 7) - 5, SizeCarmaHeight + 60), Color.White);
            string dworld;
            if (currentD == 0) dworld = "World : Normal World";
            else if (currentD == 1) dworld = "World : Hell";
            else if (currentD == 2) dworld = "World : Snow World";
            else dworld = "World : ???";
            spriteBatch.DrawString(Font1, dworld, new Vector2(graphics.PreferredBackBufferWidth - (dworld.Length * 7) - 5, SizeCarmaHeight + 80), Color.White);
            string biotemp = "World Temperature:" + dimension[currentD].GetTemperature((int)player.position.X / ITile.TileMaxSize,
                (int)player.position.Y / ITile.TileMaxSize) + "*c";
            spriteBatch.DrawString(Font1, biotemp, new Vector2(graphics.PreferredBackBufferWidth - (biotemp.Length * 7) - 5, SizeCarmaHeight + 100), Color.White);
            Vector2 vec = Util.getcube((int)player.position.X, (int)player.position.Y);
            // glint = dimension[currentD].map[(int)vec.X,(int)vec.Y].light;
            text = "brightness Block:" + glint.ToString();
            spriteBatch.DrawString(Font1, text, new Vector2(graphics.PreferredBackBufferWidth - (text.Length * 7) - 5, SizeCarmaHeight + 140), Color.White);

            string position = "Player X:" + player.position.X + " position.y: " + player.position.Y;
            spriteBatch.DrawString(Font1, position, new Vector2(graphics.PreferredBackBufferWidth - (position.Length * 7) - 5, SizeCarmaHeight + 160), Color.White);

        }

        public void addGui(GUI.GUI gui)
        {
            guis.Add(gui);
        }

        public void RemoveGUI(GUI.GUI gui)
        {
            guiremove = gui;
        }

        public void adddrop(int x, int y, Item slot, float dirx = 0)
        {
            drop.Add(new Drop(slot, x, y, dirx));
           
        }

        public Texture2D fullDrougt { get; set; }

        public Texture2D emptyDrougt { get; set; }

        public Texture2D eatHunger { get; set; }
        Texture2D explosion;
        public void AddExplosion(Vector2 pos) {
            dimension[currentD].AddExplosion(pos, explosion);
        }

        public SpriteFont Font3 { get; set; }

        public Texture2D bloodBody { get; set; }

        public Texture2D[] bloodTexture = new Texture2D[4];

        public Texture2D hand { get; set; }

        public Texture2D wolfskin { get; set; }

        public Effect postEffect { get; set; }
    }
}
