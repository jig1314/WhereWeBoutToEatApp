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
    public class RecipeTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipeTypesController(AppDbContext context, ITastyRecipeRepository tastyRecipeRepository)
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
