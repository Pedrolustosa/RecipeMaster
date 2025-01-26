using RecipeMaster.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RecipeMaster.Infra.Persistence;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Infra.Repositories;

public class IngredientRepository(RecipeMasterDbContext context, ILogger<IngredientRepository> logger) : IIngredientRepository
{
    private readonly RecipeMasterDbContext _context = context;
    private readonly ILogger<IngredientRepository> _logger = logger;

    public async Task<Ingredient> GetByIdAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to retrieve ingredient with an empty ID.");
                throw new ArgumentException("ID cannot be empty.", nameof(id));
            }

            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
                _logger.LogWarning("Ingredient with ID {Id} not found.", id);

            return ingredient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ingredient with ID {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        try
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            _logger.LogInformation("Successfully retrieved {Count} ingredients.", ingredients.Count);
            return ingredients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ingredients.");
            throw;
        }
    }

    public async Task AddAsync(Ingredient ingredient)
    {
        try
        {
            if (ingredient == null)
            {
                _logger.LogWarning("Attempted to add a null ingredient.");
                throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");
            }

            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Ingredient added successfully with ID {Id}.", ingredient.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding ingredient.");
            throw;
        }
    }

    public async Task UpdateAsync(Ingredient ingredient)
    {
        try
        {
            if (ingredient == null)
            {
                _logger.LogWarning("Attempted to update a null ingredient.");
                throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");
            }

            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Ingredient updated successfully with ID {Id}.", ingredient.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ingredient with ID {Id}.", ingredient?.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete ingredient with an empty ID.");
                throw new ArgumentException("ID cannot be empty.", nameof(id));
            }

            var ingredient = await GetByIdAsync(id);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found for deletion.", id);
                return;
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Ingredient deleted successfully with ID {Id}.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ingredient with ID {Id}.", id);
            throw;
        }
    }

    public async Task<int> CountAsync()
    {
        try
        {
            var count = await _context.Ingredients.CountAsync();
            _logger.LogInformation("Successfully counted ingredients: {Count}.", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting ingredients.");
            throw;
        }
    }

    public async Task<List<(string Name, decimal Cost)>> GetMostExpensiveIngredientsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving top 5 most expensive ingredients...");

            var expensiveIngredients = await _context.Ingredients
                .OrderByDescending(i => i.Cost.Value)
                .Take(5)
                .Select(i => new { i.Name, i.Cost.Value })
                .ToListAsync();

            _logger.LogInformation("Successfully retrieved top 5 most expensive ingredients.");

            return expensiveIngredients.Select(i => (i.Name, (decimal)i.Value)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving most expensive ingredients.");
            throw;
        }
    }
}
