using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeMaster.Application.Queries.Recipes;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class RecipeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var recipes = await _mediator.Send(new GetAllRecipesQuery());
        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var recipe = await _mediator.Send(new GetRecipeByIdQuery { Id = id });
        return recipe == null ? NotFound() : Ok(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecipeCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecipeDTO dto)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteRecipeCommand { Id = id });
        return NoContent();
    }
}
