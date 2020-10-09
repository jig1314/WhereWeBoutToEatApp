using AppClient.Services;
using AppModels;
using Microsoft.AspNetCore.Components;
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

        public IEnumerable<Recipe> Recipes { get; set; }

        public string SearchText { get; set; }

        protected async Task OnSearchTextChanged(ChangeEventArgs eventArgs)
        {
            if (eventArgs.Value != null) 
            {
                SearchText = eventArgs.Value.ToString();
                Recipes = null;
                if (!string.IsNullOrWhiteSpace(eventArgs.Value.ToString()))
                {
                    Recipes = await RecipeService.SearchRecipes(SearchText);
                }
            }
        }

    }
}
