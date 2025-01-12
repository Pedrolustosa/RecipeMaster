using RecipeMaster.Core.Entities;
using RecipeMaster.Infra.Identity;
using Microsoft.AspNetCore.Identity;
using RecipeMaster.Core.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeMaster.Infra.Persistence
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<RecipeMasterDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // Ensure the database is created
            context.Database.EnsureCreated();

            // Seed roles
            SeedRoles(roleManager);

            // Seed users
            SeedUsers(userManager);

            // Avoid duplicate seed
            if (!context.Ingredients.Any() && !context.Recipes.Any())
            {
                SeedIngredientsAndRecipes(context);
            }
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    roleManager.CreateAsync(new ApplicationRole { Name = role }).Wait();
                }
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var users = new[]
            {
                new { Email = "admin@recipemaster.com", Password = "Admin@123", Role = "Admin" },
                new { Email = "user@recipemaster.com", Password = "User@123", Role = "User" }
            };

            foreach (var userData in users)
            {
                var user = userManager.FindByEmailAsync(userData.Email).Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userData.Email,
                        Email = userData.Email,
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(user, userData.Password).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, userData.Role).Wait();
                    }
                }
            }
        }

        private static void SeedIngredientsAndRecipes(RecipeMasterDbContext context)
        {
            // Seed Ingredients
            var ingredients = new[]
            {
                new Ingredient("Salt", MeasurementUnit.Gram, new IngredientCost(0.05m)),
                new Ingredient("Sugar", MeasurementUnit.Gram, new IngredientCost(0.10m)),
                new Ingredient("Flour", MeasurementUnit.Kilogram, new IngredientCost(1.50m)),
                new Ingredient("Butter", MeasurementUnit.Kilogram, new IngredientCost(3.00m)),
                new Ingredient("Milk", MeasurementUnit.Liter, new IngredientCost(0.80m))
            };

            foreach (var ingredient in ingredients)
            {
                if (!context.Ingredients.Any(i => i.Name == ingredient.Name))
                {
                    context.Ingredients.Add(ingredient);
                }
            }

            context.SaveChanges();

            // Seed Recipes
            var recipes = new[]
            {
                new Recipe("Cake", "A simple cake recipe"),
                new Recipe("Pancakes", "A quick breakfast recipe"),
                new Recipe("Cookies", "Delicious chocolate chip cookies")
            };

            foreach (var recipe in recipes)
            {
                if (!context.Recipes.Any(r => r.Name == recipe.Name))
                {
                    context.Recipes.Add(recipe);
                }
            }

            context.SaveChanges();

            // Seed RecipeIngredients
            var ingredientDict = context.Ingredients.ToDictionary(i => i.Name, i => i.Id);
            var recipeDict = context.Recipes.ToDictionary(r => r.Name, r => r.Id);

            var recipeIngredients = new[]
            {
                new { Recipe = "Cake", Ingredient = "Sugar", Quantity = 200 },
                new { Recipe = "Cake", Ingredient = "Salt", Quantity = 5 },
                new { Recipe = "Cake", Ingredient = "Flour", Quantity = 500 },
                new { Recipe = "Pancakes", Ingredient = "Milk", Quantity = 300 },
                new { Recipe = "Pancakes", Ingredient = "Flour", Quantity = 200 },
                new { Recipe = "Pancakes", Ingredient = "Sugar", Quantity = 50 },
                new { Recipe = "Cookies", Ingredient = "Butter", Quantity = 100 },
                new { Recipe = "Cookies", Ingredient = "Flour", Quantity = 250 },
                new { Recipe = "Cookies", Ingredient = "Sugar", Quantity = 150 }
            };

            foreach (var ri in recipeIngredients)
            {
                if (!context.RecipeIngredients.Any(r =>
                        r.RecipeId == recipeDict[ri.Recipe] && r.IngredientId == ingredientDict[ri.Ingredient]))
                {
                    context.RecipeIngredients.Add(new RecipeIngredient(recipeDict[ri.Recipe], ingredientDict[ri.Ingredient], ri.Quantity));
                }
            }

            context.SaveChanges();
        }
    }
}
