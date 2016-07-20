using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;

namespace TwoSides.ModLoader
{
    class ModFile
    {
        private string ModPath;
        private ArrayList RaceListPath = new ArrayList();
        private ArrayList ClothesListPath = new ArrayList();
        private ArrayList Scripts = new ArrayList();
        private ArrayList BlocksList = new ArrayList();
        private ArrayList RecipeList = new ArrayList();
        private ArrayList ItemsList = new ArrayList();
        private ArrayList OreList = new ArrayList();
        private ArrayList BiomesList = new ArrayList();

        public ModFile(string modPath)
        {
            this.ModPath = modPath;
            ConfigParser(this.ModPath + @"/config.xml");
        }

        public override int GetHashCode()
        {
            return ModPath.GetHashCode();
        }

        public object[] callFunction(string func, params object[] args)
        {
            foreach (LuaScript script in Scripts)
            {
                if (args == null)
                    return script.callFunction(func);
                else
                    return script.callFunction(func, args);
            }
            return null;
        }
        private void ParserRaces(XmlNode xnode)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("fileName");
                if (attr != null)
                    RaceListPath.Add(this.ModPath + @"\" + attr.Value);
            }
        }

        private void ParserClothes(XmlNode xnode)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("fileName");
                if (attr != null)
                    ClothesListPath.Add(this.ModPath + @"\" + attr.Value);
            }
        }

        private void ParserLua(XmlNode xnode)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("fileName");
                if (attr != null)
                {
                    Scripts.Add(new LuaScript(this.ModPath + @"\" + attr.Value));
                }
            }
        }

        private void ParserBlocks(XmlNode xnode)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("fileName");
                if (attr != null)
                {
                    BlocksList.Add(this.ModPath + @"\" + attr.Value);
                }
            }
        }

        private void ParserItems(XmlNode xnode)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("fileName");
                if (attr != null)
                {
                    ItemsList.Add(this.ModPath + @"\" + attr.Value);
                }
            }
        }

        private void ParserRecipe(XmlNode xnode)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("fileName");
                if (attr != null)
                {
                    RecipeList.Add(this.ModPath + @"\" + attr.Value);
                }
            }
        }

        private void ConfigParser(string modPath)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(modPath);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                System.Console.WriteLine(xnode.Name);

                switch (xnode.Name)
                {
                    case "racelist":
                        ParserRaces(xnode);
                        break;
                    case "clotheslist":
                        ParserClothes(xnode);
                        break;
                    case "lua":
                        System.Console.WriteLine("LUA");
                        ParserLua(xnode);
                        break;
                    case "BlocksList":
                        ParserBlocks(xnode);
                        break;
                    case "ItemsList":
                        ParserItems(xnode);
                        break;
                    case "RecipeList":
                        ParserRecipe(xnode);
                        break;
                    case "OreList":
                        ParserOre(xnode);
                        break;

                    case "BiomesList":
                        ParserBiomes(xnode);
                        break;
                }
            }
        }

        private void ParserOre(XmlNode xnode)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("fileName");
                if (attr != null)
                {
                    OreList.Add(this.ModPath + @"\" + attr.Value);
                }
            };
        }

        private void ParserBiomes(XmlNode xnode)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("fileName");
                if (attr != null)
                {
                    BiomesList.Add(this.ModPath + @"\" + attr.Value);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ModFile)
            {
                if (((ModFile)obj).ModPath == this.ModPath) return true;
            }
            return false;
        }

        public string getPath()
        {
            return ModPath;
        }

        public ArrayList getPathClothesList()
        {
            return ClothesListPath;
        }

        public ArrayList getPathBlocksList()
        {
            return BlocksList;
        }

        public ArrayList getPathItemsList()
        {
            return ItemsList;
        }

        public ArrayList getPathRecipeList()
        {
            return RecipeList;
        }
        public ArrayList getPathRaceList()
        {
            return RaceListPath;
        }
        public ArrayList getPathOreList()
        {
            return OreList;
        }
        public ArrayList getPathBiomeList()
        {
            return BiomesList;
        }

        public ArrayList getScripts()
        {
            return Scripts;
        }
    }
}
