using MediatR;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Queries.Dashboard;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class TotalIngredientsQueryHandler(IIngredientRepository repository, ILogger<TotalIngredientsQueryHandler> logger) : IRequestHandler<TotalIngredientsQuery, int>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly ILogger<TotalIngredientsQueryHandler> _logger = logger;

    public async Task<int> Handle(TotalIngredientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var count = await _repository.CountAsync();
            _logger.LogInformation("Successfully retrieved total ingredients count: {Count}", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving total ingredients count.");
            throw;
        }
    }
}
