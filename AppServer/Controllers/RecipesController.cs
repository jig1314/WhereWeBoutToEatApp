using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppModels;
using AppServer.DatabaseContexts;
using AppServer.Repositories;
using System.Runtime.InteropServices.WindowsRuntime;

namespace AppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITastyRecipeRepository tastyRecipeRepository;

        public RecipesController(AppDbContext context, ITastyRecipeRepository tastyRecipeRepository)
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

                var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.ID == id);

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
    }
}
