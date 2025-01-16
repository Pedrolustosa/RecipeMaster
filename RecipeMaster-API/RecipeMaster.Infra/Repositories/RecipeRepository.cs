using Microsoft.EntityFrameworkCore;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Infra.Persistence;

namespace RecipeMaster.Infra.Repositories;

public class RecipeRepository(RecipeMasterDbContext context) : IRecipeRepository
{
    private readonly RecipeMasterDbContext _context = context;

    public async Task<Recipe> GetByIdAsync(Guid id)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
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

    public async Task<int> CountAsync()
    {
        return await _context.Recipes.CountAsync();
    }

    public async Task<decimal> GetAverageRecipeCostAsync()
    {
        var recipeCosts = await _context.Recipes
            .Select(r => new
            {
                TotalCost = r.Ingredients.Sum(ri => (double)(ri.Quantity * ri.Ingredient.Cost.Value))
            })
            .ToListAsync();

        if (recipeCosts.Count == 0)
        {
            return 0;
        }

        return (decimal)recipeCosts.Average(rc => rc.TotalCost);
    }

    public async Task<decimal> GetTotalRecipeCostAsync()
    {
        var recipeCosts = await _context.Recipes
            .Select(r => r.Ingredients
                .Sum(ri => (double)(ri.Quantity * ri.Ingredient.Cost.Value)))
            .ToListAsync();

        return (decimal)recipeCosts.Sum();
    }

    public async Task<List<(string Name, int RecipeCount)>> GetMostUsedIngredientsAsync()
    {
        return await _context.Recipes
            .SelectMany(r => r.Ingredients)
            .GroupBy(ri => ri.Ingredient.Name)
            .Select(g => new { Name = g.Key, RecipeCount = g.Count() })
            .OrderByDescending(x => x.RecipeCount)
            .Take(5)
            .ToListAsync()
            .ContinueWith(t => t.Result.Select(x => (x.Name, x.RecipeCount)).ToList());
    }

}