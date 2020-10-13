using AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class RecipeViewModel
    {
        public long Id { get; set; }

        public int? ApiId { get; set; }

        public string Name { get; set; }

        public string ThumbnailURL { get; set; }

        public string VideoURL { get; set; }

        public string UrlSuffix { get; set; }

        public int IdRecipeType { get; set; }

        public RecipeType RecipeType { get; set; }

        public string URL => $"{RecipeType.BaseURL}{UrlSuffix}";
    }
}
