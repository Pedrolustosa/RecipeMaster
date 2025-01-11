using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Interfaces.Services;

namespace RecipeMaster.Application.Services;

public class CostCalculationService : ICostCalculationService
{
    public decimal CalculateTotalCost(Recipe recipe, IEnumerable<Ingredient> ingredients)
    {
        decimal totalCost = 0;
        foreach (var recipeIngredient in recipe.Ingredients)
        {
            var ingredient = ingredients.FirstOrDefault(i => i.Id == recipeIngredient.IngredientId);
            if (ingredient != null)
                totalCost += recipeIngredient.Quantity * ingredient.Cost.Value;
        }
        return totalCost;
    }
}
