using MediatR;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Queries.Dashboard;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class TotalRecipesQueryHandler(IRecipeRepository repository, ILogger<TotalRecipesQueryHandler> logger) : IRequestHandler<TotalRecipesQuery, int>
{
    private readonly IRecipeRepository _repository = repository;
    private readonly ILogger<TotalRecipesQueryHandler> _logger = logger;

    public async Task<int> Handle(TotalRecipesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var count = await _repository.CountAsync();
            _logger.LogInformation("Successfully retrieved total recipes count: {Count}", count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving total recipes count.");
            throw;
        }
    }
}
