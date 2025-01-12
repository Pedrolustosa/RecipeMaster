using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecipeMaster.API.Exceptions;
using RecipeMaster.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using RecipeMaster.Application.Queries.Recipes;
using RecipeMaster.Application.Commands.Recipes;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class RecipeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var recipes = await _mediator.Send(new GetAllRecipesQuery());
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
            var recipe = await _mediator.Send(new GetRecipeByIdQuery { Id = id });

            return recipe == null ? throw new NotFoundException("Recipe", id) : (IActionResult)Ok(recipe);
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
    public async Task<IActionResult> Create([FromBody] CreateRecipeCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
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
            var command = new UpdateRecipeCommand
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Ingredients = dto.Ingredients.Select(i => new UpdateRecipeCommand.IngredientDto
                {
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity
                }).ToList()
            };

            await _mediator.Send(command);
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
            await _mediator.Send(new DeleteRecipeCommand { Id = id });
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
