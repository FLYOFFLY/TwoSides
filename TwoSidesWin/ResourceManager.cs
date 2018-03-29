using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides
{
    public static class ResourceManager
    {
        static ContentManager _contentManager;
        static readonly Dictionary<String, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static void SetConentManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public static void PutInResourceManager(string name, string texturePath)
        {
            if (textures.ContainsKey(name)) return;

            textures.Add(name,_contentManager.Load<Texture2D>(texturePath));
        }

        public static Texture2D GetTexture2D(string name) => textures[name];
    }
}
