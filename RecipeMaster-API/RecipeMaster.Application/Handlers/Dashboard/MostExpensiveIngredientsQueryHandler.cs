using MediatR;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Dashboard;
using RecipeMaster.Core.Interfaces.Repositories;

public class MostExpensiveIngredientsQueryHandler : IRequestHandler<MostExpensiveIngredientsQuery, List<IngredientCostDTO>>
{
    private readonly IIngredientRepository _repository;

    public MostExpensiveIngredientsQueryHandler(IIngredientRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<IngredientCostDTO>> Handle(MostExpensiveIngredientsQuery request, CancellationToken cancellationToken)
    {
        var ingredients = await _repository.GetMostExpensiveIngredientsAsync();

        return ingredients
            .Select(i => new IngredientCostDTO { Name = i.Name, Cost = i.Cost })
            .ToList();
    }
}
