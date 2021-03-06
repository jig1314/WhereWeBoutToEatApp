﻿using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
            var userRecipe = await httpClient.GetFromJsonAsync<AspNetUserRecipe>($"userrecipes/{id}");

            return new RecipeViewModel()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                ApiId = recipe.ApiId,
                IdRecipeType = recipe.IdRecipeType,
                ThumbnailURL = recipe.ThumbnailURL,
                UrlSuffix = recipe.UrlSuffix,
                VideoURL = recipe.VideoURL,
                RecipeType = recipeType,
                UserRecipe = userRecipe
            };
        }

        public async Task<RankedRecipes> GetRecommendations() =>
            await httpClient.GetFromJsonAsync<RankedRecipes>($"recipes/recommendations");

        public async Task<IEnumerable<Recipe>> GetUserFavorites() =>
            await httpClient.GetFromJsonAsync<IEnumerable<Recipe>>($"recipes/favorites");

        public async Task RecommendationSelected(string eventId)
        {
            await httpClient.PutAsJsonAsync($"recipes/recommendationSelected", eventId);
        }

        public async Task<IEnumerable<Recipe>> SearchRecipes(string name) =>
            await httpClient.GetFromJsonAsync<IEnumerable<Recipe>>($"recipes/search/{name}");

        public async Task UpdateUserRecipe(long idUserRecipe, AspNetUserRecipe newUserRecipe)
        {
            var response = await httpClient.PutAsJsonAsync($"userrecipes/{idUserRecipe}", newUserRecipe);
            response.EnsureSuccessStatusCode();
        }
    }
}
