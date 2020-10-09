using AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AppClient.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient httpClient;

        public RecipeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Recipe>> SearchRecipes(string name) =>
            await httpClient.GetFromJsonAsync<IEnumerable<Recipe>>($"api/recipes/search/{name}");
    }
}
