using Microsoft.EntityFrameworkCore.Migrations;

namespace AppServer.Migrations
{
    public partial class AddExtraColumnsToRecipeAndRecipeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseURL",
                table: "RecipeTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlSuffix",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoURL",
                table: "Recipes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseURL",
                table: "RecipeTypes");

            migrationBuilder.DropColumn(
                name: "UrlSuffix",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "VideoURL",
                table: "Recipes");
        }
    }
}
