using RecipeMaster.Core.Entities;
using Microsoft.EntityFrameworkCore;
using RecipeMaster.Infra.Persistence;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Infra.Repositories;

public class RecipeRepository(RecipeMasterDbContext context) : IRecipeRepository
{
    private readonly RecipeMasterDbContext _context = context;

    public async Task<Recipe> GetByIdAsync(Guid id)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.IngredientId)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .ToListAsync();
    }

    public async Task AddAsync(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var recipe = await GetByIdAsync(id);
        if (recipe != null)
        {
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
