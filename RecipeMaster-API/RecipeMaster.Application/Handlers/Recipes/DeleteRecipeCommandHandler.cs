using MediatR;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class DeleteRecipeCommandHandler(IRecipeRepository repository, ILogger<DeleteRecipeCommandHandler> logger) : IRequestHandler<DeleteRecipeCommand, Unit>
{
    private readonly IRecipeRepository _repository = repository;
    private readonly ILogger<DeleteRecipeCommandHandler> _logger = logger;

    public async Task<Unit> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting recipe with ID {Id}...", request.Id);

            await _repository.DeleteAsync(request.Id);
            _logger.LogInformation("Successfully deleted recipe with ID {Id}.", request.Id);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting recipe with ID {Id}.", request.Id);
            throw;
        }
    }
}
