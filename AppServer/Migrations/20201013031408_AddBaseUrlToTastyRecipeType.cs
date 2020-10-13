using Microsoft.EntityFrameworkCore.Migrations;

namespace AppServer.Migrations
{
    public partial class AddBaseUrlToTastyRecipeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                update RecipeTypes
                set BaseURL = 'https://tasty.co/recipe/'
                where EnumCode = 1
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                update RecipeTypes
                set BaseURL =  null
                where EnumCode = 1
            ");
        }
    }
}
