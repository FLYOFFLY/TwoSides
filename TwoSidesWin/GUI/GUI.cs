using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using TwoSides.GUI;

namespace TwoSides.GUI
{
    public sealed class XnaLayout
    {

        [NonSerialized] readonly List<Button> _buttons = new List<Button>();

        [NonSerialized] readonly List<TextField> _fields = new List<TextField>();

        [NonSerialized] readonly List<Label> _labels = new List<Label>();

        [NonSerialized] readonly List<Image> _images = new List<Image>();
        
        public int AddButon(Button button)
        {
            _buttons.Add(button);
            return _buttons.Count - 1;
        }

        public int AddImage(Image image)
        {
            _images.Add(image);
            return _images.Count - 1;
        }
        
        public int AddInput(TextField field)
        {
            _fields.Add(field);
            return _fields.Count - 1;
        }

        public int AddLabel(Label label)
        {
            _labels.Add(label);
            return _labels.Count - 1;
        }
        
        public Label GetLabel(int labelId) => _labels[labelId];

        public void AddClicked(EventHandler<EventArgs> func, int buttonId)
        {
            _buttons[buttonId].OnClicked += func;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach ( Image image in _images )
                image.Draw(spriteBatch);
            foreach ( Button button in _buttons )
                button.Draw(spriteBatch);
            foreach ( TextField input in _fields )
                input.Draw(spriteBatch);
            foreach ( Label label in _labels )
                label.Draw(spriteBatch);
        }

        public void Update()
        {
            foreach ( Button button in _buttons )
                button.Update();
            foreach ( TextField input in _fields )
                input.UpdateKey();
            foreach ( Label label in _labels )
                label.Update();
        }
    }
}
