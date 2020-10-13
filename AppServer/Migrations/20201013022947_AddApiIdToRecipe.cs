using Microsoft.EntityFrameworkCore.Migrations;

namespace AppServer.Migrations
{
    public partial class AddApiIdToRecipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Recipes",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "ApiId",
                table: "Recipes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiId",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Recipes",
                newName: "ID");
        }
    }
}
