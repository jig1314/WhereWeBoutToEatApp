using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.WindowsRuntime;
using WhereWeBoutToEatApp.Server.Data;
using WhereWeBoutToEatApp.Shared;
using WhereWeBoutToEatApp.Server.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using WhereWeBoutToEatApp.Server.Models;
using System.Security.Claims;

namespace WhereWeBoutToEatApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITastyRecipeRepository tastyRecipeRepository;

        public RecipesController(ApplicationDbContext context, ITastyRecipeRepository tastyRecipeRepository)
        {
            _context = context;
            this.tastyRecipeRepository = tastyRecipeRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                
                var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);

                if (recipe == null)
                {
                    return NotFound();
                }

                return Ok(recipe);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving recipe");
            }
        }

        [HttpGet("{search}/{name}")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchRecipes(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest();
                }

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    await AddUserSearch(name);
                }

                var databaseResults = await SearchDatabaseAsync(name);
                if (databaseResults?.Count > 0)
                {
                    return Ok(databaseResults);
                }

                var tastyRecipes = await SearchTastyDatabaseAsync(name);
                await CommitRecipesToDatabase(tastyRecipes);

                databaseResults = await SearchDatabaseAsync(name);

                return Ok(databaseResults);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving recipes");
            }
        }

        private async Task<List<Recipe>> SearchDatabaseAsync(string name)
        {
            return await _context.Recipes.Where(recipe => recipe.Name.Contains(name)).ToListAsync();
        }

        private async Task<List<Recipe>> SearchTastyDatabaseAsync(string name)
        {
            return await tastyRecipeRepository.SearchRecipes(name);
        }

        private async Task CommitRecipesToDatabase(IEnumerable<Recipe> recipes)
        {
            await _context.Recipes.AddRangeAsync(recipes);
            await _context.SaveChangesAsync();
        }

        private async Task AddUserSearch(string name)
        {
            var idUser = GetUserId();
            var recipeSearchType = await _context.SearchTypes.FirstOrDefaultAsync(type => type.EnumCode == (int)Shared.Enums.Search.SearchType.Recipe);
            var userSearch = await _context.AspNetUserSearches.FirstOrDefaultAsync(search => search.IdSearchType == recipeSearchType.Id && search.IdUser == idUser && search.Search == name);

            if (userSearch == null)
            {
                userSearch = new AspNetUserSearch()
                {
                    IdUser = idUser,
                    Search = name,
                    IdSearchType = recipeSearchType.Id,
                    TimesSearched = 1
                };
                await _context.AspNetUserSearches.AddAsync(userSearch);
            }
            else
            {
                userSearch.TimesSearched++;
                _context.Entry(userSearch).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
