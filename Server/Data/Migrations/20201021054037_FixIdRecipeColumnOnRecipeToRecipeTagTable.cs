using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class FixIdRecipeColumnOnRecipeToRecipeTagTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IdRecipe",
                table: "Recipe_RecipeTags",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdRecipe",
                table: "Recipe_RecipeTags",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
