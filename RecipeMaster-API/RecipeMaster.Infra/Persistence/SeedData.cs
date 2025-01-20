using RecipeMaster.Core.Entities;
using RecipeMaster.Infra.Identity;
using Microsoft.AspNetCore.Identity;
using RecipeMaster.Core.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeMaster.Infra.Persistence;

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
        // Expanded Seed Ingredients
        var ingredients = new[]
        {
            new Ingredient("Salt", "Fine table salt.", MeasurementUnit.Gram, new IngredientCost(0.05m),
                stockQuantity: 1000, minimumStockLevel: 100, supplierName: "Salt Co.", isPerishable: false,
                originCountry: "USA", storageInstructions: "Store in a cool, dry place.", isActive: true),
            new Ingredient("Sugar", "Granulated white sugar.", MeasurementUnit.Gram, new IngredientCost(0.10m),
                stockQuantity: 2000, minimumStockLevel: 200, supplierName: "Sweet Supplies", isPerishable: false,
                originCountry: "Brazil", storageInstructions: "Keep in an airtight container.", isActive: true),
            new Ingredient("Flour", "All-purpose flour.", MeasurementUnit.Kilogram, new IngredientCost(1.50m),
                stockQuantity: 500, minimumStockLevel: 50, supplierName: "Flour Mills", isPerishable: false,
                originCountry: "Canada", storageInstructions: "Store in a sealed container.", isActive: true),
            new Ingredient("Butter", "Unsalted butter.", MeasurementUnit.Kilogram, new IngredientCost(3.00m),
                stockQuantity: 100, minimumStockLevel: 20, supplierName: "DairyBest", isPerishable: true,
                originCountry: "France", storageInstructions: "Refrigerate at 4°C.", isActive: true),
            new Ingredient("Milk", "Whole milk.", MeasurementUnit.Liter, new IngredientCost(0.80m),
                stockQuantity: 300, minimumStockLevel: 50, supplierName: "DairyFresh", isPerishable: true,
                originCountry: "Germany", storageInstructions: "Keep refrigerated at 4°C.", isActive: true),
            new Ingredient("Eggs", "Large eggs.", MeasurementUnit.Unit, new IngredientCost(0.30m),
                stockQuantity: 500, minimumStockLevel: 100, supplierName: "FreshEgg Farms", isPerishable: true,
                originCountry: "Netherlands", storageInstructions: "Refrigerate at 4°C.", isActive: true),
            new Ingredient("Chocolate", "Dark chocolate chips.", MeasurementUnit.Kilogram, new IngredientCost(5.00m),
                stockQuantity: 100, minimumStockLevel: 10, supplierName: "ChocoDelight", isPerishable: false,
                originCountry: "Belgium", storageInstructions: "Store in a cool, dry place.", isActive: true),
            new Ingredient("Vanilla", "Pure vanilla extract.", MeasurementUnit.Milliliter, new IngredientCost(0.20m),
                stockQuantity: 200, minimumStockLevel: 20, supplierName: "VanillaCo", isPerishable: false,
                originCountry: "Madagascar", storageInstructions: "Keep in a sealed bottle.", isActive: true),
            new Ingredient("Baking Powder", "Leavening agent.", MeasurementUnit.Gram, new IngredientCost(0.02m),
                stockQuantity: 500, minimumStockLevel: 50, supplierName: "RiseUp", isPerishable: false,
                originCountry: "India", storageInstructions: "Store in a dry place.", isActive: true),
            new Ingredient("Oil", "Vegetable oil.", MeasurementUnit.Liter, new IngredientCost(1.20m),
                stockQuantity: 150, minimumStockLevel: 20, supplierName: "Healthy Harvest", isPerishable: false,
                originCountry: "Italy", storageInstructions: "Store in a cool place.", isActive: true),
            new Ingredient("Cheese", "Parmesan cheese.", MeasurementUnit.Kilogram, new IngredientCost(8.50m),
                stockQuantity: 50, minimumStockLevel: 10, supplierName: "Cheese World", isPerishable: true,
                originCountry: "Italy", storageInstructions: "Refrigerate at 4°C.", isActive: true),
            new Ingredient("Tomato Sauce", "Rich tomato sauce.", MeasurementUnit.Liter, new IngredientCost(2.00m),
                stockQuantity: 100, minimumStockLevel: 20, supplierName: "SauceCo", isPerishable: true,
                originCountry: "Spain", storageInstructions: "Refrigerate after opening.", isActive: true)
        };

        foreach (var ingredient in ingredients)
        {
            if (!context.Ingredients.Any(i => i.Name == ingredient.Name))
            {
                context.Ingredients.Add(ingredient);
            }
        }

        context.SaveChanges();

        // Expanded Seed Recipes
        var recipes = new[]
        {
            new Recipe("Cake", "A simple cake recipe", 15, 45, 8, "Mix ingredients and bake.", DifficultyLevel.Medium),
            new Recipe("Pancakes", "A quick breakfast recipe", 10, 20, 4, "Mix, fry, and serve.", DifficultyLevel.Easy),
            new Recipe("Cookies", "Delicious chocolate chip cookies", 20, 25, 12, "Mix dough, add chips, bake.",
                DifficultyLevel.Medium),
            new Recipe("Lasagna", "Classic Italian lasagna", 30, 60, 6, "Layer pasta, sauce, cheese, and bake.",
                DifficultyLevel.Hard),
            new Recipe("Salad", "Healthy vegetable salad", 10, 0, 2, "Chop vegetables and mix.", DifficultyLevel.Easy),
            new Recipe("Brownies", "Rich chocolate brownies", 15, 30, 10, "Mix, bake, and cool.", DifficultyLevel.Medium),
            new Recipe("Pizza", "Classic Italian pizza", 20, 15, 4, "Prepare dough, add toppings, bake.",
                DifficultyLevel.Hard)
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
            new { Recipe = "Cake", Ingredient = "Flour", Quantity = 300 },
            new { Recipe = "Cake", Ingredient = "Eggs", Quantity = 4 },
            new { Recipe = "Cake", Ingredient = "Butter", Quantity = 100 },
            new { Recipe = "Pancakes", Ingredient = "Flour", Quantity = 200 },
            new { Recipe = "Pancakes", Ingredient = "Milk", Quantity = 300 },
            new { Recipe = "Pancakes", Ingredient = "Eggs", Quantity = 2 },
            new { Recipe = "Cookies", Ingredient = "Flour", Quantity = 250 },
            new { Recipe = "Cookies", Ingredient = "Sugar", Quantity = 100 },
            new { Recipe = "Cookies", Ingredient = "Chocolate", Quantity = 150 },
            new { Recipe = "Lasagna", Ingredient = "Flour", Quantity = 500 },
            new { Recipe = "Lasagna", Ingredient = "Cheese", Quantity = 200 },
            new { Recipe = "Lasagna", Ingredient = "Tomato Sauce", Quantity = 300 },
            new { Recipe = "Salad", Ingredient = "Salt", Quantity = 10 },
            new { Recipe = "Salad", Ingredient = "Oil", Quantity = 20 },
            new { Recipe = "Brownies", Ingredient = "Chocolate", Quantity = 200 },
            new { Recipe = "Brownies", Ingredient = "Butter", Quantity = 100 },
            new { Recipe = "Brownies", Ingredient = "Flour", Quantity = 150 },
            new { Recipe = "Pizza", Ingredient = "Flour", Quantity = 300 },
            new { Recipe = "Pizza", Ingredient = "Tomato Sauce", Quantity = 200 },
            new { Recipe = "Pizza", Ingredient = "Cheese", Quantity = 150 }
        };

        foreach (var ri in recipeIngredients)
        {
            if (!context.RecipeIngredients.Any(r =>
                    r.RecipeId == recipeDict[ri.Recipe] && r.IngredientId == ingredientDict[ri.Ingredient]))
            {
                context.RecipeIngredients.Add(new RecipeIngredient(recipeDict[ri.Recipe], ingredientDict[ri.Ingredient],
                    ri.Quantity));
            }
        }
        context.SaveChanges();
    }
}
