using System;
using System.Collections.Generic;
using System.Text;

namespace WhereWeBoutToEatApp.Shared.Enums
{
    public class Recipe
    {
        public enum RecipeType
        {
            Tasty = 1
        }

        public enum RecipeTagType
        {
            Cuisine = 1,
            Dietary = 2,
            Occasion = 3,
            Meal = 4,
            Seasonal = 5,
            Holiday = 6
        }
    }
}
