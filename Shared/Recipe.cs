using System;
using System.Collections.Generic;

namespace WhereWeBoutToEatApp.Shared
{
    public class Recipe
    {
        public long Id { get; set; }

        public int? ApiId { get; set; }

        public string Name { get; set; }

        public string ThumbnailURL { get; set; }

        public string VideoURL { get; set; }

        public string UrlSuffix { get; set; }

        public int IdRecipeType { get; set; }
    }

    public class RecipeComparer : EqualityComparer<Recipe>
    {
        public override bool Equals(Recipe x, Recipe y)
        {
            return x.ApiId == y.ApiId && x.IdRecipeType == y.IdRecipeType;
        }

        public override int GetHashCode(Recipe recipe)
        {
            int hash = recipe.ApiId.HasValue ? recipe.ApiId.Value ^ recipe.IdRecipeType : new Random().Next() ^ recipe.IdRecipeType;
            return hash.GetHashCode();
        }
    }
}
