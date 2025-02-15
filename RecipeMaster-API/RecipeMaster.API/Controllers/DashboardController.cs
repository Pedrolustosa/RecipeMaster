using Microsoft.AspNetCore.Mvc;
using RecipeMaster.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using RecipeMaster.Application.Interfaces;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [HttpGet("total-recipes")]
    public async Task<IActionResult> GetTotalRecipes()
    {
        try
        {
            var result = await _dashboardService.GetTotalRecipesAsync();
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
            var result = await _dashboardService.GetTotalIngredientsAsync();
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
            var result = await _dashboardService.GetAverageRecipeCostAsync();
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
            var result = await _dashboardService.GetTotalRecipeCostAsync();
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
            var result = await _dashboardService.GetMostExpensiveIngredientsAsync();
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
            var result = await _dashboardService.GetMostUsedIngredientsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while retrieving the most used ingredients.", ex.Message);
        }
    }
}
