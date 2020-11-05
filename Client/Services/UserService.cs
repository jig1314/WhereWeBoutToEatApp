using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<RecipeTag>> GetRecipeTags(int idRecipeTagType) =>
            await httpClient.GetFromJsonAsync<IEnumerable<RecipeTag>>($"recipeTags/{idRecipeTagType}");

        public async Task<IEnumerable<RecipeTagType>> GetRecipeTagTypes() =>
            await httpClient.GetFromJsonAsync<IEnumerable<RecipeTagType>>("recipeTagTypes");

        public async Task<IEnumerable<AspNetUserRecipeTag>> GetUserRecipeTags(int idRecipeTagType) =>
            await httpClient.GetFromJsonAsync<IEnumerable<AspNetUserRecipeTag>>($"userRecipeTags/{idRecipeTagType}");

        public async Task UpdateUserRecipeTags(int idRecipeTagType, List<int> selectedRecipeTagIds)
        {
            var response = await httpClient.PutAsJsonAsync($"userRecipeTags/{idRecipeTagType}", selectedRecipeTagIds);
            response.EnsureSuccessStatusCode();
        }
    }
}
