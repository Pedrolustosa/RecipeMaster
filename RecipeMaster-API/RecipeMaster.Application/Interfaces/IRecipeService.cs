using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Services.Interfaces;

public interface IRecipeService
{
    Task<IEnumerable<RecipeDTO>> GetAllAsync();
    Task<RecipeDTO> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(RecipeDTO dto);
    Task UpdateAsync(Guid id, UpdateRecipeDTO dto);
    Task DeleteAsync(Guid id);
}
