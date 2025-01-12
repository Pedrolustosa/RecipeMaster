using MediatR;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, Guid>
{
    private readonly IRecipeRepository _repository;

    public CreateRecipeCommandHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = new Recipe(request.Name, request.Description);

        foreach (var ingredient in request.Ingredients)
        {
            recipe.AddIngredient(new RecipeIngredient(recipe.Id, ingredient.IngredientId, ingredient.Quantity));
        }

        await _repository.AddAsync(recipe);
        return recipe.Id;
    }
}
