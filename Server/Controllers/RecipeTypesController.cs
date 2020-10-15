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

namespace WhereWeBoutToEatApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipeTypesController(ApplicationDbContext context, ITastyRecipeRepository tastyRecipeRepository)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeType>> GetRecipeType(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var recipeType = await _context.RecipeTypes.FirstOrDefaultAsync(r => r.ID == id);

                if (recipeType == null)
                {
                    return NotFound();
                }

                return Ok(recipeType);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving recipe type");
            }
        }
    }
}
