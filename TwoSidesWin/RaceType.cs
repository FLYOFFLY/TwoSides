using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GUI;

using static System.Console;

using static TwoSides.Utils.Tools;

namespace TwoSides
{
    public enum RaceType {
        EUROPIAN = 0,
        NIGER = 1
    }

    public sealed class Race
    {
        readonly Button _button;
        [NonSerialized]
        Color _color;

        public static List<Race> Racelist = new List<Race>();

        public Race(Color c, Button btn)
        {
            _color = c;
            _button = btn;
        }

        public Color GetColor() => _color;

        public Color GetZombieColor() => _color;

        public Button GetButton() => _button;

        public void Load(BinaryReader reader) => _color = ReadColor(reader);

        public void Save(BinaryWriter writer) => SaveColor(_color, writer);

        public static int LoadRace(SpriteFont font1, int heightmenu, Texture2D buttonMenu, string fileName,int r)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileName);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            Debug.Assert(xRoot != null , nameof( xRoot ) + " != null");
            foreach (XmlNode xnode in xRoot)
            {

                string name = "";
                int colorR = 255;
                int colorG = 255;
                int colorB = 255;
                // получаем атрибут name
                Debug.Assert(xnode.Attributes != null , "xnode.Attributes != null");
                if (xnode.Attributes.Count > 0)
                {
                    const string S = "name";
                    if (xnode.Attributes.GetNamedItem(S) != null)
                        name = xnode.Attributes.GetNamedItem(S)?.Value ?? "";
                }
                // обходим все дочерние узлы элемента user
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // если узел - company
                    switch (childnode.Name)
                    {
                        case "ColorR":
                            colorR = int.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                            break;
                        case "ColorG":
                            colorG = int.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                            break;
                        case "ColorB":
                            colorB = int.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                            break;
                        default:
                            WriteLine(childnode.Name);
                            break;
                    }
                    // если узел age
                }
                Rectangle rect = new Rectangle(120 + 130 * (r+Racelist.Count / 15),
                    heightmenu + 35 * (Racelist.Count % 15), (int)font1.MeasureString("Exit Game").X+50, 30);
                Racelist.Add(new Race(new Color(colorR, colorG, colorB), new Button(buttonMenu, font1, rect, name)));
            }
            return r;
        }

    }
}
