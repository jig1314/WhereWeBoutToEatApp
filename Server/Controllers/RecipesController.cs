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
                if (databaseResults?.Count > 50)
                {
                    return Ok(databaseResults);
                }

                await ImportRecipesFromTastyAsync(name);

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

        private async Task ImportRecipesFromTastyAsync(string name)
        {
            var tastyRecipes = await tastyRecipeRepository.SearchRecipes(name);
            var commitedRecipes = await _context.Recipes.ToListAsync();

            tastyRecipes = (from tastyRecipe in tastyRecipes
                            join recipe in commitedRecipes on new { tastyRecipe.Recipe.ApiId, tastyRecipe.Recipe.IdRecipeType } equals new { recipe.ApiId, recipe.IdRecipeType } 
                            into existingRecipes
                            from recipe in existingRecipes.DefaultIfEmpty()
                            where recipe == null
                            select tastyRecipe).ToList();

            var recipes = tastyRecipes.Select(r => r.Recipe);

            await _context.Recipes.AddRangeAsync(recipes);
            await _context.SaveChangesAsync();

            commitedRecipes = await _context.Recipes.ToListAsync();
            var commitedRecipesWithTags = commitedRecipes.Join(tastyRecipes, 
                                                        recipe => new { recipe.ApiId, recipe.IdRecipeType }, 
                                                        tastyRecipe => new { tastyRecipe.Recipe.ApiId, tastyRecipe.Recipe.IdRecipeType }, 
                                                        (r, t) => new { Recipe = r, Tags = t.TastyRecipeTags }).ToList();

            var recipeTags = await _context.RecipeTags.ToListAsync();
            var recipeTagTypes = await _context.RecipeTagTypes.ToListAsync();

            var recipeTagsWithType = recipeTags.Join(recipeTagTypes, tag => tag.IdRecipeTagType, type => type.Id, (tag, type) => new { Tag = tag, Type = type });
            var distinctTastyRecipeTags = commitedRecipesWithTags.SelectMany(recipe => recipe.Tags).Distinct(new TastyRecipeTagComparer());

            var tastyRecipeTagToRecipeTagsDictionary =
                    (from tastyRecipeTag in distinctTastyRecipeTags
                     join recipeTag in recipeTagsWithType
                     on new { Name = tastyRecipeTag.Name.ToUpper(), DisplayName = tastyRecipeTag.DisplayName.ToUpper(), Type = tastyRecipeTag.RecipeTagType.ToUpper() }
                     equals new { Name = recipeTag.Tag.Name.ToUpper(), DisplayName = recipeTag.Tag.DisplayName.ToUpper(), Type = recipeTag.Type.Type.ToUpper() }
                     select new
                     {
                         TastyRecipeTag = tastyRecipeTag,
                         RecipeTag = recipeTag
                     }).ToDictionary(mapping => mapping.TastyRecipeTag, mapping => mapping.RecipeTag.Tag, new TastyRecipeTagComparer());


            foreach (var recipe in commitedRecipesWithTags)
            {
                var recipe_RecipeTags = recipe.Tags.Where(tag => tastyRecipeTagToRecipeTagsDictionary.ContainsKey(tag)).Select(tag => tastyRecipeTagToRecipeTagsDictionary[tag]).Select(tag => new Recipe_RecipeTag()
                {
                    IdRecipe = recipe.Recipe.Id,
                    IdRecipeTag = tag.Id
                });

                await _context.Recipe_RecipeTags.AddRangeAsync(recipe_RecipeTags);
            }
            
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
