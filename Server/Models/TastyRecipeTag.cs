using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereWeBoutToEatApp.Server.Models
{
    public class TastyRecipeTag
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string RecipeTagType { get; set; }
    }

    public class TastyRecipeTagComparer : EqualityComparer<TastyRecipeTag>
    {
        public override bool Equals(TastyRecipeTag x, TastyRecipeTag y)
        {
            return x.Name.ToUpper() == y.Name.ToUpper() && x.DisplayName.ToUpper() == y.DisplayName.ToUpper() && x.RecipeTagType.ToUpper() == y.RecipeTagType.ToUpper();
        }

        public override int GetHashCode(TastyRecipeTag tastyRecipeTag)
        {
            int hash = tastyRecipeTag.Name.ToUpper().GetHashCode() + tastyRecipeTag.DisplayName.ToUpper().GetHashCode() + tastyRecipeTag.RecipeTagType.ToUpper().GetHashCode();
            return hash;
        }
    }
}
