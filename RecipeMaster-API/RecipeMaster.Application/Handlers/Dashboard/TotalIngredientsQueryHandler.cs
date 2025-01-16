using MediatR;
using RecipeMaster.Application.Queries.Dashboard;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Dashboard;

public class TotalIngredientsQueryHandler : IRequestHandler<TotalIngredientsQuery, int>
{
    private readonly IIngredientRepository _repository;

    public TotalIngredientsQueryHandler(IIngredientRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(TotalIngredientsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.CountAsync();
    }
}
