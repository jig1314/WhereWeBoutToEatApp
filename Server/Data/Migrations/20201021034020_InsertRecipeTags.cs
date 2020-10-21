using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereWeBoutToEatApp.Server.Data.Migrations
{
    public partial class InsertRecipeTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				declare @idRecipeTagType_Cuisine int = (select Id from RecipeTagTypes where Type = 'Cuisine')
				declare @idRecipeTagType_Dietary int = (select Id from RecipeTagTypes where Type = 'Dietary')
				declare @idRecipeTagType_Occasion int = (select Id from RecipeTagTypes where Type = 'Occasion')
				declare @idRecipeTagType_Meal int = (select Id from RecipeTagTypes where Type = 'Meal')
				declare @idRecipeTagType_Seasonal int = (select Id from RecipeTagTypes where Type = 'Seasonal')
				declare @idRecipeTagType_Holiday int = (select Id from RecipeTagTypes where Type = 'Holiday')

				insert into RecipeTags (Name, DisplayName, IdRecipeTagType)
				values
					('greek', 'Greek', @idRecipeTagType_Cuisine),
					('japanese', 'Japanese', @idRecipeTagType_Cuisine),
					('middle_eastern', 'Middle Eastern', @idRecipeTagType_Cuisine),
					('vietnamese', 'Vietnamese', @idRecipeTagType_Cuisine),
					('gluten_free', 'Gluten-Free', @idRecipeTagType_Dietary),
					('vegetarian', 'Vegetarian', @idRecipeTagType_Dietary),
					('christmas', 'Christmas', @idRecipeTagType_Holiday),
					('easter', 'Easter', @idRecipeTagType_Holiday),
					('halloween', 'Halloween', @idRecipeTagType_Holiday),
					('brunch', 'Brunch', @idRecipeTagType_Occasion),
					('dinner', 'Dinner', @idRecipeTagType_Meal),
					('date_night', 'Date Night', @idRecipeTagType_Occasion),
					('casual_party', 'Casual Party', @idRecipeTagType_Occasion),
					('bbq', 'BBQ', @idRecipeTagType_Occasion),
					('spring', 'Spring', @idRecipeTagType_Seasonal),
					('special_occasion', 'Special Occasion', @idRecipeTagType_Occasion),
					('asian_pacific_american_heritage_month', 'Asian Pacific American Heritage Month', @idRecipeTagType_Holiday),
					('british', 'British', @idRecipeTagType_Cuisine),
					('german', 'German', @idRecipeTagType_Cuisine),
					('korean', 'Korean', @idRecipeTagType_Cuisine),
					('latin_american', 'Latin American', @idRecipeTagType_Cuisine),
					('seafood', 'Seafood', @idRecipeTagType_Cuisine),
					('comfort_food', 'Comfort Food', @idRecipeTagType_Dietary),
					('dairy_free', 'Dairy-Free', @idRecipeTagType_Dietary),
					('thanksgiving', 'Thanksgiving', @idRecipeTagType_Holiday),
					('valentines_day', 'Valentine''s Day', @idRecipeTagType_Holiday),
					('desserts', 'Desserts', @idRecipeTagType_Meal),
					('summer', 'Summer', @idRecipeTagType_Seasonal),
					('african', 'African', @idRecipeTagType_Cuisine),
					('french', 'French', @idRecipeTagType_Cuisine),
					('lunch', 'Lunch', @idRecipeTagType_Meal),
					('snacks', 'Snacks', @idRecipeTagType_Meal),
					('game_day', 'Game Day', @idRecipeTagType_Occasion),
					('indulgent_sweets', 'Indulgent Sweets', @idRecipeTagType_Dietary),
					('contains_alcohol', 'Contains Alcohol', @idRecipeTagType_Dietary),
					('brazilian', 'Brazilian', @idRecipeTagType_Cuisine),
					('chinese', 'Chinese', @idRecipeTagType_Cuisine),
					('italian', 'Italian', @idRecipeTagType_Cuisine),
					('thai', 'Thai', @idRecipeTagType_Cuisine),
					('low_carb', 'Low-Carb', @idRecipeTagType_Dietary),
					('fourth_of_july', 'Fourth of July', @idRecipeTagType_Holiday),
					('breakfast', 'Breakfast', @idRecipeTagType_Meal),
					('drinks', 'Drinks', @idRecipeTagType_Meal),
					('pescatarian', 'Pescatarian', @idRecipeTagType_Dietary),
					('caribbean', 'Caribbean', @idRecipeTagType_Cuisine),
					('black_history_month', 'Black History Month', @idRecipeTagType_Holiday),
					('pride_month', 'Pride Month', @idRecipeTagType_Holiday),
					('hispanic_heritage_month', 'Hispanic Heritage Month', @idRecipeTagType_Holiday),
					('american', 'American', @idRecipeTagType_Cuisine),
					('bbq', 'BBQ', @idRecipeTagType_Cuisine),
					('indian', 'Indian', @idRecipeTagType_Cuisine),
					('mexican', 'Mexican', @idRecipeTagType_Cuisine),
					('healthy', 'Healthy', @idRecipeTagType_Dietary),
					('vegan', 'Vegan', @idRecipeTagType_Dietary),
					('appetizers', 'Appetizers', @idRecipeTagType_Meal),
					('kid_friendly', 'Kid-Friendly', @idRecipeTagType_Dietary),
					('sides', 'Sides', @idRecipeTagType_Meal),
					('happy_hour', 'Happy Hour', @idRecipeTagType_Occasion),
					('weeknight', 'Weeknight', @idRecipeTagType_Occasion),
					('fall', 'Fall', @idRecipeTagType_Seasonal),
					('winter', 'Winter', @idRecipeTagType_Seasonal),
					('fusion', 'Fusion', @idRecipeTagType_Cuisine),
					('bakery_goods', 'Bakery Goods', @idRecipeTagType_Meal)
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				delete from RecipeTags
				where Name in
				(
					'greek',
					'japanese',
					'middle_eastern',
					'vietnamese',
					'gluten_free',
					'vegetarian', 
					'christmas', 
					'easter',
					'halloween', 
					'brunch', 
					'dinner', 
					'date_night', 
					'casual_party',
					'bbq', 
					'spring',
					'special_occasion', 
					'asian_pacific_american_heritage_month', 
					'british', 
					'german', 
					'korean',
					'latin_american', 
					'seafood', 
					'comfort_food', 
					'dairy_free', 
					'thanksgiving',
					'valentines_day',
					'desserts', 
					'summer', 
					'african', 
					'french',
					'lunch', 
					'snacks',
					'game_day',
					'indulgent_sweets', 
					'contains_alcohol',
					'brazilian', 
					'chinese', 
					'italian',
					'thai', 
					'low_carb', 
					'fourth_of_july',
					'breakfast',
					'drinks', 
					'pescatarian', 
					'caribbean', 
					'black_history_month',
					'pride_month', 
					'hispanic_heritage_month',
					'american', 
					'bbq',
					'indian',
					'mexican',
					'healthy',
					'vegan',
					'appetizers', 
					'kid_friendly', 
					'sides', 
					'happy_hour', 
					'weeknight',
					'fall', 
					'winter',
					'fusion',
					'bakery_goods'
				)
            ");
        }
    }
}
