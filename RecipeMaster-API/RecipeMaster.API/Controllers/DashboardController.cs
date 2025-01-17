using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecipeMaster.API.Exceptions;
using Microsoft.AspNetCore.Authorization;
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
        try
        {
            var result = await _mediator.Send(new TotalRecipesQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving total recipes.", ex.Message);
        }
    }

    [HttpGet("total-ingredients")]
    public async Task<IActionResult> GetTotalIngredients()
    {
        try
        {
            var result = await _mediator.Send(new TotalIngredientsQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving total ingredients.", ex.Message);
        }
    }

    [HttpGet("average-recipe-cost")]
    public async Task<IActionResult> GetAverageRecipeCost()
    {
        try
        {
            var result = await _mediator.Send(new AverageRecipeCostQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving the average recipe cost.", ex.Message);
        }
    }

    [HttpGet("total-recipe-cost")]
    public async Task<IActionResult> GetTotalRecipeCost()
    {
        try
        {
            var result = await _mediator.Send(new TotalRecipeCostQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving the total recipe cost.", ex.Message);
        }
    }

    [HttpGet("most-expensive-ingredients")]
    public async Task<IActionResult> GetMostExpensiveIngredients()
    {
        try
        {
            var result = await _mediator.Send(new MostExpensiveIngredientsQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving the most expensive ingredients.", ex.Message);
        }
    }

    [HttpGet("most-used-ingredients")]
    public async Task<IActionResult> GetMostUsedIngredients()
    {
        try
        {
            var result = await _mediator.Send(new MostUsedIngredientsQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving the most used ingredients.", ex.Message);
        }
    }
}
