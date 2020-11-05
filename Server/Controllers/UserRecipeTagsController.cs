using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class UserRecipeTagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserRecipeTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserRecipeTags/5
        [HttpGet("{idRecipeTagType}")]
        public async Task<ActionResult<IEnumerable<AspNetUserRecipeTag>>> GetAspNetUserRecipeTag(int idRecipeTagType)
        {
            var idUser = GetUserId();
            var query = (from recipeTag in _context.RecipeTags.Where(tag => tag.IdRecipeTagType == idRecipeTagType)
                         join userRecipeTag in _context.AspNetUserRecipeTag.Where(item => item.IdUser == idUser) on recipeTag.Id equals userRecipeTag.IdRecipeTag
                         select userRecipeTag);
            return await query.ToListAsync();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{idRecipeTagType}")]
        public async Task<IActionResult> PutAspNetUserRecipeTag(int idRecipeTagType, List<int?> idRecipeTags)
        {
            var idUser = GetUserId();
            var committedUserRecipeTags = await (from recipeTag in _context.RecipeTags.Where(tag => tag.IdRecipeTagType == idRecipeTagType)
                                                   join userRecipeTag in _context.AspNetUserRecipeTag.Where(item => item.IdUser == idUser) on recipeTag.Id equals userRecipeTag.IdRecipeTag
                                                   select userRecipeTag).ToListAsync();

            var userRecipeTagsToDelete = (from committedUserRecipeTag in committedUserRecipeTags
                                          join recipeTagId in idRecipeTags on committedUserRecipeTag.IdRecipeTag equals recipeTagId
                                          into existingUserRecipeTags
                                          from recipeTag in existingUserRecipeTags.DefaultIfEmpty()
                                          where recipeTag == null
                                          select committedUserRecipeTag).ToList();

            var userRecipeTagsToInsert = (from recipeTagId in idRecipeTags
                                          join committedUserRecipeTag in committedUserRecipeTags on recipeTagId equals committedUserRecipeTag.IdRecipeTag
                                          into existingUserRecipeTags
                                          from recipeTag in existingUserRecipeTags.DefaultIfEmpty()
                                          where recipeTag == null
                                          select recipeTagId).ToList();

            _context.AspNetUserRecipeTag.RemoveRange(userRecipeTagsToDelete);
            _context.AspNetUserRecipeTag.AddRange(userRecipeTagsToInsert.Select(tag => new AspNetUserRecipeTag() { IdRecipeTag = tag.Value, IdUser = idUser }));
            await _context.SaveChangesAsync();

            return Ok();
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
