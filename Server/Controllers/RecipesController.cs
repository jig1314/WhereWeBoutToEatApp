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
using Microsoft.Azure.CognitiveServices.Personalizer;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;

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

        [HttpGet("favorites")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetUserFavoriteRecipes()
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    throw new Exception();
                }

                var idUser = GetUserId();
                var favoriteRecipes = await _context.AspNetUserRecipes.Where(userRecipe => userRecipe.IdUser == idUser && userRecipe.IsFavorite)
                                                                        .Join(_context.Recipes,
                                                                        userRecipe => userRecipe.IdRecipe,
                                                                        recipe => recipe.Id,
                                                                        (userRecipe, recipe) => recipe).ToListAsync();
                return Ok(favoriteRecipes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving recipes");
            }
        }

        [HttpGet("recommendations")]
        public async Task<ActionResult<RankedRecipes>> GetRecommendations()
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    throw new Exception();
                }

                var apiKey = (await _context.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)Enums.AppSetting.PersonalizerApiKey)).Value;
                var serviceEndpoint = (await _context.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)Enums.AppSetting.PersonalizerServiceEndpoint)).Value;

                PersonalizerClient client = new PersonalizerClient(new ApiKeyServiceClientCredentials(apiKey))
                {
                    Endpoint = serviceEndpoint
                };

                var idUser = GetUserId();

                var userRecipeTags = await _context.AspNetUserRecipeTag.Where(userTag => userTag.IdUser == idUser)
                                    .Join(
                                        _context.RecipeTags,
                                        userTag => userTag.IdRecipeTag,
                                        tag => tag.Id,
                                        (userTag, tag) => tag)
                                    .Join(
                                    _context.RecipeTagTypes,
                                    tag => tag.IdRecipeTagType,
                                    type => type.Id,
                                    (tag, type) => new { tag, type }).ToListAsync();
                var userRecipeTagDictionary = userRecipeTags.GroupBy(recipe => recipe.type).ToDictionary(mapping => mapping.Key.EnumCode, mapping => userRecipeTags.Where(tag => tag.type.Id == mapping.Key.Id).Select(tag => tag.tag.Name));

                IList<object> currentContext = new List<object>() {
                    new { Cuisine = userRecipeTagDictionary.ContainsKey((int)Shared.Enums.Recipe.RecipeTagType.Cuisine) ? string.Join(',', userRecipeTagDictionary[(int)Shared.Enums.Recipe.RecipeTagType.Cuisine]) : string.Empty },
                    new {  Dietary = userRecipeTagDictionary.ContainsKey((int)Shared.Enums.Recipe.RecipeTagType.Dietary) ? string.Join(',', userRecipeTagDictionary[(int)Shared.Enums.Recipe.RecipeTagType.Dietary]) : string.Empty },
                    new { Occasion = userRecipeTagDictionary.ContainsKey((int)Shared.Enums.Recipe.RecipeTagType.Occasion) ? string.Join(',', userRecipeTagDictionary[(int)Shared.Enums.Recipe.RecipeTagType.Occasion]) : string.Empty },
                    new { Meal = userRecipeTagDictionary.ContainsKey((int)Shared.Enums.Recipe.RecipeTagType.Meal) ? string.Join(',', userRecipeTagDictionary[(int)Shared.Enums.Recipe.RecipeTagType.Meal]) : string.Empty },
                    new { Seasonal = userRecipeTagDictionary.ContainsKey((int)Shared.Enums.Recipe.RecipeTagType.Seasonal) ? string.Join(',', userRecipeTagDictionary[(int)Shared.Enums.Recipe.RecipeTagType.Seasonal]) : string.Empty },
                    new { Holiday = userRecipeTagDictionary.ContainsKey((int)Shared.Enums.Recipe.RecipeTagType.Holiday) ? string.Join(',', userRecipeTagDictionary[(int)Shared.Enums.Recipe.RecipeTagType.Holiday]) : string.Empty }
                };

                (IEnumerable<Recipe> recipes, string selectedSearchCriteria) = await GetUserRecipes();

                if (recipes != null)
                {
                    var recipeWithTags = recipes.Join(_context.Recipe_RecipeTags,
                                                    recipe => recipe.Id,
                                                    recipeTagMapping => recipeTagMapping.IdRecipe,
                                                    (recipe, recipeTagMapping) => new { recipe, recipeTagMapping })
                                                .Join(_context.RecipeTags,
                                                        recipeTagMapping => recipeTagMapping.recipeTagMapping.IdRecipeTag,
                                                        recipeTag => recipeTag.Id,
                                                        (recipeTagMapping, recipeTag) => new { recipeTagMapping.recipe, recipeTag })
                                                .Join(_context.RecipeTagTypes,
                                                tag => tag.recipeTag.IdRecipeTagType,
                                                type => type.Id,
                                                (tag, type) => new { tag.recipe, tag.recipeTag, type }).ToList();

                    var recipeWithTagsDictionary = recipeWithTags.GroupBy(recipe => recipe.recipe).ToDictionary(group => group.Key, group => recipeWithTags.Where(item => item.recipe == group.Key).Select(item => item));
                    IList<RankableAction> actions = recipeWithTagsDictionary.Select(recipe => new RankableAction()
                    {
                        Id = recipe.Key.Id.ToString(),
                        Features = new List<object>()
                    {
                        new { Cuisine = string.Join(',', recipe.Value.Where(item => item.type.EnumCode == (int)Shared.Enums.Recipe.RecipeTagType.Cuisine).Select(item => item.recipeTag.Name)) },
                        new { Dietary = string.Join(',', recipe.Value.Where(item => item.type.EnumCode == (int)Shared.Enums.Recipe.RecipeTagType.Dietary).Select(item => item.recipeTag.Name)) },
                        new { Occasion = string.Join(',', recipe.Value.Where(item => item.type.EnumCode == (int)Shared.Enums.Recipe.RecipeTagType.Occasion).Select(item => item.recipeTag.Name)) },
                        new { Meal = string.Join(',', recipe.Value.Where(item => item.type.EnumCode == (int)Shared.Enums.Recipe.RecipeTagType.Meal).Select(item => item.recipeTag.Name)) },
                        new { Seasonal = string.Join(',', recipe.Value.Where(item => item.type.EnumCode == (int)Shared.Enums.Recipe.RecipeTagType.Seasonal).Select(item => item.recipeTag.Name)) },
                        new { Holiday = string.Join(',', recipe.Value.Where(item => item.type.EnumCode == (int)Shared.Enums.Recipe.RecipeTagType.Holiday).Select(item => item.recipeTag.Name)) }
                    }
                    }).Take(50).ToList();

                    IList<string> excludeActions = new List<string>();
                    var eventId = Guid.NewGuid().ToString();

                    // Rank the actions
                    var request = new RankRequest(actions, currentContext, excludeActions, eventId);
                    RankResponse response = await client.RankAsync(request);
                    var rankedRecipes = await GetRecipes(response.Ranking, Convert.ToInt64(response.RewardActionId));

                    return new RankedRecipes()
                    {
                        EventId = response.EventId,
                        RecommendedRecipeId = Convert.ToInt64(response.RewardActionId),
                        Recipes = rankedRecipes,
                        RecipeSearchCriteria = selectedSearchCriteria
                    };
                }

                return null;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving recipes");
            }
        }

        private async Task<(IEnumerable<Recipe>, string)> GetUserRecipes()
        {
            var recipeTag = await GetSearchCriteriaFromUserInformation();
            if (recipeTag == null)
                return (null, null);

            return (await SearchForUserRecipesAsync(recipeTag.Name), recipeTag.DisplayName);
        }

        private async Task<RecipeTag> GetSearchCriteriaFromUserInformation()
        {
            var idUser = GetUserId();
            var userRecipeTags = new List<RecipeTag>();

            var tagsFromUserPreferences = await _context.AspNetUserRecipeTag.Where(userRecipeTag => userRecipeTag.IdUser == idUser)
                                                                            .Join(
                                                                            _context.RecipeTags,
                                                                            userRecipeTag => userRecipeTag.IdRecipeTag,
                                                                            recipeTag => recipeTag.Id,
                                                                            (userRecipeTag, recipeTag) => recipeTag).ToListAsync();
            userRecipeTags.AddRange(tagsFromUserPreferences);

            if (userRecipeTags?.Count > 0)
            {
                var random = new Random();
                int randomIndex = random.Next(userRecipeTags.Count);

                return userRecipeTags[randomIndex];
            }
            else
            {
                return null;
            }
        }

        private async Task<List<Recipe>> GetRecipes(IList<RankedAction> rankRecipes, long idRecommededRecipe)
        {
            var recipes = await _context.Recipes.ToListAsync();
            return recipes.Join(rankRecipes,
                                recipe => recipe.Id,
                                rankRecipe => Convert.ToInt64(rankRecipe.Id),
                                (recipe, rankRecipe) => new { recipe, rankRecipe })
                            .OrderByDescending(recipe => recipe.recipe.Id == idRecommededRecipe)
                            .ThenByDescending(recipe => recipe.rankRecipe.Probability).Select(recipe => recipe.recipe).ToList();
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

        private async Task<IEnumerable<Recipe>> SearchForUserRecipesAsync(string tag)
        {
            var databaseResults = await SearchDatabaseByTagAsync(tag);
            if (databaseResults?.Count >= 50)
                return databaseResults;

            await ImportRecipesFromTastyByTagAsync(tag);

            return await SearchDatabaseByTagAsync(tag);
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
                if (databaseResults?.Count >= 50)
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

        private async Task<List<Recipe>> SearchDatabaseByTagAsync(string tag)
        {
            return await _context.Recipes.Join(_context.Recipe_RecipeTags,
                                                recipe => recipe.Id,
                                                recipeTagMapping => recipeTagMapping.IdRecipe,
                                                (recipe, recipeTagMapping) => new { recipe, recipeTagMapping })
                                            .Join(_context.RecipeTags,
                                            recipe => recipe.recipeTagMapping.IdRecipeTag,
                                            tag => tag.Id,
                                            (recipe, tag) => new { recipe, tag })
                                            .Where(recipe => recipe.tag.Name.Contains(tag))
                                            .Select(recipe => recipe.recipe.recipe).ToListAsync();
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

        private async Task ImportRecipesFromTastyByTagAsync(string tag)
        {
            var tastyRecipes = await tastyRecipeRepository.SearchRecipesByTag(tag);
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

        [HttpPut("recommendationSelected")]
        public async Task<IActionResult> PutRecommendationAsSelected(object eventId)
        {
            var apiKey = (await _context.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)Enums.AppSetting.PersonalizerApiKey)).Value;
            var serviceEndpoint = (await _context.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)Enums.AppSetting.PersonalizerServiceEndpoint)).Value;

            PersonalizerClient client = new PersonalizerClient(new ApiKeyServiceClientCredentials(apiKey))
            {
                Endpoint = serviceEndpoint
            };

            var eventIdAsString = eventId.ToString();
            await client.RewardAsync(eventIdAsString, new RewardRequest(1));
            return NoContent();
        }
    }
}
