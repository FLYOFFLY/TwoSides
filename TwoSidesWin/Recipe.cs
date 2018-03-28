using System.Collections.Generic;

using TwoSides.World;

namespace TwoSides
{
    public sealed class Recipe
    {
        public static List<Recipe> Recipes = new List<Recipe>();
        public bool Isblock { get; }
        public int Idblock { get; }
        public float Hp { get; }

        readonly List<int> _ingridents = new List<int>();
        readonly List<int> _count = new List<int>();

        public Item Slot { get; }
        public static void AddRecipe(Recipe recipe,Item[] igr)
        {
            foreach ( Item item in igr )
            {
                recipe.AddIngridents(item.Id, item.Ammount);
            }

            Recipes.Add(recipe);
        }
        public Recipe(Item slot,int id, float hp )
        {
            Slot = slot;
            Idblock = id;
            Isblock = true;
            Hp = hp;
        }
        
        public Recipe(Item slot, float hp)
        {
            Slot = slot;
            Isblock = false;
            Hp = hp;
        }
        
        public int GetSize() => _count.Count;

        public int GetIngrident(int id) => _ingridents[id];

        public int GetSize(int id) => _count[id];

        public void AddIngridents(int ingridents, int count)
        {
            _ingridents.Add(ingridents);
            _count.Add(count);
        }

        public static void LoadRecipe()
        {
            Program.StartSw();
            Recipe test = new Recipe(new Item(1, 6), 100.0f);

            Recipe.AddRecipe(test, new[] { new Item(2, 7),new Item(1,5) });
            
            Recipe.AddRecipe(new Recipe(new Item( 5,    19),    20.0f),new[] {new Item(1,5)});
            Recipe.AddRecipe(new Recipe(new Item(1,    8),     20.0f), new[] { new Item(2, 19), new Item(3, 1) });
            Recipe.AddRecipe(new Recipe(new Item(1,22),   20.0f), new[] { new Item(3,1) });
            Recipe.AddRecipe(new Recipe(new Item(1,23),    20.0f), new[] { new Item(2, 1) });
            Recipe.AddRecipe(new Recipe(new Item(1, 24), 20.0f), new[] { new Item(3, 1) });
            //NEW UPDATE
            Recipe.AddRecipe(new Recipe(new Item(1, 26), 20.0f),new[] { new Item(6, 19), new Item(6, 5) });
            Recipe.AddRecipe(new Recipe(new Item(1, 27), 20.0f),new[] { new Item(4, 19), new Item(3, 5) });
            Recipe.AddRecipe(new Recipe(new Item(1, 28), 20.0f), new[] { new Item(1,10) });
            Recipe.AddRecipe(new Recipe(new Item(1, 29),22, 20.0f),new[] { new Item(4,3)});
            Recipe.AddRecipe(new Recipe(new Item(1, 30),22, 20.0f),new[] {new Item(4,2) });
            Recipe.AddRecipe(new Recipe(new Item(1, 31), 20.0f), new[] { new Item(5, 29) });
            Recipe.AddRecipe(new Recipe(new Item(1, 32), 20.0f), new[] { new Item(4, 29) });
            Recipe.AddRecipe(new Recipe(new Item(1, 33), 20.0f), new[] { new Item(5, 29) });
            Recipe.AddRecipe(new Recipe(new Item(1, 34), 20.0f), new[] { new Item(1, 29), new Item(2, 19) }); // sword
            Recipe.AddRecipe(new Recipe(new Item(1, 38),22, 20.0f), new[] { new Item(4, 36) });
            Recipe.AddRecipe(new Recipe(new Item(1, 43), 20.0f), new[] { new Item(6, 1), new Item(1, 19) }); // hammer
            Recipe.AddRecipe(new Recipe(new Item(1, 41), 22, 20.0f), new[] { new Item(3, 29), new Item(1, 10) }); // pickaxe
            Recipe.AddRecipe(new Recipe(new Item(1, 44), 22, 20.0f), new[] { new Item(1, 2), new Item(2, 19) });
            Recipe.AddRecipe(new Recipe(new Item(1, 39), 20.0f), new[] { new Item(4, 38) });
            Recipe.AddRecipe(new Recipe(new Item(1, 45), 22, 20.0f), new[] { new Item(1, 1), new Item(2, 19) });
            Recipe.AddRecipe(new Recipe(new Item(4, 47), 20.0f), new[] { new Item(1, 46) });
            Recipe.AddRecipe(new Recipe(new Item(1, 48), 20.0f), new[] { new Item(1, 46), new Item(1, 49) });

            Recipe.AddRecipe(new Recipe(new Item(1, 49), 20.0f), new[] { new Item(1, 19), new Item(1, 47) });
            //EAT
            Recipe.AddRecipe(new Recipe(new Item(1, 52), 19, 20.0f), new[] { new Item(4, 51) });
            //EAT
            Recipe.AddRecipe(new Recipe(new Item(1, 53), 22, 20.0f), new[] { new Item(1, 52), new Item(1, 10) });

            Program.StopSw("Created Recipe");
        }
    }
}
