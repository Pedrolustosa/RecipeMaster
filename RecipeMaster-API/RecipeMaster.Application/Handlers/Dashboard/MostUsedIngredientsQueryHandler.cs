using MediatR;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Queries.Dashboard;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class MostUsedIngredientsQueryHandler(IRecipeRepository repository, ILogger<MostUsedIngredientsQueryHandler> logger) : IRequestHandler<MostUsedIngredientsQuery, List<IngredientUsageDTO>>
{
    private readonly IRecipeRepository _repository = repository;
    private readonly ILogger<MostUsedIngredientsQueryHandler> _logger = logger;

    public async Task<List<IngredientUsageDTO>> Handle(MostUsedIngredientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ingredients = await _repository.GetMostUsedIngredientsAsync();
            var dtos = ingredients
                .Select(i => new IngredientUsageDTO
                {
                    Name = i.Name,
                    RecipeCount = i.RecipeCount
                })
                .ToList();

            _logger.LogInformation("Successfully retrieved {Count} most used ingredients.", dtos.Count);
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving most used ingredients.");
            throw;
        }
    }
}
