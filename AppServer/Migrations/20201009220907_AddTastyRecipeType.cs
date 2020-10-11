using Microsoft.EntityFrameworkCore.Migrations;

namespace AppServer.Migrations
{
    public partial class AddTastyRecipeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into RecipeTypes (EnumCode,Name, Description)
                values (1, 'Tasty', 'Recipes from the Tasty API')
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
