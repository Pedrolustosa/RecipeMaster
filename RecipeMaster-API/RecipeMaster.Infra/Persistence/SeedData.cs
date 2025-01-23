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
        // Seed Ingredients
        var ingredients = new[]
        {
            new Ingredient("Salt", "Fine table salt.", MeasurementUnit.Gram, new IngredientCost(0.05m),
                stockQuantity: 1000, minimumStockLevel: 50, supplierName: "Salt Co.", isPerishable: false,
                originCountry: "USA", storageInstructions: "Store in a cool, dry place.", isActive: true),
            new Ingredient("Sugar", "Granulated white sugar.", MeasurementUnit.Gram, new IngredientCost(0.10m),
                stockQuantity: 1500, minimumStockLevel: 100, supplierName: "Sweet Supplies", isPerishable: false,
                originCountry: "Brazil", storageInstructions: "Keep in an airtight container.", isActive: true),
            new Ingredient("Flour", "All-purpose flour.", MeasurementUnit.Kilogram, new IngredientCost(1.50m),
                stockQuantity: 800, minimumStockLevel: 30, supplierName: "Flour Mills", isPerishable: false,
                originCountry: "Canada", storageInstructions: "Store in a sealed container.", isActive: true),
            new Ingredient("Eggs", "Large eggs.", MeasurementUnit.Unit, new IngredientCost(0.30m),
                stockQuantity: 200, minimumStockLevel: 20, supplierName: "FreshEgg Farms", isPerishable: true,
                originCountry: "Netherlands", storageInstructions: "Refrigerate at 4°C.", isActive: true),
            new Ingredient("Milk", "Whole milk.", MeasurementUnit.Liter, new IngredientCost(0.80m),
                stockQuantity: 200, minimumStockLevel: 20, supplierName: "DairyFresh", isPerishable: true,
                originCountry: "Germany", storageInstructions: "Keep refrigerated at 4°C.", isActive: true),
            new Ingredient("Butter", "Unsalted butter.", MeasurementUnit.Kilogram, new IngredientCost(3.00m),
                stockQuantity: 100, minimumStockLevel: 10, supplierName: "DairyBest", isPerishable: true,
                originCountry: "France", storageInstructions: "Refrigerate at 4°C.", isActive: true),
            new Ingredient("Vanilla", "Pure vanilla extract.", MeasurementUnit.Milliliter, new IngredientCost(0.20m),
                stockQuantity: 200, minimumStockLevel: 20, supplierName: "VanillaCo", isPerishable: false,
                originCountry: "Madagascar", storageInstructions: "Keep in a sealed bottle.", isActive: true),
            new Ingredient("Baking Powder", "Leavening agent.", MeasurementUnit.Gram, new IngredientCost(0.02m),
                stockQuantity: 300, minimumStockLevel: 50, supplierName: "RiseUp", isPerishable: false,
                originCountry: "India", storageInstructions: "Store in a dry place.", isActive: true),
            new Ingredient("Chocolate Chips", "Dark chocolate chips.", MeasurementUnit.Kilogram, new IngredientCost(5.00m),
                stockQuantity: 50, minimumStockLevel: 10, supplierName: "ChocoDelight", isPerishable: false,
                originCountry: "Belgium", storageInstructions: "Store in a cool, dry place.", isActive: true),
            new Ingredient("Yeast", "Active dry yeast.", MeasurementUnit.Gram, new IngredientCost(0.12m),
                stockQuantity: 300, minimumStockLevel: 30, supplierName: "BakersCo", isPerishable: false,
                originCountry: "USA", storageInstructions: "Store in a dry, cool place.", isActive: true),
            new Ingredient("Tomatoes", "Fresh ripe tomatoes.", MeasurementUnit.Kilogram, new IngredientCost(2.50m),
                stockQuantity: 150, minimumStockLevel: 20, supplierName: "Fresh Farms", isPerishable: true,
                originCountry: "Italy", storageInstructions: "Keep refrigerated.", isActive: true),
            new Ingredient("Cheese", "Mozzarella cheese.", MeasurementUnit.Kilogram, new IngredientCost(4.00m),
                stockQuantity: 100, minimumStockLevel: 10, supplierName: "DairyDelight", isPerishable: true,
                originCountry: "France", storageInstructions: "Refrigerate at 4°C.", isActive: true),
            new Ingredient("Pasta", "Dry spaghetti pasta.", MeasurementUnit.Kilogram, new IngredientCost(2.00m),
                stockQuantity: 200, minimumStockLevel: 20, supplierName: "PastaCo", isPerishable: false,
                originCountry: "Italy", storageInstructions: "Store in a dry place.", isActive: true),
            new Ingredient("Basil", "Fresh basil leaves.", MeasurementUnit.Gram, new IngredientCost(0.30m),
                stockQuantity: 150, minimumStockLevel: 10, supplierName: "HerbGarden", isPerishable: true,
                originCountry: "India", storageInstructions: "Keep refrigerated.", isActive: true),
            new Ingredient("Oil", "Olive oil.", MeasurementUnit.Liter, new IngredientCost(5.00m),
                stockQuantity: 50, minimumStockLevel: 10, supplierName: "OliveKing", isPerishable: false,
                originCountry: "Spain", storageInstructions: "Store in a cool, dark place.", isActive: true),
            new Ingredient("Garlic", "Fresh garlic cloves.", MeasurementUnit.Kilogram, new IngredientCost(3.00m),
                stockQuantity: 80, minimumStockLevel: 5, supplierName: "GarlicWorld", isPerishable: true,
                originCountry: "India", storageInstructions: "Store in a cool, dry place.", isActive: true),
            new Ingredient("Onion", "Yellow onion.", MeasurementUnit.Kilogram, new IngredientCost(1.50m),
                stockQuantity: 100, minimumStockLevel: 10, supplierName: "VeggieDelight", isPerishable: true,
                originCountry: "Spain", storageInstructions: "Store in a dry, ventilated place.", isActive: true),
            new Ingredient("Pepper", "Black pepper powder.", MeasurementUnit.Gram, new IngredientCost(0.50m),
                stockQuantity: 200, minimumStockLevel: 20, supplierName: "SpiceCo", isPerishable: false,
                originCountry: "India", storageInstructions: "Keep in an airtight container.", isActive: true),
            new Ingredient("Oregano", "Dried oregano leaves.", MeasurementUnit.Gram, new IngredientCost(0.30m),
                stockQuantity: 100, minimumStockLevel: 10, supplierName: "HerbHeaven", isPerishable: false,
                originCountry: "Greece", storageInstructions: "Keep in a dry place.", isActive: true),
            new Ingredient("Parmesan", "Grated Parmesan cheese.", MeasurementUnit.Kilogram, new IngredientCost(6.00m),
                stockQuantity: 50, minimumStockLevel: 5, supplierName: "DairyGold", isPerishable: true,
                originCountry: "Italy", storageInstructions: "Refrigerate at 4°C.", isActive: true)
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
            new Recipe("Vanilla Cake", "A soft and moist vanilla cake.", 20, 30, 8, "Mix all ingredients and bake.",
                DifficultyLevel.Medium, totalCost: 15.00m, yieldPerPortion: 1.88m),
            new Recipe("Chocolate Chip Cookies", "Classic cookies with chocolate chips.", 15, 25, 12, "Mix and bake.",
                DifficultyLevel.Easy, totalCost: 10.00m, yieldPerPortion: 0.83m),
            new Recipe("Spaghetti Bolognese", "Pasta with a rich meat sauce.", 10, 60, 4, "Cook pasta, prepare sauce.",
                DifficultyLevel.Medium, totalCost: 20.00m, yieldPerPortion: 5.00m),
            new Recipe("Tomato Basil Soup", "Fresh and hearty soup.", 15, 40, 6, "Simmer and blend ingredients.",
                DifficultyLevel.Easy, totalCost: 12.00m, yieldPerPortion: 2.00m),
            new Recipe("Garlic Bread", "Toasted bread with garlic butter.", 10, 15, 6, "Spread and bake.",
                DifficultyLevel.Easy, totalCost: 6.00m, yieldPerPortion: 1.00m),
            new Recipe("Caesar Salad", "Crisp salad with Caesar dressing.", 10, 0, 4, "Toss and serve.",
                DifficultyLevel.Easy, totalCost: 8.00m, yieldPerPortion: 2.00m),
            new Recipe("Margherita Pizza", "Classic pizza with tomato and cheese.", 30, 15, 8, "Prepare dough and bake.",
                DifficultyLevel.Hard, totalCost: 18.00m, yieldPerPortion: 2.25m),
            new Recipe("Pancakes", "Fluffy breakfast pancakes.", 10, 15, 4, "Mix and cook on griddle.",
                DifficultyLevel.Easy, totalCost: 5.00m, yieldPerPortion: 1.25m),
            new Recipe("Lasagna", "Layers of pasta, cheese, and sauce.", 40, 60, 8, "Assemble and bake.",
                DifficultyLevel.Hard, totalCost: 25.00m, yieldPerPortion: 3.13m),
            new Recipe("Brownies", "Rich chocolate brownies.", 15, 30, 12, "Mix and bake.",
                DifficultyLevel.Medium, totalCost: 12.00m, yieldPerPortion: 1.00m)
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
            new { Recipe = "Vanilla Cake", Ingredient = "Flour", Quantity = 200 },
            new { Recipe = "Vanilla Cake", Ingredient = "Sugar", Quantity = 150 },
            new { Recipe = "Vanilla Cake", Ingredient = "Eggs", Quantity = 3 },
            new { Recipe = "Chocolate Chip Cookies", Ingredient = "Flour", Quantity = 250 },
            new { Recipe = "Chocolate Chip Cookies", Ingredient = "Sugar", Quantity = 100 },
            new { Recipe = "Chocolate Chip Cookies", Ingredient = "Chocolate Chips", Quantity = 150 },
            new { Recipe = "Spaghetti Bolognese", Ingredient = "Pasta", Quantity = 200 },
            new { Recipe = "Spaghetti Bolognese", Ingredient = "Tomatoes", Quantity = 300 },
            new { Recipe = "Spaghetti Bolognese", Ingredient = "Garlic", Quantity = 20 },
            new { Recipe = "Tomato Basil Soup", Ingredient = "Tomatoes", Quantity = 500 },
            new { Recipe = "Tomato Basil Soup", Ingredient = "Basil", Quantity = 20 },
            new { Recipe = "Garlic Bread", Ingredient = "Flour", Quantity = 100 },
            new { Recipe = "Garlic Bread", Ingredient = "Butter", Quantity = 50 },
            new { Recipe = "Caesar Salad", Ingredient = "Salt", Quantity = 5 },
            new { Recipe = "Caesar Salad", Ingredient = "Oil", Quantity = 50 },
            new { Recipe = "Margherita Pizza", Ingredient = "Flour", Quantity = 300 },
            new { Recipe = "Margherita Pizza", Ingredient = "Cheese", Quantity = 200 },
            new { Recipe = "Margherita Pizza", Ingredient = "Tomatoes", Quantity = 150 },
            new { Recipe = "Pancakes", Ingredient = "Flour", Quantity = 150 },
            new { Recipe = "Pancakes", Ingredient = "Milk", Quantity = 200 },
            new { Recipe = "Lasagna", Ingredient = "Flour", Quantity = 250 },
            new { Recipe = "Lasagna", Ingredient = "Cheese", Quantity = 300 },
            new { Recipe = "Brownies", Ingredient = "Flour", Quantity = 150 },
            new { Recipe = "Brownies", Ingredient = "Sugar", Quantity = 200 },
            new { Recipe = "Brownies", Ingredient = "Chocolate Chips", Quantity = 100 }
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
