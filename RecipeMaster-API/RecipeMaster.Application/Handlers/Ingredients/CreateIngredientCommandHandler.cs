using MediatR;
using RecipeMaster.Core.Entities;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.ValueObjects;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Commands.Ingredients;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class CreateIngredientCommandHandler(IIngredientRepository repository, ILogger<CreateIngredientCommandHandler> logger) : IRequestHandler<CreateIngredientCommand, Guid>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly ILogger<CreateIngredientCommandHandler> _logger = logger;

    public async Task<Guid> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating a new ingredient...");
            var ingredient = new Ingredient(
                request.Name,
                request.Description,
                Enum.Parse<MeasurementUnit>(request.Unit),
                new IngredientCost(request.Cost),
                request.StockQuantity,
                request.MinimumStockLevel,
                request.SupplierName,
                request.IsPerishable,
                request.OriginCountry,
                request.StorageInstructions,
                request.IsActive
            );

            await _repository.AddAsync(ingredient);
            _logger.LogInformation("Successfully created ingredient with ID {Id}.", ingredient.Id);

            return ingredient.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating ingredient.");
            throw;
        }
    }
}
