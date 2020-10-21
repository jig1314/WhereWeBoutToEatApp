using System;
using System.Collections.Generic;
using System.Text;

namespace WhereWeBoutToEatApp.Shared
{
    public class Recipe_RecipeTag
    {
        public long Id { get; set; }

        public long IdRecipe { get; set; }

        public int IdRecipeTag { get; set; }
    }
}
