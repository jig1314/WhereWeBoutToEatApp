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
}
