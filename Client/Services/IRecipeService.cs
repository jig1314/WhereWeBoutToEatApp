using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Client.Models;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Client.Services
{
    public interface IRecipeService
    {
        public Task<IEnumerable<Recipe>> SearchRecipes(string name);
        public Task<RecipeViewModel> GetRecipe(int id);
        Task UpdateUserRecipe(long id, AspNetUserRecipe newUserRecipe);
        Task<RankedRecipes> GetRecommendations();
        Task RecommendationSelected(string eventId);
    }
}
