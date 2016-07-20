using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoSides.GUI;
using System.Xml;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using TwoSides.Utils;

namespace TwoSides
{
    sealed public class Race
    {
        Button button;
        [NonSerialized]
        Color color;

        public static ArrayList racelist = new ArrayList();

        public Race(Color c, Button btn)
        {
            this.color = c;
            this.button = btn;
        }

        public Color getColor(){
            return color;
        }

        public Color getZombieColor()
        {
            return color;
        }

        public Button getButton()
        {
            return button;
        }
        public void Load(BinaryReader reader)
        {
            color = Util.readColor(reader);
        }
        public void Save(BinaryWriter writer)
        {
            Util.SaveColor(color, writer);
        }
        public static void LoadRace(SpriteFont Font1, int heightmenu, Texture2D buttonMenu, string FileName, ref int r)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(FileName);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {

                string name = "";
                int colorR = 255;
                int colorG = 255;
                int colorB = 255;
                // получаем атрибут name
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                        name = attr.Value;
                }
                // обходим все дочерние узлы элемента user
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // если узел - company
                    if (childnode.Name == "ColorR")
                    {
                        colorR = int.Parse(childnode.InnerText);
                    }
                    // если узел age
                    if (childnode.Name == "ColorG")
                    {
                        colorG = int.Parse(childnode.InnerText);
                    }
                    if (childnode.Name == "ColorB")
                    {
                        colorB = int.Parse(childnode.InnerText);
                    }
                }
                if (racelist.Count % 15 == 0) r++;
                Rectangle rect = new Rectangle(120 + 130 * r, heightmenu + 35 * (racelist.Count % 15), (int)Font1.MeasureString("Exit Game").X+50, 30);
                racelist.Add(new Race(new Color(colorR, colorG, colorB), new Button(buttonMenu, Font1, rect, name)));
            }
        }

    }
}
