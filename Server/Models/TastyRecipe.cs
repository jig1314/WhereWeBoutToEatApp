using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Server.Models
{
    public class TastyRecipe
    {
        public Recipe Recipe { get; set; }

        public List<TastyRecipeTag> TastyRecipeTags { get; set; }
    }

    public class TastyRecipeComparer : EqualityComparer<TastyRecipe>
    {
        public override bool Equals(TastyRecipe x, TastyRecipe y)
        {
            return x.Recipe.ApiId == y.Recipe.ApiId && x.Recipe.IdRecipeType == y.Recipe.IdRecipeType;
        }

        public override int GetHashCode(TastyRecipe tastyRecipe)
        {
            int hash = tastyRecipe.Recipe.ApiId.HasValue ? tastyRecipe.Recipe.ApiId.Value ^ tastyRecipe.Recipe.IdRecipeType : new Random().Next() ^ tastyRecipe.Recipe.IdRecipeType;
            return hash.GetHashCode();
        }
    }
}
