using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TwoSides.GameContent.GenerationResources;
using TwoSides.Utils;
using TwoSides.World;
using TwoSides.World.Tile;

namespace TwoSides.GUI
{

    public sealed class Dialog
    {

        //Переменные
        public bool IsClick;
        public int Type;

        //Листы
        public List<Recipe> Recipes = new List<Recipe>();


        //Массивы
        public bool[] IsBtnVisible = new bool[4];
        public Button[] Buttons = new Button[4];

        //Объекты

        public Image Background;
        public Label NormalText;
        public Label GoodText;
        public Label BadText;
        public Rectangle Area;

        [NonSerialized]
        public SpriteFont Font;

        public Dialog(Rectangle area, string positiveGood, string possitiveBad, string negativeGood, string negativeBad, 
            string normalText, string badText, string goodText,
            Texture2D imageButton, SpriteFont font, Texture2D background)
        {
            Area = area;
            Buttons[0] = new Button(imageButton, font, new Rectangle(area.X + 2,
                area.Y + area.Height - 60,
                (area.Width - 6) / 2,
                30), positiveGood);
            Buttons[1] = new Button(imageButton, font, new Rectangle(area.X + 2,
                area.Y + area.Height - 100,
                (area.Width - 6) / 2,
                30), possitiveBad);
            Buttons[2] = new Button(imageButton, font, new Rectangle(area.X + 2 + (area.Width - 6) / 2 + 5,
                area.Y + area.Height - 60,
                (area.Width - 6) / 2,
                30), negativeGood);
            Buttons[3] = new Button(imageButton, font, new Rectangle(area.X + 2 + (area.Width - 6) / 2 + 5,
                area.Y + area.Height - 100,
                (area.Width - 6) / 2,
                30), negativeBad);
            NormalText       = new Label(Program.Game.Player.Name + "," + normalText,       new Vector2(area.X + 2, area.Y + 2), font);
            GoodText   = new Label(Program.Game.Player.Name + "," + goodText,   new Vector2(area.X + 2, area.Y + 2), font);
            BadText    = new Label(Program.Game.Player.Name + "," + badText,    new Vector2(area.X + 2, area.Y + 2), font);
            Background = new Image(background, area);
            for (int i = 0; i < 4; i++)
            {
                if (Buttons[i].Text.Length > 0) IsBtnVisible[i] = true;
                else IsBtnVisible[i] = false;
            }
            Type = 0;
        }
        
        public Dialog(Rectangle area, string positiveGood, List<Recipe> recipe,
            string normalText, string badText, string goodText,
                      Texture2D imageButton, SpriteFont font, Texture2D background)
        {
            Area = area;
            Buttons[0] = new Button(imageButton, font, new Rectangle(area.X + area.Width / 2,
                area.Y + area.Height - 60,
                (area.Width - 6) / 2,
                30), positiveGood);
            if (Buttons[0].Text.Length > 0) IsBtnVisible[0] = true;
            else IsBtnVisible[0] = false;
            NormalText       = new Label(Program.Game.Player.Name + "," + normalText,     new Vector2(area.X + 2, area.Y + 2), font);
            GoodText   = new Label(Program.Game.Player.Name + "," + goodText, new Vector2(area.X + 2, area.Y + 2), font);
            BadText    = new Label(Program.Game.Player.Name + "," + badText,  new Vector2(area.X + 2, area.Y + 2), font);
            Background = new Image(background, area);
            Recipes = recipe;
            for (int i = 1; i < 4; i++)
            {
                Buttons[i] = null;
                IsBtnVisible[i] = false;
            }
            Type = 1;
            Font = font;
        }

        public bool GetValidRecipes(int id)
        {
            bool t = false;
            Recipe recipe = Recipes[id];
            for (int i = 0; i < recipe.GetSize(); i++)
            {
                int slotIds = Program.Game.Player.GetSlotItem(recipe.GetIngrident(i), recipe.Hp, recipe.GetSize(i));
                if (slotIds != -1)
                {
                    t = true;
                }
                else { t = false; break; }
            }

            if ( !t ) return false;

            if (!recipe.Isblock) return true;
            for (int xi = -3; xi <= 3; xi++)
            {
                for (int yi = -5; yi <= 5; yi++)
                {
                    int newx = (int)(Program.Game.Player.Position.X / Tile.TileMaxSize) + xi;
                    int newy = (int)(Program.Game.Player.Position.Y / Tile.TileMaxSize) + yi;
                    if (newx >= SizeGeneratior.WorldWidth ||
                        newx < 0) continue;
                    if (newy >= SizeGeneratior.WorldHeight ||
                        newy < 0) continue;
                    if (Program.Game.Dimension[Program.Game.CurrentDimension].MapTile[newx, newy].IdTexture == recipe.Idblock) return true;
                }
            }

            return false;
        }

