using RecipeMaster.Core.Entities;
using Microsoft.EntityFrameworkCore;
using RecipeMaster.Infra.Persistence;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Infra.Repositories;

public class IngredientRepository(RecipeMasterDbContext context) : IIngredientRepository
{
    private readonly RecipeMasterDbContext _context = context;

    public async Task<Ingredient> GetByIdAsync(Guid id)
    {
        return await _context.Ingredients.FindAsync(id);
    }

    public async Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        return await _context.Ingredients.ToListAsync();
    }

    public async Task AddAsync(Ingredient ingredient)
    {
        await _context.Ingredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ingredient ingredient)
    {
        _context.Ingredients.Update(ingredient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var ingredient = await GetByIdAsync(id);
        if (ingredient != null)
        {
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }
    }
}
