using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Client.Services
{
    public interface IUserService
    {
        Task<IEnumerable<RecipeTagType>> GetRecipeTagTypes();

        Task<IEnumerable<RecipeTag>> GetRecipeTags(int idRecipeTagType);
        Task<IEnumerable<AspNetUserRecipeTag>> GetUserRecipeTags(int idRecipeTagType);
        Task UpdateUserRecipeTags(int id, List<int> selectedRecipeTagIds);
    }
}
