using MediatR;
using RecipeMaster.Application.Queries.Dashboard;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class AverageRecipeCostQueryHandler : IRequestHandler<AverageRecipeCostQuery, decimal>
{
    private readonly IRecipeRepository _repository;

    public AverageRecipeCostQueryHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<decimal> Handle(AverageRecipeCostQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAverageRecipeCostAsync();
    }
}