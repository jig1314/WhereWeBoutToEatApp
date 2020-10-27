using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class AddUniqueIndexToRecipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EnumCode",
                table: "RecipeTags",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ApiId_IdRecipeType",
                table: "Recipes",
                columns: new[] { "ApiId", "IdRecipeType" },
                unique: true,
                filter: "[ApiId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipes_ApiId_IdRecipeType",
                table: "Recipes");

            migrationBuilder.AlterColumn<int>(
                name: "EnumCode",
                table: "RecipeTags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
