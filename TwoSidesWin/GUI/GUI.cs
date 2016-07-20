using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides.GUI
{
    sealed public class GUI
    {

        [NonSerialized]
        ArrayList arl_Buttons = new ArrayList();

        [NonSerialized]
        ArrayList arl_Inputs = new ArrayList();

        [NonSerialized]
        ArrayList arl_Labels = new ArrayList();

        [NonSerialized]
        ArrayList arl_Images = new ArrayList();

        /// <summary>
        /// Проверяет нажатие кнопки
        /// <param name="btnid">Ид кнопки</param>
        /// </summary>
        public bool IsButtonClicked(int L_buttonID)
        {

            return ((Button)this.arl_Buttons[L_buttonID]).IsClicked();
        }

        public int AddButon(Button L_button)
        {
            this.arl_Buttons.Add(L_button);
            return this.arl_Buttons.Count - 1;
        }

        public int AddImage(Image L_image)
        {
            this.arl_Images.Add(L_image);
            return this.arl_Images.Count - 1;
        }
        
        public int AddInput(TextField L_input)
        {
            this.arl_Inputs.Add(L_input);
            return this.arl_Inputs.Count - 1;
        }

        public int AddLabel(Label L_label)
        {
            this.arl_Labels.Add(L_label);
            return this.arl_Labels.Count - 1;
        }
        
        public Label GetLabel(int L_id)
        {
            return (Label)this.arl_Labels[L_id];
        }

        public void AddClicked(Button.onClicked L_func, int L_buttonID)
        {
            ((Button)this.arl_Buttons[L_buttonID]).e_onClicked += L_func;
        }

        public void Draw(SpriteBatch L_spriteBatch)
        {
            if (this.arl_Images.Count > 0) for (int i = 0; i < this.arl_Images.Count; i++) ((Image)this.arl_Images[i]).Draw(L_spriteBatch);
            if (this.arl_Buttons.Count > 0) for (int i = 0; i < this.arl_Buttons.Count; i++) ((Button)this.arl_Buttons[i]).Draw(L_spriteBatch);
            if (this.arl_Inputs.Count > 0) for (int i = 0; i < this.arl_Inputs.Count; i++) ((TextField)this.arl_Inputs[i]).Draw(L_spriteBatch);
            if (this.arl_Labels.Count > 0) for (int i = 0; i < this.arl_Labels.Count; i++) ((Label)this.arl_Labels[i]).Draw(L_spriteBatch);
        }

        public void Update()
        {
            if (this.arl_Buttons.Count > 0) for (int i = 0; i < this.arl_Buttons.Count; i++) ((Button)this.arl_Buttons[i]).Update();
            if (this.arl_Inputs.Count > 0) for (int i = 0; i < this.arl_Inputs.Count; i++) ((TextField)this.arl_Inputs[i]).updateKey();
            if (this.arl_Labels.Count > 0) for (int i = 0; i < this.arl_Labels.Count; i++) ((Label)this.arl_Labels[i]).Update();
        }
    }
}
