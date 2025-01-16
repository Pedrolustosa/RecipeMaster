using MediatR;
using RecipeMaster.Application.Queries.Dashboard;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class TotalRecipesQueryHandler : IRequestHandler<TotalRecipesQuery, int>
{
    private readonly IRecipeRepository _repository;

    public TotalRecipesQueryHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(TotalRecipesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.CountAsync();
    }
}
