using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Interfaces;

public interface IDashboardService
{
    Task<int> GetTotalRecipesAsync();
    Task<int> GetTotalIngredientsAsync();
    Task<decimal> GetAverageRecipeCostAsync();
    Task<decimal> GetTotalRecipeCostAsync();
    Task<IEnumerable<IngredientCostDTO>> GetMostExpensiveIngredientsAsync();
    Task<IEnumerable<IngredientUsageDTO>> GetMostUsedIngredientsAsync(); 
}
