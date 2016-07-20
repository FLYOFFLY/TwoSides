using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.GUI;
using TwoSides.World;
using NLua;
using TwoSides.GameContent.Tiles;
using TwoSides.World.Generation;
using Microsoft.Xna.Framework;

namespace TwoSides.ModLoader
{
   
    public class ModLoader
    {
        public static ArrayList Mods = new ArrayList();
        public static ArrayList beltMods = new ArrayList();
        public static ArrayList ShirtMods = new ArrayList();
        public static ArrayList PantsMods = new ArrayList();
        public static ArrayList SpecialMods = new ArrayList();
        public static ArrayList BlocksMods = new ArrayList();
        public static ArrayList ItemsMods = new ArrayList();
        public static List<Ore> OreMods = new List<Ore>();
        public static List<Biome> BiomesMods = new List<Biome>();

        public const int StartIdItem = 10000;
        public static int getIndexBlocks(int id)
        {
            return BlocksMods.IndexOf(new BlockMod(id, "",0,null));
        }
        public static int getIndexMode(string path)
        {
            return Mods.IndexOf(new ModFile(path));
        }
        public static object[] callFunction(string func, params object[] args)
        {
            foreach (ModFile mod in Mods)
            {
                foreach (LuaScript script in mod.getScripts())
                {
                    if (args == null)
                      return  script.callFunction(func);
                    else
                      return  script.callFunction(func, args);
                }
            }
            return null;
        }
        private static void ClothesModsParser(string modPath,string modFolder)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(modPath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                switch (xnode.Name)
                {
                    case "belt":
                        if (xnode.Attributes.Count > 0)
                        {
                            XmlNode attr = xnode.Attributes.GetNamedItem("path");
                            if (attr != null)
                            {
                                beltMods.Add(modFolder + @"\images\" + attr.Value);
                            }
                        }
                        break;
                    case "shirt":
                        if (xnode.Attributes.Count > 0)
                        {
                            XmlNode attr = xnode.Attributes.GetNamedItem("path");
                            if (attr != null)
                            {
                                ShirtMods.Add(modFolder + @"\images\" + attr.Value);
                            }
                        }
                        break;
                    case "pants":
                        if (xnode.Attributes.Count > 0)
                        {
                            XmlNode attr = xnode.Attributes.GetNamedItem("path");
                            if (attr != null)
                            {
                                PantsMods.Add(modFolder + @"\images\" + attr.Value);
                            }
                        }
                        break;
                    case "special":
                        if (xnode.Attributes.Count > 0)
                        {
                            XmlNode attr = xnode.Attributes.GetNamedItem("path");
                            if (attr != null)
                            {
                                SpecialMods.Add(modFolder + @"\images\" + attr.Value);
                            }
                        }
                        break;
                }
            }
        }
        
        private static void ItemsModsParser(string modPath, string modFolder)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(modPath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    int ids = 0;
                    string img = "";
                    int blocks = 0;
                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null) ids = int.Parse(attr.Value);

                    attr = xnode.Attributes.GetNamedItem("img");
                    if (attr != null) img = attr.Value;
                    switch (xnode.Name)
                    {
                        case "BlockIcon":
                            {

                                attr = xnode.Attributes.GetNamedItem("blockID");
                                if (attr != null) blocks = int.Parse(attr.Value);

                                ItemsMods.Add(new ItemMod(ids, modFolder + @"\images\" + img, (int)Item.Type.BLOCKICON, blocks + TileTwoSides.TileMax, modFolder));

                            }
                            break;
                        case "Eat":
                            {
                                attr = xnode.Attributes.GetNamedItem("hunger");
                                if (attr != null) blocks = int.Parse(attr.Value);

                                ItemsMods.Add(new ItemMod(ids, modFolder + @"\images\" + img, (int)Item.Type.EAT, blocks, modFolder));

                            }
                            break;

                        case "Water":
                            {
                                attr = xnode.Attributes.GetNamedItem("dunget");
                                if (attr != null) blocks = int.Parse(attr.Value);
                                ItemsMods.Add(new ItemMod(ids, modFolder + @"\images\" + img, (int)Item.Type.WATER, blocks, modFolder));

                            }
                            break;
                        case "CustomItem":
                            {
                                ItemsMods.Add(new ItemMod(ids, modFolder + @"\images\" + img, (int)Item.Type.MODDINGITEM, blocks, modFolder));

                            }
                            break;
                    }
                }
            }
        }
        private static void OreModsParser(string modPath, string modFolder)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(modPath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    int ids = 0;
                    int maxY, minY;
                    int range, change;

                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null) ids = int.Parse(attr.Value);
                    attr = xnode.Attributes.GetNamedItem("minY");
                    if (attr != null) minY = int.Parse(attr.Value);
                    attr = xnode.Attributes.GetNamedItem("maxY");
                    if (attr != null) maxY = int.Parse(attr.Value);
                    attr = xnode.Attributes.GetNamedItem("range");
                    if (attr != null) range = int.Parse(attr.Value);
                    attr = xnode.Attributes.GetNamedItem("change");
                    if (attr != null) change = int.Parse(attr.Value);

                }
            }
        }
        private static void BiomesModsParser(string modPath, string modFolder)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(modPath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    int t = 0;
                    int minHeight = 0, MaxHeight = 0, TopBlock = 0, stoneBlock = 0;
                    Color color = Color.White;

                    XmlNode temp = xnode.Attributes.GetNamedItem("Temperature");
                    if(temp != null)
                    t = int.Parse(temp.Value);

                    temp = xnode.Attributes.GetNamedItem("minHeight");
                    if (temp != null)
                        minHeight = int.Parse(temp.Value);

                    temp = xnode.Attributes.GetNamedItem("maxHeight");
                    if (temp != null)
                        MaxHeight = int.Parse(temp.Value);

                    temp = xnode.Attributes.GetNamedItem("TopBlock");
                    if (temp != null)
                        TopBlock = int.Parse(temp.Value);

                    temp = xnode.Attributes.GetNamedItem("StoneBlock");
                    if (temp != null)
                        stoneBlock = int.Parse(temp.Value);

                    BiomesMods.Add(new Biome(t,0,minHeight,MaxHeight,TopBlock,stoneBlock,color));
                }
            }
        }
        
        private static void BlocksModsParser(string modPath, string modFolder)
        {
            int size = BlocksMods.Count;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(modPath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    string img = "";
                    int id = 0;
                    XmlNode attr = xnode.Attributes.GetNamedItem("img");
                    if (attr != null)
                    {
                        img = attr.Value;
                    }
                    attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null)
                    {
                        id = int.Parse(attr.Value)+TileTwoSides.TileMax;
                    }
                    int hp = -1;
                    attr = xnode.Attributes.GetNamedItem("hp");
                    if (attr != null)
                    {
                        hp = int.Parse(attr.Value);
                    }
                    List<Item> droping = new List<Item>();
                    foreach (XmlNode xnode2 in xnode.ChildNodes)
                    {
                        int iddrop = 0, count = 0, type = 0;

                        System.Console.WriteLine(xnode2.Name); 
                        if (xnode2.Name == "drop")
                        {
                            System.Console.WriteLine("##");
                            attr = xnode2.Attributes.GetNamedItem("id");

                            foreach (XmlNode childnode in xnode2.ChildNodes)
                            {
                                switch (childnode.Name)
                                {
                                    case "id":
                                        iddrop = int.Parse(childnode.InnerText);
                                        break;

                                    case "count":
                                        count = int.Parse(childnode.InnerText);
                                        break;
                                }
                            }
                            System.Console.WriteLine(iddrop);
                            System.Console.WriteLine(count);
                            System.Console.WriteLine(type);
                            System.Console.WriteLine("####");
                            droping.Add(new Item(iddrop, count));
                        }
                    }
                    BlocksMods.Add(new BlockMod(id, modFolder + @"\images\"+img,hp,droping));
                }
            }
        }

        private static void RecipeModsParser(string modPath, string modFolder)
        {
            int size = BlocksMods.Count;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(modPath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine("###RECIPE STARTLIst###");
                if (xnode.Attributes.Count > 0)
                {
                    System.Console.WriteLine("###RECIPE START###");
                    int id = 0;
                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null)
                    {
                        id = int.Parse(attr.Value);
                    }
                    int hp = -1;
                    attr = xnode.Attributes.GetNamedItem("hp");
                    if (attr != null)
                    {
                        hp = int.Parse(attr.Value);
                    }
                    int ammout = -1;
                    attr = xnode.Attributes.GetNamedItem("ammout");
                    if (attr != null)
                    {
                        ammout = int.Parse(attr.Value);
                    }
                    Item item = new Item(ammout,id);
                    System.Console.WriteLine("TYPE:"+item.getTypeItem());
                    int recipeid = Program.game.recipes.Count;
                    Program.game.recipes.Add(new Recipe(item,hp));
                    foreach (XmlNode xnode2 in xnode.ChildNodes)
                    {
                        int iddrop = 0, count = 0;

                        System.Console.WriteLine(xnode2.Name);
                        if (xnode2.Name == "ingredient")
                        {
                            System.Console.WriteLine("##");
                            attr = xnode2.Attributes.GetNamedItem("id");

                            foreach (XmlNode childnode in xnode2.ChildNodes)
                            {
                                switch (childnode.Name)
                                {
                                    case "id":
                                        iddrop = int.Parse(childnode.InnerText);
                                        break;

                                    case "count":
                                        count = int.Parse(childnode.InnerText);
                                        break;
                                }
                            }
                            System.Console.WriteLine(iddrop);
                            System.Console.WriteLine(count);
                            System.Console.WriteLine("####");
                            ((Recipe)Program.game.recipes[recipeid]).addigr(iddrop, count);
                        }
                    }
                    System.Console.WriteLine("###RECIPE END###");
                }
            }
        }


        public static void loadModedList(string path){
            Mods.Clear();
            DirectoryInfo direct = new DirectoryInfo(path);
            DirectoryInfo[] modsDir = direct.GetDirectories();
            foreach (DirectoryInfo mod in modsDir)
            {
                string filename = mod.FullName + @"/config.xml";
                if (!new FileInfo(filename).Exists) continue;
                ModFile temp = new ModFile(mod.FullName);
                Mods.Add(temp);
                foreach (string CLFile in temp.getPathClothesList())
                {
                    ClothesModsParser(CLFile, mod.FullName);
                }

                foreach (string CLFile in temp.getPathItemsList())
                {
                    ItemsModsParser(CLFile, mod.FullName);
                }

                foreach (string CLFile in temp.getPathBlocksList())
                {
                    BlocksModsParser(CLFile, mod.FullName);
                }

                foreach (string CLFile in temp.getPathRecipeList())
                {
                    RecipeModsParser(CLFile, mod.FullName);
                }
                foreach (string CLFile in temp.getPathOreList())
                {
                    OreModsParser(CLFile, mod.FullName);
                }
                foreach (string CLFile in temp.getPathBiomeList())
                {
                    BiomesModsParser(CLFile, mod.FullName);
                }
            }
        }

        public static int getIndexItem(int id)
        {
            return ItemsMods.IndexOf(new ItemMod(id-StartIdItem, "", 0,0,""));
        }
    }
}
