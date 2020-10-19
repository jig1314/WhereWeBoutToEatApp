using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Client.Services;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Client.Pages
{
    public class RecipeSearchBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public IRecipeService RecipeService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<Recipe> Recipes { get; set; }

        [Parameter]
        public string SearchText { get; set; }
        
        public AuthenticationState AuthenticationState { get; set; }

        public string LocalSearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState = await AuthenticationStateTask;

            LocalSearchText = SearchText;
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                await SearchForRecipes();
            }
        }

        protected void OnSearchTextChanged(ChangeEventArgs eventArgs)
        {
            if (eventArgs.Value != null && eventArgs.Value.ToString() != SearchText)
            {
                NavigationManager.NavigateTo($"/recipeSearch/{LocalSearchText}", true);
            }
        }

        private async Task SearchForRecipes()
        {
            if (!AuthenticationState.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"/authentication/login?returnUrl={WebUtility.UrlEncode(NavigationManager.Uri)}");
            }
            else
            {
                Recipes = await RecipeService.SearchRecipes(SearchText);
            }
        }
    }
}
