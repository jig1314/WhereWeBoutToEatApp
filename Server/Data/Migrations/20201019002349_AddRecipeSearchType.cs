using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class AddRecipeSearchType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into SearchTypes (EnumCode,Name, Description)
                values (1, 'Recipe', 'User searches for recipes')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                delete from SearchTypes 
                where EnumCode = 1
            ");
        }
    }
}
