using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeMaster.Application.Queries.Ingredients;
using RecipeMaster.Application.Commands.Ingredients;
using RecipeMaster.Application.DTOs;

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
        var ingredients = await _mediator.Send(new GetAllIngredientsQuery());
        return Ok(ingredients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ingredient = await _mediator.Send(new GetIngredientByIdQuery { Id = id });
        return ingredient == null ? NotFound() : Ok(ingredient);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIngredientCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIngredientDTO dto)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteIngredientCommand { Id = id });
        return NoContent();
    }
}
