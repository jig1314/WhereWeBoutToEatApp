using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhereWeBoutToEatApp.Server.Data;
using WhereWeBoutToEatApp.Server.Models;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserRecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserRecipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUserRecipe>>> GetAspNetUserRecipes()
        {
            return await _context.AspNetUserRecipes.ToListAsync();
        }

        // GET: api/UserRecipes/5
        [HttpGet("{idRecipe}")]
        public async Task<ActionResult<AspNetUserRecipe>> GetAspNetUserRecipe(long idRecipe)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var idUser = GetUserId();
            var aspNetUserRecipe = await _context.AspNetUserRecipes.Where(ur => ur.IdRecipe == idRecipe && ur.IdUser == idUser).FirstOrDefaultAsync();

            if (aspNetUserRecipe == null)
            {
                var newUserRecipe = new AspNetUserRecipe()
                {
                    IdRecipe = idRecipe,
                    IdUser = idUser,
                    IsFavorite = false
                };

                _context.AspNetUserRecipes.Add(newUserRecipe);
                await _context.SaveChangesAsync();

                aspNetUserRecipe = await _context.AspNetUserRecipes.Where(ur => ur.IdRecipe == idRecipe && ur.IdUser == idUser).FirstOrDefaultAsync();
            }

            return Ok(aspNetUserRecipe);
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // PUT: api/UserRecipes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAspNetUserRecipe(long id, AspNetUserRecipe aspNetUserRecipe)
        {
            if (id != aspNetUserRecipe.Id)
            {
                return BadRequest();
            }

            _context.Entry(aspNetUserRecipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserRecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserRecipes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AspNetUserRecipe>> PostAspNetUserRecipe(AspNetUserRecipe aspNetUserRecipe)
        {
            _context.AspNetUserRecipes.Add(aspNetUserRecipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAspNetUserRecipe", new { id = aspNetUserRecipe.Id }, aspNetUserRecipe);
        }

        // DELETE: api/UserRecipes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AspNetUserRecipe>> DeleteAspNetUserRecipe(long id)
        {
            var aspNetUserRecipe = await _context.AspNetUserRecipes.FindAsync(id);
            if (aspNetUserRecipe == null)
            {
                return NotFound();
            }

            _context.AspNetUserRecipes.Remove(aspNetUserRecipe);
            await _context.SaveChangesAsync();

            return aspNetUserRecipe;
        }

        private bool AspNetUserRecipeExists(long id)
        {
            return _context.AspNetUserRecipes.Any(e => e.Id == id);
        }
    }
}
