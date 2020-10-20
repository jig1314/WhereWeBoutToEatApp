using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
//using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WhereWeBoutToEatApp.Server.Data;
using WhereWeBoutToEatApp.Server.Enums;
using WhereWeBoutToEatApp.Shared;

namespace WhereWeBoutToEatApp.Server.Repositories
{
    public class TastyRecipeRepository : ITastyRecipeRepository
    {
        private readonly ApplicationDbContext appDbContext;

        public TastyRecipeRepository(ApplicationDbContext context)
        {
            this.appDbContext = context;
        }

        public async Task<List<Recipe>> SearchRecipes(string name)
        {
            var tastyApiUrl = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiUrl)).Value;
            var tastyApiHostName = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiHostName)).Value;
            var tastyApiHostValue = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiHostValue)).Value;
            var tastyApiKeyName = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiKeyName)).Value;
            var tastyApiKeyValue = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiKeyValue)).Value;

            var tastyRecipeType = await appDbContext.RecipeTypes.FirstOrDefaultAsync(type => type.EnumCode == (int)Shared.Enums.Recipe.RecipeType.Tasty);

            var client = new RestClient($"{tastyApiUrl}{name}&from=0&sizes=50");
            var request = new RestRequest(Method.GET);
            request.AddHeader(tastyApiHostName, tastyApiHostValue);
            request.AddHeader(tastyApiKeyName, tastyApiKeyValue);
            IRestResponse response = await client.ExecuteAsync(request);

            var content = JsonConvert.DeserializeObject<JToken>(response.Content);

            var results = content.SelectTokens("results[*]");
            var recipes = results.Select(recipe => new Recipe
            {
                ApiId = (int)recipe["id"],
                Name = (string)recipe["name"],
                ThumbnailURL = (string)recipe["thumbnail_url"],
                VideoURL = (string)recipe["original_video_url"],
                UrlSuffix = (string)recipe["slug"],
                IdRecipeType = tastyRecipeType.ID
            }).ToList();

            foreach (var result in results)
            {
                var childRecipes = result.SelectTokens("recipes[*]").Select(recipe => new Recipe
                {
                    ApiId = (int)recipe["id"],
                    Name = (string)recipe["name"],
                    ThumbnailURL = (string)recipe["thumbnail_url"],
                    VideoURL = (string)recipe["original_video_url"],
                    UrlSuffix = (string)recipe["slug"],
                    IdRecipeType = tastyRecipeType.ID
                }).ToList();

                recipes.AddRange(childRecipes);
            }

            recipes = recipes.Distinct(new RecipeComparer()).ToList();

            return recipes;
        }
    }
}
