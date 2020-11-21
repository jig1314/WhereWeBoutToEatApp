using System;
using System.Collections.Generic;
using System.Text;

namespace WhereWeBoutToEatApp.Shared
{
    public class RankedRecipes
    {
        public string EventId { get; set; }

        public long RecommendedRecipeId { get; set; }

        public List<Recipe> Recipes { get; set; }

        public string RecipeSearchCriteria { get; set; }
    }
}
