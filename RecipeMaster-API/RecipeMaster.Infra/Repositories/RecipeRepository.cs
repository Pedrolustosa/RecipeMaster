using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using RecipeMaster.Infra.Persistence;
using RecipeMaster.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace RecipeMaster.Infra.Repositories;

public class RecipeRepository(RecipeMasterDbContext context, ILogger<RecipeRepository> logger) : IRecipeRepository
{
    private readonly RecipeMasterDbContext _context = context;
    private readonly ILogger<RecipeRepository> _logger = logger;

    public async Task<Recipe> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Attempting to retrieve recipe with ID: {RecipeId}", id);
        try
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException("Recipe", id);

            _logger.LogInformation("Successfully retrieved recipe with ID: {RecipeId}", id);
            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recipe by ID: {RecipeId}", id);
            throw new RepositoryException("An error occurred while retrieving the recipe.", ex);
        }
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        _logger.LogInformation("Attempting to retrieve all recipes.");
        try
        {
            var recipes = await _context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();

            _logger.LogInformation("Successfully retrieved all recipes. Total count: {Count}", recipes.Count);
            return recipes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all recipes.");
            throw new RepositoryException("An error occurred while retrieving all recipes.", ex);
        }
    }

    public async Task AddAsync(Recipe recipe)
    {
        _logger.LogInformation("Attempting to add a new recipe: {RecipeName}", recipe.Name);
        try
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully added recipe: {RecipeName} with ID: {RecipeId}", recipe.Name, recipe.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding a new recipe: {RecipeName}", recipe.Name);
            throw new RepositoryException("An error occurred while adding the recipe.", ex);
        }
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        _logger.LogInformation("Attempting to update recipe with ID: {RecipeId}", recipe.Id);
        try
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated recipe with ID: {RecipeId}", recipe.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating recipe with ID: {RecipeId}", recipe.Id);
            throw new RepositoryException("An error occurred while updating the recipe.", ex);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Attempting to delete recipe with ID: {RecipeId}", id);
        try
        {
            var recipe = await GetByIdAsync(id) ?? throw new NotFoundException("Recipe", id);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted recipe with ID: {RecipeId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting recipe with ID: {RecipeId}", id);
            throw new RepositoryException("An error occurred while deleting the recipe.", ex);
        }
    }

    public async Task<int> CountAsync()
    {
        _logger.LogInformation("Attempting to count all recipes.");
        try
        {
            var count = await _context.Recipes.CountAsync();
            _logger.LogInformation("Successfully counted recipes. Total count: {Count}", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting recipes.");
            throw new RepositoryException("An error occurred while counting recipes.", ex);
        }
    }

    public async Task<decimal> GetAverageRecipeCostAsync()
    {
        _logger.LogInformation("Attempting to calculate average recipe cost.");
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
            {
                _logger.LogWarning("No recipes found to calculate average cost.");
                return 0m;
            }

            var averageCost = Math.Round((decimal)recipeCosts.Average(rc => rc.TotalCost), 2);
            _logger.LogInformation("Successfully calculated average recipe cost: {AverageCost}", averageCost);
            return averageCost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating average recipe cost.");
            throw new RepositoryException("An error occurred while calculating the average recipe cost.", ex);
        }
    }

    public async Task<decimal> GetTotalRecipeCostAsync()
    {
        _logger.LogInformation("Attempting to calculate total recipe cost.");
        try
        {
            var recipeCosts = await _context.Recipes
                .Select(r => r.Ingredients
                    .Sum(ri => (double)(ri.Quantity * (ri.Ingredient.Cost.Value != null ? ri.Ingredient.Cost.Value : 0))))
                .ToListAsync();

            var totalCost = recipeCosts.Any() ? (decimal)recipeCosts.Sum() : 0m;
            _logger.LogInformation("Successfully calculated total recipe cost: {TotalCost}", totalCost);
            return Math.Round(totalCost, 2);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total recipe cost.");
            throw new RepositoryException("An error occurred while calculating the total recipe cost.", ex);
        }
    }

    public async Task<List<(string Name, int RecipeCount)>> GetMostUsedIngredientsAsync()
    {
        _logger.LogInformation("Attempting to retrieve the most used ingredients.");
        try
        {
            var mostUsedIngredients = await _context.Recipes
                .SelectMany(r => r.Ingredients)
                .GroupBy(ri => ri.Ingredient.Name)
                .Select(g => new { Name = g.Key, RecipeCount = g.Count() })
                .OrderByDescending(x => x.RecipeCount)
                .Take(5)
                .ToListAsync()
                .ContinueWith(t => t.Result.Select(x => (x.Name, x.RecipeCount)).ToList());

            _logger.LogInformation("Successfully retrieved the most used ingredients.");
            return mostUsedIngredients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving the most used ingredients.");
            throw new RepositoryException("An error occurred while retrieving the most used ingredients.", ex);
        }
    }
}
