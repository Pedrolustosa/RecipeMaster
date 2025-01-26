using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RecipeMaster.API.Exceptions;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Services.Interfaces;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IngredientController(IIngredientService ingredientService, ILogger<IngredientController> logger) : ControllerBase
{
    private readonly IIngredientService _ingredientService = ingredientService;
    private readonly ILogger<IngredientController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all ingredients...");
        try
        {
            var ingredients = await _ingredientService.GetAllAsync();
            _logger.LogInformation("Successfully retrieved all ingredients.");
            return Ok(ingredients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all ingredients.");
            throw new InternalServerException("An error occurred while retrieving ingredients.", ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Getting ingredient with ID {Id}...", id);
        try
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);

            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found.", id);
                throw new NotFoundException("Ingredient", id);
            }

            _logger.LogInformation("Successfully retrieved ingredient with ID {Id}.", id);
            return Ok(ingredient);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "NotFoundException while retrieving ingredient with ID {Id}.", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving ingredient with ID {Id}.", id);
            throw new InternalServerException("An error occurred while retrieving the ingredient.", ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IngredientDTO dto)
    {
        _logger.LogInformation("Creating a new ingredient...");
        try
        {
            var id = await _ingredientService.CreateAsync(dto);
            _logger.LogInformation("Successfully created ingredient with ID {Id}.", id);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "ValidationException while creating a new ingredient.");
            return UnprocessableEntity(new { error = ex.Message, details = ex.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a new ingredient.");
            throw new InternalServerException("An error occurred while creating the ingredient.", ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIngredientDTO dto)
    {
        _logger.LogInformation("Updating ingredient with ID {Id}...", id);
        try
        {
            await _ingredientService.UpdateAsync(id, dto);
            _logger.LogInformation("Successfully updated ingredient with ID {Id}.", id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "NotFoundException while updating ingredient with ID {Id}.", id);
            return NotFound(new { error = ex.Message });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "ValidationException while updating ingredient with ID {Id}.", id);
            return UnprocessableEntity(new { error = ex.Message, details = ex.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating ingredient with ID {Id}.", id);
            throw new InternalServerException("An error occurred while updating the ingredient.", ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deleting ingredient with ID {Id}...", id);
        try
        {
            await _ingredientService.DeleteAsync(id);
            _logger.LogInformation("Successfully deleted ingredient with ID {Id}.", id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "NotFoundException while deleting ingredient with ID {Id}.", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting ingredient with ID {Id}.", id);
            throw new InternalServerException("An error occurred while deleting the ingredient.", ex.Message);
        }
    }
}
