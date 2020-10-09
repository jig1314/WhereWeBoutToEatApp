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
    }
}
