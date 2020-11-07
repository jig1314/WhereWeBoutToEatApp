using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class UpdateRecipeTagTypesWithEnumCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                update RecipeTagTypes
                set EnumCode = 1
                where Type = 'Cuisine'

                update RecipeTagTypes
                set EnumCode = 2
                where Type = 'Dietary'

                update RecipeTagTypes
                set EnumCode = 3
                where Type = 'Occasion'

                update RecipeTagTypes
                set EnumCode = 4
                where Type = 'Meal'

                update RecipeTagTypes
                set EnumCode = 5
                where Type = 'Seasonal'

                update RecipeTagTypes
                set EnumCode = 6
                where Type = 'Holiday'
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                update RecipeTagTypes
                set EnumCode = null
                where Type = 'Cuisine'

                update RecipeTagTypes
                set EnumCode = null
                where Type = 'Dietary'

                update RecipeTagTypes
                set EnumCode = null
                where Type = 'Occasion'

                update RecipeTagTypes
                set EnumCode = null
                where Type = 'Meal'

                update RecipeTagTypes
                set EnumCode = null
                where Type = 'Seasonal'

                update RecipeTagTypes
                set EnumCode = null
                where Type = 'Holiday'
            ");
        }
    }
}
