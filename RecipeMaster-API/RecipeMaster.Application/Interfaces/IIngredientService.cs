using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Services.Interfaces;

public interface IIngredientService
{
    Task<IEnumerable<IngredientDTO>> GetAllAsync();
    Task<IngredientDTO> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(IngredientDTO dto);
    Task UpdateAsync(Guid id, UpdateIngredientDTO dto);
    Task DeleteAsync(Guid id);
}
