using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Commands.Ingredients;

namespace RecipeMaster.Application.Services.Interfaces;

public interface IIngredientService
{
    Task<IEnumerable<IngredientDTO>> GetAllAsync();
    Task<IngredientDTO> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(IngredientDTO command);
    Task UpdateAsync(Guid id, UpdateIngredientDTO dto);
    Task DeleteAsync(Guid id);
}
