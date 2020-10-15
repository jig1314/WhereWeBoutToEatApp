using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Server.Repositories
{
    public interface ITastyRecipeRepository
    {
        Task<List<Recipe>> SearchRecipes(string name);
    }
}
