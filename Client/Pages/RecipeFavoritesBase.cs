using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Client.Services;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Client.Pages
{
    public class RecipeFavoritesBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public IRecipeService RecipeService { get; set; }

        public IEnumerable<Recipe> Recipes { get; set; }

        public AuthenticationState AuthenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState = await AuthenticationStateTask;

            Recipes = await RecipeService.GetUserFavorites();
        }

    }
}
