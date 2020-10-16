using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Client.Models;
using WhereWeBoutToEatApp.Client.Services;

namespace WhereWeBoutToEatApp.Client.Pages
{
    public class RecipeDetailsBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IRecipeService RecipeService { get; set; }

        [Parameter]
        public string Id { get; set; }

        public RecipeViewModel Recipe { get; set; }

        public AuthenticationState AuthenticationState { get; set; }

        public string FavoriteState { get; set; }

        protected void OnFavoriteStateChanged(bool isFavorite)
        {
            if (isFavorite)
            {
                FavoriteState = "Favorited";
            }
            else
            {
                FavoriteState = "Favorite";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState = await AuthenticationStateTask;

            Recipe = await RecipeService.GetRecipe(int.Parse(Id));

            OnFavoriteStateChanged((Recipe.UserRecipe != null) && Recipe.UserRecipe.IsFavorite);
        }

        protected async Task FavoriteButtonClicked()
        {
            if (!AuthenticationState.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"/authentication/login?returnUrl={WebUtility.UrlEncode(NavigationManager.Uri)}");
            }
            else
            {
                Recipe.UserRecipe.IsFavorite = !Recipe.UserRecipe.IsFavorite;
                await RecipeService.UpdateUserRecipe(Recipe.UserRecipe.Id, Recipe.UserRecipe);
                OnFavoriteStateChanged(Recipe.UserRecipe.IsFavorite);
            }
        }
    }
}
