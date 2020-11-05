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
    public class RecipeTagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipeTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{idRecipeTagType}")]
        public async Task<ActionResult<IEnumerable<RecipeTag>>> GetRecipeTags(int idRecipeTagType)
        {
            return await _context.RecipeTags.Where(tag => tag.IdRecipeTagType == idRecipeTagType).ToListAsync();
        }
    }
}
