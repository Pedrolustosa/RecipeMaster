using MediatR;
using RecipeMaster.Core.Entities;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.ValueObjects;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class CreateRecipeCommandHandler(IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository, ILogger<CreateRecipeCommandHandler> logger) : IRequestHandler<CreateRecipeCommand, Guid>
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    private readonly ILogger<CreateRecipeCommandHandler> _logger = logger;

    public async Task<Guid> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting the creation of a recipe...");

            var recipe = new Recipe(
                name: request.Name,
                description: request.Description,
                preparationTime: request.PreparationTime,
                cookingTime: request.CookingTime,
                servings: request.Servings,
                difficulty: Enum.Parse<DifficultyLevel>(request.Difficulty, ignoreCase: true),
                instructions: request.Instructions,
                totalCost: request.TotalCost,
                yieldPerPortion: request.YieldPerPortion
            );

            foreach (var ingredientDto in request.Ingredients)
            {
                var ingredient = await _ingredientRepository.GetByIdAsync(ingredientDto.IngredientId)
                    ?? throw new KeyNotFoundException($"Ingredient with ID {ingredientDto.IngredientId} not found.");

                ingredient.DecreaseStock(ingredientDto.Quantity);
                recipe.AddIngredient(new RecipeIngredient(recipe.Id, ingredient.Id, ingredientDto.Quantity));
            }

            await _recipeRepository.AddAsync(recipe);
            _logger.LogInformation("Successfully created recipe with ID {Id}.", recipe.Id);

            return recipe.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating the recipe.");
            throw;
        }
    }
}
