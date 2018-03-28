using System;
using System.Collections.Generic;

namespace TwoSides.GUI
{
    public sealed class XnaLayout
    {

        [NonSerialized] readonly List<GuiElement> _guielements = new List<GuiElement>();
        public int AddElement(GuiElement guiElement) {
            _guielements.Add(guiElement);
            return _guielements.Count - 1;
        }
        
        public T GetElement<T>(int elementId) where T:GuiElement => _guielements[elementId] as T;

 
        public void Draw(Render render)
        {
            foreach ( GuiElement element in _guielements)
                element.Draw(render);
        }

        public void Update()
        {
            foreach (GuiElement element in _guielements)
                element.Update();
        }
    }
}
