using Microsoft.EntityFrameworkCore;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Exceptions;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Infra.Persistence;
using Serilog;

namespace RecipeMaster.Infra.Repositories;

public class RecipeRepository(RecipeMasterDbContext context) : IRecipeRepository
{
    private readonly RecipeMasterDbContext _context = context;

    public async Task<Recipe> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException($"Recipe with ID {id} not found.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving recipe by ID: {RecipeId}", id);
            throw new RepositoryException("An error occurred while retrieving the recipe.", ex);
        }
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        try
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving all recipes.");
            throw new RepositoryException("An error occurred while retrieving all recipes.", ex);
        }
    }

    public async Task AddAsync(Recipe recipe)
    {
        try
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error adding a new recipe.");
            throw new RepositoryException("An error occurred while adding the recipe.", ex);
        }
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        try
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating the recipe with ID: {RecipeId}", recipe.Id);
            throw new RepositoryException("An error occurred while updating the recipe.", ex);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var recipe = await GetByIdAsync(id);
            if (recipe == null)
                throw new NotFoundException($"Recipe with ID {id} not found.");

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting the recipe with ID: {RecipeId}", id);
            throw new RepositoryException("An error occurred while deleting the recipe.", ex);
        }
    }

    public async Task<int> CountAsync()
    {
        try
        {
            return await _context.Recipes.CountAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error counting recipes.");
            throw new RepositoryException("An error occurred while counting recipes.", ex);
        }
    }

    public async Task<decimal> GetAverageRecipeCostAsync()
    {
        try
        {
            var recipeCosts = await _context.Recipes
                .Select(r => new
                {
                    TotalCost = r.Ingredients
                        .Sum(ri => (double)(ri.Quantity * (ri.Ingredient.Cost.Value != null ? ri.Ingredient.Cost.Value : 0)))
                })
                .ToListAsync();

            if (!recipeCosts.Any())
                return 0m;

            return Math.Round((decimal)recipeCosts.Average(rc => rc.TotalCost), 2);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error calculating average recipe cost.");
            throw new RepositoryException("An error occurred while calculating the average recipe cost.", ex);
        }
    }


    public async Task<decimal> GetTotalRecipeCostAsync()
    {
        try
        {
            var recipeCosts = await _context.Recipes
                .Select(r => r.Ingredients
                    .Sum(ri => (double)(ri.Quantity * (ri.Ingredient.Cost.Value != null ? ri.Ingredient.Cost.Value : 0))))
                .ToListAsync();

            var totalCost = recipeCosts.Any() ? (decimal)recipeCosts.Sum() : 0m;
            return Math.Round(totalCost, 2);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error calculating total recipe cost.");
            throw new RepositoryException("An error occurred while calculating the total recipe cost.", ex);
        }
    }

    public async Task<List<(string Name, int RecipeCount)>> GetMostUsedIngredientsAsync()
    {
        try
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
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving the most used ingredients.");
            throw new RepositoryException("An error occurred while retrieving the most used ingredients.", ex);
        }
    }
}
