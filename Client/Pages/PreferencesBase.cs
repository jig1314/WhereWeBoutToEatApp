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
    public class PreferencesBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public List<RecipeTagType> RecipeTagTypes { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        public AuthenticationState AuthenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState = await AuthenticationStateTask;

            RecipeTagTypes = (await UserService.GetRecipeTagTypes()).ToList();
        }
    }
}
