using MediatR;
using RecipeMaster.Application.Queries.Dashboard;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class TotalRecipeCostQueryHandler : IRequestHandler<TotalRecipeCostQuery, decimal>
{
    private readonly IRecipeRepository _repository;

    public TotalRecipeCostQueryHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<decimal> Handle(TotalRecipeCostQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetTotalRecipeCostAsync();
    }
}
