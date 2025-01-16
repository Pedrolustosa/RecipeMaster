using RecipeMaster.Core.Entities;

namespace RecipeMaster.Core.Interfaces.Repositories;

public interface IRecipeRepository
{
    Task<Recipe> GetByIdAsync(Guid id);
    Task<IEnumerable<Recipe>> GetAllAsync();
    Task AddAsync(Recipe recipe);
    Task UpdateAsync(Recipe recipe);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<decimal> GetAverageRecipeCostAsync();
    Task<decimal> GetTotalRecipeCostAsync();
    Task<List<(string Name, int RecipeCount)>> GetMostUsedIngredientsAsync();
}