using RecipeMaster.Core.Entities;

namespace RecipeMaster.Core.Interfaces.Repositories;

public interface IRecipeRepository
{
    Task<Recipe> GetByIdAsync(Guid id);
    Task<IEnumerable<Recipe>> GetAllAsync();
    Task AddAsync(Recipe recipe);
    Task UpdateAsync(Recipe recipe);
    Task DeleteAsync(Guid id);
}
