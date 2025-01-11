using MediatR;
using RecipeMaster.Application.Commands.Ingredients;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand, Unit>
{
    private readonly IIngredientRepository _repository;

    public UpdateIngredientCommandHandler(IIngredientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var ingredient = await _repository.GetByIdAsync(request.Id);

        if (ingredient == null)
        {
            throw new KeyNotFoundException("Ingredient not found");
        }

        ingredient = new Ingredient(request.Name, Enum.Parse<MeasurementUnit>(request.Unit), new IngredientCost(request.Cost));
        await _repository.UpdateAsync(ingredient);

        return Unit.Value;
    }
}
