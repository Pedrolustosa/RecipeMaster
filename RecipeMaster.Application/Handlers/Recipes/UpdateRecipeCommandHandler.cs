using MediatR;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, Unit>
{
    private readonly IRecipeRepository _repository;

    public UpdateRecipeCommandHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await _repository.GetByIdAsync(request.Id);

        if (recipe == null)
        {
            throw new KeyNotFoundException("Recipe not found");
        }

        recipe = new Recipe(request.Name, request.Description);

        foreach (var ingredient in request.Ingredients)
        {
            recipe.AddIngredient(new RecipeIngredient(recipe.Id, ingredient.IngredientId, ingredient.Quantity));
        }

        await _repository.UpdateAsync(recipe);

        return Unit.Value;
    }
}
