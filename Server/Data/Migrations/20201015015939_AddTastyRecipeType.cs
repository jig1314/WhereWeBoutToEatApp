using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class AddTastyRecipeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into RecipeTypes (EnumCode,Name, Description, BaseURL)
                values (1, 'Tasty', 'Recipes from the Tasty API', 'https://tasty.co/recipe/')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                delete from RecipeTypes 
                where EnumCode = 1
            ");
        }
    }
}
