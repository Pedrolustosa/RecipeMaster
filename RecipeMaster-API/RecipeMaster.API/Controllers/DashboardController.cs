using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMaster.Application.Queries.Dashboard;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class DashboardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("total-recipes")]
    public async Task<IActionResult> GetTotalRecipes()
    {
        var result = await _mediator.Send(new TotalRecipesQuery());
        return Ok(result);
    }

    [HttpGet("total-ingredients")]
    public async Task<IActionResult> GetTotalIngredients()
    {
        var result = await _mediator.Send(new TotalIngredientsQuery());
        return Ok(result);
    }

    [HttpGet("average-recipe-cost")]
    public async Task<IActionResult> GetAverageRecipeCost()
    {
        var result = await _mediator.Send(new AverageRecipeCostQuery());
        return Ok(result);
    }

    [HttpGet("total-recipe-cost")]
    public async Task<IActionResult> GetTotalRecipeCost()
    {
        var result = await _mediator.Send(new TotalRecipeCostQuery());
        return Ok(result);
    }

    [HttpGet("most-expensive-ingredients")]
    public async Task<IActionResult> GetMostExpensiveIngredients()
    {
        var result = await _mediator.Send(new MostExpensiveIngredientsQuery());
        return Ok(result);
    }

    [HttpGet("most-used-ingredients")]
    public async Task<IActionResult> GetMostUsedIngredients()
    {
        var result = await _mediator.Send(new MostUsedIngredientsQuery());
        return Ok(result);
    }
}
