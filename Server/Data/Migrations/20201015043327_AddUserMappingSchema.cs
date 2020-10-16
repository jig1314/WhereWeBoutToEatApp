using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class AddUserMappingSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUserRecipes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdRecipe = table.Column<long>(nullable: false),
                    IsFavorite = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRecipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserSearches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Search = table.Column<string>(nullable: true),
                    IdSearchType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserSearches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumCode = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserRecipes");

            migrationBuilder.DropTable(
                name: "AspNetUserSearches");

            migrationBuilder.DropTable(
                name: "SearchTypes");
        }
    }
}
