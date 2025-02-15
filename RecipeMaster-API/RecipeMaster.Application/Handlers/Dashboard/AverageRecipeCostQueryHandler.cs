using MediatR;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Queries.Dashboard;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class AverageRecipeCostQueryHandler(IRecipeRepository repository, ILogger<AverageRecipeCostQueryHandler> logger) : IRequestHandler<AverageRecipeCostQuery, decimal>
{
    private readonly IRecipeRepository _repository = repository;
    private readonly ILogger<AverageRecipeCostQueryHandler> _logger = logger;

    public async Task<decimal> Handle(AverageRecipeCostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repository.GetAverageRecipeCostAsync();
            _logger.LogInformation("Successfully retrieved average recipe cost: {Result}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving average recipe cost.");
            throw;
        }
    }
}