        public bool IsVisible(int idButton) => IsBtnVisible[idButton];

        public bool IsBtnClicked(int idButton) => IsBtnVisible[idButton] && Buttons[idButton] != null && Buttons[idButton].IsClicked();

        public void RemoveRecipes(int id)
        {
            Recipe recipe = Recipes[id];
            for (int i = 0; i < recipe.GetSize(); i++)
            {

                int slotIds = Program.Game.Player.GetSlotItem(recipe.GetIngrident(i), recipe.Hp, recipe.GetSize(i));
                if ( slotIds == -1 ) continue;

                Program.Game.Player.Slot[slotIds].Ammount -= recipe.GetSize(i);

                if ( Program.Game.Player.Slot[slotIds].Ammount > 0 ) continue;

                Program.Game.Player.Slot[slotIds] = new Item {IsEmpty = true};
            }
        }

        public void SetTextButton(int idButton, string text) => Buttons[idButton].Text = text;

        public void Update()
        {
            if (Type == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (IsBtnVisible[i])
                    {
                        Buttons[i].Update();
                    }
                }
            }
            else
            {
                Buttons[0].Update();
                if (!IsClick && Program.Game.MouseState.LeftButton == ButtonState.Pressed )
                {
                    for ( int i = 0 ; i < Recipes.Count ; i++ )
                    {
                        if ( !GetValidRecipes(i) ) continue;

                        Rectangle rectrecip = new Rectangle(Area.X + 32 + i * 32 ,
                                                            Area.Y + Area.Height / 2 + 32 , 32 , 32);
                        if (!Tools.MouseInCube(rectrecip.X , rectrecip.Y , rectrecip.Width , rectrecip.Height))
                            continue;
                        IsClick = true;
                        RemoveRecipes(i);
                        Program.Game.Player.SetSlot(Recipes[i].Slot);
                        break;
                    }
                }

                else if (Program.Game.MouseState.LeftButton == ButtonState.Released) IsClick = false;
            }
        }

        public void ChangeVisible(int idButton,bool isActiveButton)
        {
            IsBtnVisible[idButton] = isActiveButton;
        }

        public void ChangeVisible(int idButton)
        {
            IsBtnVisible[idButton] = !IsBtnVisible[idButton];
        }

        public void Render(SpriteBatch spriteBatch)
        {
            if (Font == null) Font = Program.Game.Font1;
            Background.Draw(spriteBatch);
            if (Type == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (IsBtnVisible[i])
                    {
                        Buttons[i].Draw(spriteBatch);
                    }
                }
            }
            else
            {
                Buttons[0].Draw(spriteBatch);
                spriteBatch.Begin();
                for (int i = 0; i < Recipes.Count; i++)
                {
                    if ( !GetValidRecipes(i) ) continue;
                    
                    Rectangle rectrecip = new Rectangle(Area.X + 32 + i * 32, Area.Y + Area.Height / 2 + 32, 32, 32);
                    Rectangle reitems = rectrecip;
                    reitems.X += 16 - 8;
                    reitems.Y += 16 - 8;
                    reitems.Width = 16;
                    reitems.Height = 16;
                    spriteBatch.Draw(Program.Game.Inv, rectrecip, Color.Black);
                    Recipes[i].Slot.Render(spriteBatch, reitems);
                    for (int j = 0; j < Recipes[i].GetSize(); j++)
                    {
                        Rectangle rectrecip2 = new Rectangle(Area.X + 32 + i * 32, 
                            Area.Y + Area.Height / 2 + 32 * (j + 2), 32, 32);
                        Rectangle reitems2 = rectrecip2;
                        reitems2.X += 16 - 8;
                        reitems2.Y += 16 - 8;
                        reitems2.Width = 16;
                        reitems2.Height = 16;
                        spriteBatch.Draw(Program.Game.Inv, rectrecip2, Color.Green);
                        Item.Render(spriteBatch, Recipes[i].GetIngrident(j), reitems2);
                        reitems2.Y += 2;
                        reitems2.X = rectrecip2.X;
                        spriteBatch.DrawString(Font, Recipes[i].GetSize(j).ToString(CultureInfo.CurrentCulture), new Vector2(reitems2.X, reitems2.Y), Color.White);
                    }
                }
                spriteBatch.End();
            }
            if      (Program.Game.Player.Carma < 40)   BadText.Draw (   spriteBatch   );
            else if (Program.Game.Player.Carma > 60)   GoodText.Draw(   spriteBatch   );
            else                                NormalText.Draw    (   spriteBatch   );
        }
    }
}//TODO