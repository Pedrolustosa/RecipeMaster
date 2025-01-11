using RecipeMaster.Core.Entities;
using RecipeMaster.Core.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeMaster.Infra.Persistence;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<RecipeMasterDbContext>();

        if (context.Ingredients.Any() || context.Recipes.Any())
            return;

        // Add Ingredients
        var salt = new Ingredient("Salt", MeasurementUnit.Gram, new IngredientCost(0.05m));
        var sugar = new Ingredient("Sugar", MeasurementUnit.Gram, new IngredientCost(0.10m));

        context.Ingredients.AddRange(salt, sugar);

        // Add Recipes
        var cake = new Recipe("Cake", "A simple cake recipe");
        cake.AddIngredient(new RecipeIngredient(cake.Id, sugar.Id, 200));
        cake.AddIngredient(new RecipeIngredient(cake.Id, salt.Id, 5));

        context.Recipes.Add(cake);

        context.SaveChanges();
    }
}
