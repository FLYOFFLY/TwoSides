using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwoSides.World;
using TwoSides.GameContent.GenerationResources;
using TwoSides.World.Tile;
using TwoSides.Utils;
namespace TwoSides.GUI
{
    sealed public class Dialog
    {

        //Переменные
        public bool b_IsDown;
        public int i32_Type;

        //Листы
        public ArrayList arl_Recipes = new ArrayList();

        //Массивы
        public bool[] arb_IsBtnVisible = new bool[4];
        public Button[] arbtn_Say = new Button[4];

        //Объекты

        public Image img_Background;
        public Label lab_Text;
        public Label lab_GoodText;
        public Label lab_BadText;
        public Rectangle rect_Dialog;

        [NonSerialized]
        public SpriteFont font_Font1;

        public Dialog(Rectangle L_rect, string L_sayText1, string L_sayText2, string L_sayText3, string L_sayText4, string L_text, string L_badText, string L_goodText,
            Texture2D L_btnTexture, SpriteFont L_font, Texture2D L_background)
        {
            this.rect_Dialog = L_rect;
            this.arbtn_Say[0] = new Button(L_btnTexture, L_font, new Rectangle(L_rect.X + 2,
                L_rect.Y + L_rect.Height - 60,
                (L_rect.Width - 6) / 2,
                30), L_sayText1);
            this.arbtn_Say[1] = new Button(L_btnTexture, L_font, new Rectangle(L_rect.X + 2,
                L_rect.Y + L_rect.Height - 100,
                (L_rect.Width - 6) / 2,
                30), L_sayText2);
            this.arbtn_Say[2] = new Button(L_btnTexture, L_font, new Rectangle((L_rect.X + 2) + ((L_rect.Width - 6) / 2) + 5,
                L_rect.Y + L_rect.Height - 60,
                (L_rect.Width - 6) / 2,
                30), L_sayText3);
            this.arbtn_Say[3] = new Button(L_btnTexture, L_font, new Rectangle((L_rect.X + 2) + ((L_rect.Width - 6) / 2) + 5,
                L_rect.Y + L_rect.Height - 100,
                (L_rect.Width - 6) / 2,
                30), L_sayText4);
            this.lab_Text       = new Label(Program.game.player.Name + "," + L_text,       new Vector2(L_rect.X + 2, L_rect.Y + 2), L_font);
            this.lab_GoodText   = new Label(Program.game.player.Name + "," + L_goodText,   new Vector2(L_rect.X + 2, L_rect.Y + 2), L_font);
            this.lab_BadText    = new Label(Program.game.player.Name + "," + L_badText,    new Vector2(L_rect.X + 2, L_rect.Y + 2), L_font);
            this.img_Background = new Image(L_background, L_rect);
            for (int i = 0; i < 4; i++)
            {
                if (this.arbtn_Say[i].Text.Count() > 0) this.arb_IsBtnVisible[i] = true;
                else this.arb_IsBtnVisible[i] = false;
            }
            this.i32_Type = 0;
        }
        
        public Dialog(Rectangle L_rect, string L_sayText1, ArrayList L_recipes, string L_text, string L_badText, string L_goodText,
            Texture2D L_btnTexture, SpriteFont L_font, Texture2D L_background)
        {
            this.rect_Dialog = L_rect;
            this.arbtn_Say[0] = new Button(L_btnTexture, L_font, new Rectangle(L_rect.X + L_rect.Width / 2,
                L_rect.Y + L_rect.Height - 60,
                (L_rect.Width - 6) / 2,
                30), L_sayText1);
            if (arbtn_Say[0].Text.Count() > 0) arb_IsBtnVisible[0] = true;
            else arb_IsBtnVisible[0] = false;
            this.lab_Text       = new Label(Program.game.player.Name + "," + L_text,     new Vector2(L_rect.X + 2, L_rect.Y + 2), L_font);
            this.lab_GoodText   = new Label(Program.game.player.Name + "," + L_goodText, new Vector2(L_rect.X + 2, L_rect.Y + 2), L_font);
            this.lab_BadText    = new Label(Program.game.player.Name + "," + L_badText,  new Vector2(L_rect.X + 2, L_rect.Y + 2), L_font);
            this.img_Background = new Image(L_background, L_rect);
            this.arl_Recipes = L_recipes;
            for (int i = 1; i < 4; i++)
            {
                this.arbtn_Say[i] = null;
                this.arb_IsBtnVisible[i] = false;
            }
            this.i32_Type = 1;
            this.font_Font1 = L_font;
        }

        public bool GetValidRecipes(int L_id)
        {
            bool t = false;
            Recipe recipe = (Recipe)this.arl_Recipes[L_id];
            for (int i = 0; i < recipe.getsize(); i++)
            {
                int slotids = Program.game.player.getslotitem(recipe.getigr(i), recipe.hp, recipe.getconut(i));
                if (slotids != -1)
                {
                    t = true;
                }
                else { t = false; break; }
            }
            if (t)
            {
                if (!recipe.isblock) return true;
                for (int xi = -3; xi <= 3; xi++)
                {
                    for (int yi = -5; yi <= 5; yi++)
                    {
                        int newx = (int)(Program.game.player.position.X / ITile.TileMaxSize) + xi;
                        int newy = (int)(Program.game.player.position.Y / ITile.TileMaxSize) + yi;
                        if (newx >= SizeGeneratior.WorldWidth ||
                            newx < 0) continue;
                        if (newy >= SizeGeneratior.WorldHeight ||
                            newy < 0) continue;
                        if (Program.game.dimension[Program.game.currentD].map[newx, newy].idtexture == recipe.idblock) return true;
                    }
                }
            }
            return false;
        }

