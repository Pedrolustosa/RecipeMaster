using MediatR;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Commands.Ingredients;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class DeleteIngredientCommandHandler(IIngredientRepository repository, ILogger<DeleteIngredientCommandHandler> logger) : IRequestHandler<DeleteIngredientCommand, Unit>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly ILogger<DeleteIngredientCommandHandler> _logger = logger;

    public async Task<Unit> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ingredient = await _repository.GetByIdAsync(request.Id);

            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found.", request.Id);
                throw new KeyNotFoundException($"Ingredient with ID {request.Id} not found.");
            }

            await _repository.DeleteAsync(request.Id);
            _logger.LogInformation("Ingredient with ID {Id} deleted successfully.", request.Id);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting ingredient with ID {Id}.", request.Id);
            throw;
        }
    }
}