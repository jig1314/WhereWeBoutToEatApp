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

        [HttpGet("{search}/{name}")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchRecipes(string name)
        {
            try
            {
                return Ok(await tastyRecipeRepository.SearchRecipes(name));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving recipes from Tasty");
            }
        }
    }
}
