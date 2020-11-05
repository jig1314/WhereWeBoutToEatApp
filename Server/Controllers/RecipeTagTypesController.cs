using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhereWeBoutToEatApp.Server.Data;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeTagTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipeTagTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RecipeTagTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeTagType>>> GetRecipeTagTypes()
        {
            return await _context.RecipeTagTypes.ToListAsync();
        }

        // GET: api/RecipeTagTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeTagType>> GetRecipeTagType(int id)
        {
            var recipeTagType = await _context.RecipeTagTypes.FindAsync(id);

            if (recipeTagType == null)
            {
                return NotFound();
            }

            return recipeTagType;
        }
    }
}
