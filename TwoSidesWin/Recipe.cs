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
    }
}
