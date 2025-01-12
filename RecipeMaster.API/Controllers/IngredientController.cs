using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecipeMaster.API.Exceptions;
using RecipeMaster.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using RecipeMaster.Application.Queries.Ingredients;
using RecipeMaster.Application.Commands.Ingredients;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class IngredientController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var ingredients = await _mediator.Send(new GetAllIngredientsQuery());
            return Ok(ingredients);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving ingredients.", ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var ingredient = await _mediator.Send(new GetIngredientByIdQuery { Id = id });

            return ingredient == null ? throw new NotFoundException("Ingredient", id) : (IActionResult)Ok(ingredient);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving the ingredient.", ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIngredientCommand command)
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
            throw new InternalServerException("An error occurred while creating the ingredient.", ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIngredientDTO dto)
    {
        try
        {
            var command = new UpdateIngredientCommand
            {
                Id = id,
                Name = dto.Name,
                Unit = dto.Unit,
                Cost = dto.Cost
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
            throw new InternalServerException("An error occurred while updating the ingredient.", ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteIngredientCommand { Id = id });
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while deleting the ingredient.", ex.Message);
        }
    }
}
