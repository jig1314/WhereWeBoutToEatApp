using AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServer.Repositories
{
    public interface ITastyRecipeRepository
    {
        Task<List<Recipe>> SearchRecipes(string name);
    }
}
