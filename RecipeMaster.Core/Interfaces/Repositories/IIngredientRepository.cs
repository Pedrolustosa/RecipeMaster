using RecipeMaster.Core.Entities;

namespace RecipeMaster.Core.Interfaces.Repositories;

public interface IIngredientRepository
{
    Task<Ingredient> GetByIdAsync(Guid id);
    Task<IEnumerable<Ingredient>> GetAllAsync();
    Task AddAsync(Ingredient ingredient);
    Task UpdateAsync(Ingredient ingredient);
    Task DeleteAsync(Guid id);
}
