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
            context.Database.EnsureCreated();
            SeedRoles(roleManager);
            SeedUsers(userManager);
            if (!context.Ingredients.Any() && !context.Recipes.Any())
                SeedIngredientsAndRecipes(context);
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                    roleManager.CreateAsync(new ApplicationRole { Name = role }).Wait();
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
                        userManager.AddToRoleAsync(user, userData.Role).Wait();
                }
            }
        }

        private static void SeedIngredientsAndRecipes(RecipeMasterDbContext context)
        {
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
                    context.Ingredients.Add(ingredient);
            }

            context.SaveChanges();

            var recipes = new Dictionary<string, Recipe>
            {
                { "Vanilla Cake", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 8, 1.88m, 8, 15.00m) },
                { "Chocolate Chip Cookies", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 12, 0.83m, 12, 10.00m) },
                { "Spaghetti Bolognese", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 4, 5.00m, 4, 20.00m) },
                { "Tomato Basil Soup", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 6, 2.00m, 6, 12.00m) },
                { "Garlic Bread", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 6, 1.00m, 6, 6.00m) },
                { "Caesar Salad", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 4, 2.00m, 4, 8.00m) },
                { "Margherita Pizza", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 8, 2.25m, 8, 18.00m) },
                { "Pancakes", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 4, 1.25m, 4, 5.00m) },
                { "Lasagna", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 8, 3.13m, 8, 25.00m) },
                { "Brownies", new Recipe(new List<RecipeIngredient>().AsReadOnly(), 12, 1.00m, 12, 12.00m) }
            };

            foreach (var recipe in recipes.Values)
            {
                context.Recipes.Add(recipe);
            }
            context.SaveChanges();
            var ingredientDict = context.Ingredients.ToDictionary(i => i.Name, i => i.Id);
            var recipeIngredients = new[]
            {
                new { RecipeKey = "Vanilla Cake", Ingredient = "Flour", Quantity = 200 },
                new { RecipeKey = "Vanilla Cake", Ingredient = "Sugar", Quantity = 150 },
                new { RecipeKey = "Vanilla Cake", Ingredient = "Eggs", Quantity = 3 },
                new { RecipeKey = "Chocolate Chip Cookies", Ingredient = "Flour", Quantity = 250 },
                new { RecipeKey = "Chocolate Chip Cookies", Ingredient = "Sugar", Quantity = 100 },
                new { RecipeKey = "Chocolate Chip Cookies", Ingredient = "Chocolate Chips", Quantity = 150 },
                new { RecipeKey = "Spaghetti Bolognese", Ingredient = "Pasta", Quantity = 200 },
                new { RecipeKey = "Spaghetti Bolognese", Ingredient = "Tomatoes", Quantity = 300 },
                new { RecipeKey = "Spaghetti Bolognese", Ingredient = "Garlic", Quantity = 20 },
                new { RecipeKey = "Tomato Basil Soup", Ingredient = "Tomatoes", Quantity = 500 },
                new { RecipeKey = "Tomato Basil Soup", Ingredient = "Basil", Quantity = 20 },
                new { RecipeKey = "Garlic Bread", Ingredient = "Flour", Quantity = 100 },
                new { RecipeKey = "Garlic Bread", Ingredient = "Butter", Quantity = 50 },
                new { RecipeKey = "Caesar Salad", Ingredient = "Salt", Quantity = 5 },
                new { RecipeKey = "Caesar Salad", Ingredient = "Oil", Quantity = 50 },
                new { RecipeKey = "Margherita Pizza", Ingredient = "Flour", Quantity = 300 },
                new { RecipeKey = "Margherita Pizza", Ingredient = "Cheese", Quantity = 200 },
                new { RecipeKey = "Margherita Pizza", Ingredient = "Tomatoes", Quantity = 150 },
                new { RecipeKey = "Pancakes", Ingredient = "Flour", Quantity = 150 },
                new { RecipeKey = "Pancakes", Ingredient = "Milk", Quantity = 200 },
                new { RecipeKey = "Lasagna", Ingredient = "Flour", Quantity = 250 },
                new { RecipeKey = "Lasagna", Ingredient = "Cheese", Quantity = 300 },
                new { RecipeKey = "Brownies", Ingredient = "Flour", Quantity = 150 },
                new { RecipeKey = "Brownies", Ingredient = "Sugar", Quantity = 200 },
                new { RecipeKey = "Brownies", Ingredient = "Chocolate Chips", Quantity = 100 }
            };

            foreach (var ri in recipeIngredients)
            {
                if (!context.RecipeIngredients.Any(r =>
                        r.RecipeId == recipes[ri.RecipeKey].Id && r.IngredientId == ingredientDict[ri.Ingredient]))
                {
                    context.RecipeIngredients.Add(new RecipeIngredient(recipes[ri.RecipeKey].Id, ingredientDict[ri.Ingredient],
                        ri.Quantity));
                }
            }
            context.SaveChanges();
        }
    }
}
