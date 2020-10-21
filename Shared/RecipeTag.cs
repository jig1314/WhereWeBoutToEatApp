using System;
using System.Collections.Generic;
using System.Text;

namespace WhereWeBoutToEatApp.Shared
{
    public class RecipeTag
    {
        public int Id { get; set; }

        public int? EnumCode { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public int IdRecipeTagType { get; set; }

    }
}
