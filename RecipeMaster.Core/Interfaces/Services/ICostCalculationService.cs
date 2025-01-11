using RecipeMaster.Core.Entities;

namespace RecipeMaster.Core.Interfaces.Services;

public interface ICostCalculationService
{
    decimal CalculateTotalCost(Recipe recipe, IEnumerable<Ingredient> ingredients);
}
