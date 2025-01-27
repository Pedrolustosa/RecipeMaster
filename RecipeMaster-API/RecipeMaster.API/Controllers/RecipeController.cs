using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeMaster.Application.Services.Interfaces;
using RecipeMaster.Core.Exceptions;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecipeController(IRecipeService recipeService) : ControllerBase
{
    private readonly IRecipeService _recipeService = recipeService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var recipes = await _recipeService.GetAllAsync();
            return Ok(recipes);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving recipes.", ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            return recipe == null
                ? throw new NotFoundException("Recipe", id)
                : (IActionResult)Ok(recipe);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving the recipe.", ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RecipeDTO dto)
    {
        try
        {
            var id = await _recipeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(new { error = ex.Message, details = ex.Errors });
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while creating the recipe.", ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecipeDTO dto)
    {
        try
        {
            await _recipeService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(new { error = ex.Message, details = ex.Errors });
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while updating the recipe.", ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _recipeService.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while deleting the recipe.", ex.Message);
        }
    }
}
