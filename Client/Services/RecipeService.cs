using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Client.Models;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Client.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient httpClient;

        public RecipeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<RecipeViewModel> GetRecipe(int id)
        {
            var recipe = await httpClient.GetFromJsonAsync<Recipe>($"recipes/{id}");
            var recipeType = await httpClient.GetFromJsonAsync<RecipeType>($"recipeTypes/{recipe.IdRecipeType}");

            return new RecipeViewModel()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                ApiId = recipe.ApiId,
                IdRecipeType = recipe.IdRecipeType,
                ThumbnailURL = recipe.ThumbnailURL,
                UrlSuffix = recipe.UrlSuffix,
                VideoURL = recipe.VideoURL,
                RecipeType = recipeType
            };
        }

        public async Task<IEnumerable<Recipe>> SearchRecipes(string name) =>
            await httpClient.GetFromJsonAsync<IEnumerable<Recipe>>($"recipes/search/{name}");
    }
}
