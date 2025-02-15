using MediatR;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Queries.Dashboard;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class TotalRecipeCostQueryHandler(IRecipeRepository repository, ILogger<TotalRecipeCostQueryHandler> logger) : IRequestHandler<TotalRecipeCostQuery, decimal>
{
    private readonly IRecipeRepository _repository = repository;
    private readonly ILogger<TotalRecipeCostQueryHandler> _logger = logger;

    public async Task<decimal> Handle(TotalRecipeCostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var totalCost = await _repository.GetTotalRecipeCostAsync();
            _logger.LogInformation("Successfully retrieved total recipe cost: {TotalCost}", totalCost);
            return totalCost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving total recipe cost.");
            throw;
        }
    }
}
