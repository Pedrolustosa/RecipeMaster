using MediatR;
using RecipeMaster.Application.Commands.Ingredients;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, Guid>
{
    private readonly IIngredientRepository _repository;

    public CreateIngredientCommandHandler(IIngredientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        var ingredient = new Ingredient(request.Name, Enum.Parse<MeasurementUnit>(request.Unit), new IngredientCost(request.Cost));
        await _repository.AddAsync(ingredient);
        return ingredient.Id;
    }
}
