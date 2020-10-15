using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Client.Models;
using WhereWeBoutToEatApp.Client.Services;

namespace WhereWeBoutToEatApp.Client.Pages
{
    public class RecipeDetailsBase : ComponentBase
    {
        [Inject]
        public IRecipeService RecipeService { get; set; }

        [Parameter]
        public string Id { get; set; }

        public RecipeViewModel Recipe { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Recipe = await RecipeService.GetRecipe(int.Parse(Id));
        }
    }
}
