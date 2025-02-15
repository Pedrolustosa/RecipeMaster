using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.Exceptions;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Interfaces;
using RecipeMaster.Application.Queries.Dashboard;

namespace RecipeMaster.Application.Services;

public class DashboardService(IMediator mediator, IMapper mapper, ILogger<DashboardService> logger) : IDashboardService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<DashboardService> _logger = logger;

    public async Task<int> GetTotalRecipesAsync()
    {
        try
        {
            return await _mediator.Send(new TotalRecipesQuery());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving total recipes.");
            throw new InternalServerException("An error occurred while retrieving total recipes.", ex.Message);
        }
    }

    public async Task<int> GetTotalIngredientsAsync()
    {
        try
        {
            return await _mediator.Send(new TotalIngredientsQuery());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving total ingredients.");
            throw new InternalServerException("An error occurred while retrieving total ingredients.", ex.Message);
        }
    }

    public async Task<decimal> GetAverageRecipeCostAsync()
    {
        try
        {
            return await _mediator.Send(new AverageRecipeCostQuery());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving average recipe cost.");
            throw new InternalServerException("An error occurred while retrieving the average recipe cost.", ex.Message);
        }
    }

    public async Task<decimal> GetTotalRecipeCostAsync()
    {
        try
        {
            return await _mediator.Send(new TotalRecipeCostQuery());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving total recipe cost.");
            throw new InternalServerException("An error occurred while retrieving the total recipe cost.", ex.Message);
        }
    }

    public async Task<IEnumerable<IngredientCostDTO>> GetMostExpensiveIngredientsAsync()
    {
        try
        {
            var ingredientCostDtoList = await _mediator.Send(new MostExpensiveIngredientsQuery());
            return _mapper.Map<IEnumerable<IngredientCostDTO>>(ingredientCostDtoList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving most expensive ingredients.");
            throw new InternalServerException("An error occurred while retrieving the most expensive ingredients.", ex.Message);
        }
    }

    public async Task<IEnumerable<IngredientUsageDTO>> GetMostUsedIngredientsAsync()
    {
        try
        {
            var ingredientUsageDtoList = await _mediator.Send(new MostUsedIngredientsQuery());
            return _mapper.Map<IEnumerable<IngredientUsageDTO>>(ingredientUsageDtoList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving most used ingredients.");
            throw new InternalServerException("An error occurred while retrieving the most used ingredients.", ex.Message);
        }
    }
}
