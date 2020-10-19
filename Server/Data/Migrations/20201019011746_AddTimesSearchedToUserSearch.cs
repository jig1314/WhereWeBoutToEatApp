using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class AddTimesSearchedToUserSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimesSearched",
                table: "AspNetUserSearches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesSearched",
                table: "AspNetUserSearches");
        }
    }
}
