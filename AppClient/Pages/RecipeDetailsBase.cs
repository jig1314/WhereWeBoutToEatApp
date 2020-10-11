using AppClient.Services;
using AppModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppClient.Pages
{
    public class RecipeDetailsBase : ComponentBase
    {
        [Inject]
        public IRecipeService RecipeService { get; set; }

        [Parameter]
        public string Id { get; set; }

        public Recipe Recipe { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Recipe = await RecipeService.GetRecipe(int.Parse(Id));
        }
    }
}
