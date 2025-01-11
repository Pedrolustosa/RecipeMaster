using RecipeMaster.Core.Interfaces.Services;

namespace RecipeMaster.Application.Services;

public class CostCalculationService : ICostCalculationService
{
    public decimal CalculateTotalCost(Core.Entities.Recipe recipe, IEnumerable<Core.Entities.Ingredient> ingredients)
    {
        return ingredients.Sum(i => i.Cost.Value * recipe.Ingredients
            .Where(ri => ri.IngredientId == i.Id)
            .Sum(ri => ri.Quantity));
    }
}
