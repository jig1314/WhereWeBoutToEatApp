using WhereWeBoutToEatApp.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<RecipeType> RecipeTypes { get; set; }

        public DbSet<AppSetting> AppSettings { get; set; }

        public DbSet<AspNetUserSearch> AspNetUserSearches { get; set; }

        public DbSet<SearchType> SearchTypes { get; set; }

        public DbSet<AspNetUserRecipe> AspNetUserRecipes { get; set; }

        public DbSet<RecipeTag> RecipeTags { get; set; }

        public DbSet<RecipeTagType> RecipeTagTypes { get; set; }

        public DbSet<Recipe_RecipeTag> Recipe_RecipeTags { get; set; }

    }
}
