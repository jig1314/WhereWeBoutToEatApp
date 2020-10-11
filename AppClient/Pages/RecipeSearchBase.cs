using AppClient.Services;
using AppModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AppClient.Pages
{
    public class RecipeSearchBase : ComponentBase
    {
        [Inject]
        public IRecipeService RecipeService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<Recipe> Recipes { get; set; }

        [Parameter]
        public string SearchText { get; set; }

        public string LocalSearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
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
            Recipes = await RecipeService.SearchRecipes(SearchText);
        }
    }
}