        public bool IsVisible(int L_btnID)
        {
            return this.arb_IsBtnVisible[L_btnID];
        }

        public bool IsBtnClicked(int L_btnID)
        {

            if (this.arb_IsBtnVisible[L_btnID] && this.arbtn_Say[L_btnID] != null)
            {
                return this.arbtn_Say[L_btnID].IsClicked();
            }
            else return false;
        }

        public void RemoveRecipes(int L_id)
        {
            Recipe recipe = (Recipe)this.arl_Recipes[L_id];
            for (int i = 0; i < recipe.getsize(); i++)
            {

                int slotids = Program.game.player.getslotitem(recipe.getigr(i), recipe.hp, recipe.getconut(i));
                if (slotids != -1)
                {

                    Program.game.player.slot[slotids].ammount -= recipe.getconut(i);

                    if (Program.game.player.slot[slotids].ammount <= 0)
                    {
                        Program.game.player.slot[slotids] = new Item();
                        Program.game.player.slot[slotids].IsEmpty = true;
                    }

                }
            }
        }
        
        public void SetTextButton(int L_id, string L_text)
        {
            this.arbtn_Say[L_id].Text = L_text;
        }
        
        public void Update()
        {
            if (this.i32_Type == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (this.arb_IsBtnVisible[i])
                    {
                        this.arbtn_Say[i].Update();
                    }
                }
            }
            else
            {
                this.arbtn_Say[0].Update();

                for (int i = 0; i < this.arl_Recipes.Count; i++)
                {
                    if (GetValidRecipes(i))
                    {
                        Recipe recip = (Recipe)this.arl_Recipes[i];
                        Rectangle rectrecip = new Rectangle(this.rect_Dialog.X + 32 + i * 32, this.rect_Dialog.Y + rect_Dialog.Height / 2 + 32, 32, 32);
                        if (Util.MouseInCube(rectrecip.X, rectrecip.Y, rectrecip.Width, rectrecip.Height))
                        {
                            if (Program.game.mouseState.LeftButton == ButtonState.Pressed && !b_IsDown)
                            {
                                this.b_IsDown = true;
                                RemoveRecipes(i);
                                Program.game.player.setslot(recip.slot);
                            }
                        }
                    }
                }
                if (Program.game.mouseState.LeftButton == ButtonState.Released) this.b_IsDown = false;
            }
        }

        public void ChangeVisible(int L_btn,bool L_isOn)
        {
            this.arb_IsBtnVisible[L_btn] = L_isOn;
        }

        public void ChangeVisible(int L_btn)
        {
            this.arb_IsBtnVisible[L_btn] = !this.arb_IsBtnVisible[L_btn];
        }

        public void render(SpriteBatch L_spriteBatch)
        {
            if (this.font_Font1 == null) this.font_Font1 = Program.game.Font1;
            this.img_Background.Draw(L_spriteBatch);
            if (this.i32_Type == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (this.arb_IsBtnVisible[i])
                    {
                        this.arbtn_Say[i].Draw(L_spriteBatch);
                    }
                }
            }
            else
            {
                this.arbtn_Say[0].Draw(L_spriteBatch);
                L_spriteBatch.Begin();
                for (int i = 0; i < this.arl_Recipes.Count; i++)
                {
                    if (GetValidRecipes(i))
                    {
                        Recipe recip = (Recipe)this.arl_Recipes[i];
                        Rectangle rectrecip = new Rectangle(this.rect_Dialog.X + 32 + i * 32, this.rect_Dialog.Y + this.rect_Dialog.Height / 2 + 32, 32, 32);
                        Rectangle reitems = rectrecip;
                        reitems.X += 16 - 8;
                        reitems.Y += 16 - 8;
                        reitems.Width = 16;
                        reitems.Height = 16;
                        L_spriteBatch.Draw(Program.game.inv, rectrecip, Color.Black);
                        L_spriteBatch.Draw(Item.items[recip.slot.iditem], reitems, Color.White);
                        for (int j = 0; j < recip.getsize(); j++)
                        {
                            Rectangle rectrecip2 = new Rectangle(this.rect_Dialog.X + 32 + i * 32, this.rect_Dialog.Y + this.rect_Dialog.Height / 2 + 32 * (j + 2), 32, 32);
                            Rectangle reitems2 = rectrecip2;
                            reitems2.X += 16 - 8;
                            reitems2.Y += 16 - 8;
                            reitems2.Width = 16;
                            reitems2.Height = 16;
                            L_spriteBatch.Draw(Program.game.inv, rectrecip2, Color.Green);
                            L_spriteBatch.Draw(Item.items[recip.getigr(j)], reitems2, Color.White);
                            reitems2.Y += 2;
                            reitems2.X = rectrecip2.X;
                            L_spriteBatch.DrawString(font_Font1, recip.getconut(j).ToString(), new Vector2(reitems2.X, reitems2.Y), Color.White);
                        }
                    }
                }
                L_spriteBatch.End();
            }
            if      (Program.game.player.carma < 40)   this.lab_BadText.Draw (   L_spriteBatch   );
            else if (Program.game.player.carma > 60)   this.lab_GoodText.Draw(   L_spriteBatch   );
            else                                this.lab_Text.Draw    (   L_spriteBatch   );
        }
    }
}//TODO