using AppClient.Models;
using AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppClient.Services
{
    public interface IRecipeService
    {
        public Task<IEnumerable<Recipe>> SearchRecipes(string name);
        public Task<RecipeViewModel> GetRecipe(int id);
    }
}
