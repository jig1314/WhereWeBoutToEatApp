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
    public class RecipeTagsBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public IEnumerable<RecipeTag> RecipeTags { get; set; }

        public List<string> SelectedRecipeTagIds { get; set; }

        public IEnumerable<AspNetUserRecipeTag> UserRecipeTags { get; set; }

        [Parameter]
        public RecipeTagType RecipeTagType { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        public AuthenticationState AuthenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState = await AuthenticationStateTask;
            await RefreshData();
        }

        protected async Task SaveRecipeTagsAsync()
        {
            RecipeTags = null;
            UserRecipeTags = null;

            await UserService.UpdateUserRecipeTags(RecipeTagType.Id, SelectedRecipeTagIds.ConvertAll((item) => Convert.ToInt32(item)));
            await RefreshData();
        }

        private async Task RefreshData()
        {
            var taskRecipeTags = UserService.GetRecipeTags(RecipeTagType.Id);
            var taskUserRecipeTags = UserService.GetUserRecipeTags(RecipeTagType.Id);

            RecipeTags = await taskRecipeTags;

            var userRecipeTags = await taskUserRecipeTags;
            SelectedRecipeTagIds = userRecipeTags.Select(tag => tag.IdRecipeTag.ToString()).ToList();

            UserRecipeTags = userRecipeTags;
        }

    }
}
