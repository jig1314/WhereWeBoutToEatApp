using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class InsertRecipeTagTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into RecipeTagTypes (Type)
                values 
	                ('Cuisine'),
	                ('Dietary'),
	                ('Occasion'),
	                ('Meal'),
	                ('Seasonal'),
	                ('Holiday')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                delete from RecipeTagTypes
                where Type in
                (
	                'Cuisine',
	                'Dietary',
	                'Occasion',
	                'Meal',
	                'Seasonal',
	                'Holiday'
                )
            ");
        }
    }
}
