using MediatR;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Dashboard;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class MostExpensiveIngredientsQueryHandler(IIngredientRepository repository, ILogger<MostExpensiveIngredientsQueryHandler> logger) : IRequestHandler<MostExpensiveIngredientsQuery, List<IngredientCostDTO>>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly ILogger<MostExpensiveIngredientsQueryHandler> _logger = logger;

    public async Task<List<IngredientCostDTO>> Handle(MostExpensiveIngredientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ingredients = await _repository.GetMostExpensiveIngredientsAsync();
            var dtos = ingredients
                .Select(i => new IngredientCostDTO { Name = i.Name, Cost = i.Cost })
                .ToList();

            _logger.LogInformation("Successfully retrieved {Count} most expensive ingredients.", dtos.Count);
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving most expensive ingredients.");
            throw;
        }
    }
}
