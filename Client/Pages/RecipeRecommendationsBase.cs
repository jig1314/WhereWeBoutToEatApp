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
    public class RecipeRecommendationsBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IRecipeService RecipeService { get; set; }

        public IEnumerable<Recipe> Recipes { get; set; }

        public RankedRecipes RankedRecipes { get; set; }

        public bool HasSearched { get; private set; } = false;

        public AuthenticationState AuthenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState = await AuthenticationStateTask;
            RankedRecipes = await RecipeService.GetRecommendations();
            Recipes = RankedRecipes?.Recipes;
            HasSearched = true;
        }

        protected async Task OpenRecipeAsync(long idRecipe)
        {
            if (idRecipe == RankedRecipes.RecommendedRecipeId)
            {
                await RecipeService.RecommendationSelected(RankedRecipes.EventId);
            }
            NavigationManager.NavigateTo($"/recipeDetails/{idRecipe}");
        }
    }
}
