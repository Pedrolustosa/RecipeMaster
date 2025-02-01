using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class UpdateRecipeCommandHandler(
    IRecipeRepository repository,
    IMapper mapper,
    ILogger<UpdateRecipeCommandHandler> logger) : IRequestHandler<UpdateRecipeCommand, Unit>
{
    private readonly IRecipeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UpdateRecipeCommandHandler> _logger = logger;

    public async Task<Unit> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating recipe with ID {Id}...", request.Id);
            var recipe = await _repository.GetByIdAsync(request.Id)
                         ?? throw new KeyNotFoundException("Recipe not found");

            _mapper.Map(request, recipe);

            await _repository.UpdateAsync(recipe);
            _logger.LogInformation("Successfully updated recipe with ID {Id}.", request.Id);
            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating recipe with ID {Id}.", request.Id);
            throw;
        }
    }
}
