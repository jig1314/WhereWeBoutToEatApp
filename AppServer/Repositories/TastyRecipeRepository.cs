using AppModels;
using AppServer.Controllers;
using AppServer.DatabaseContexts;
using AppServer.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AppServer.Repositories
{
    public class TastyRecipeRepository : ITastyRecipeRepository
    {
        private readonly AppDbContext appDbContext;

        public TastyRecipeRepository(AppDbContext context)
        {
            this.appDbContext = context;
        }

        public async Task<IEnumerable<Recipe>> SearchRecipes(string name)
        {
            var tastyApiUrl = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiUrl)).Value;
            var tastyApiHostName = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiHostName)).Value;
            var tastyApiHostValue = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiHostValue)).Value;
            var tastyApiKeyName = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiKeyName)).Value;
            var tastyApiKeyValue = (await appDbContext.AppSettings.FirstOrDefaultAsync(setting => setting.EnumCode == (int)AppSetting.TastyApiKeyValue)).Value;

            var client = new RestClient($"{tastyApiUrl}{name}&from=0&sizes=20");
            var request = new RestRequest(Method.GET);
            request.AddHeader(tastyApiHostName, tastyApiHostValue);
            request.AddHeader(tastyApiKeyName, tastyApiKeyValue);
            IRestResponse response = await client.ExecuteAsync(request);

            var content = JsonConvert.DeserializeObject<JToken>(response.Content);

            var recipes = content.SelectTokens("results[*]")
                .Select(recipe => new Recipe
                {
                    Name = (string)recipe["name"],
                    ThumbnailURL = (string)recipe["thumbnail_url"]
                })
                .ToList();

            return recipes;
        }
    }
}
