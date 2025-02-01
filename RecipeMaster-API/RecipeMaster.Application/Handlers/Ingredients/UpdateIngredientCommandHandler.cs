using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Commands.Ingredients;

namespace RecipeMaster.Application.Handlers.Ingredients
{
    public class UpdateIngredientCommandHandler(
        IIngredientRepository repository,
        IMapper mapper,
        ILogger<UpdateIngredientCommandHandler> logger
    ) : IRequestHandler<UpdateIngredientCommand, Unit>
    {
        private readonly IIngredientRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateIngredientCommandHandler> _logger = logger;

        public async Task<Unit> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ingredient = await _repository.GetByIdAsync(request.Id);
                if (ingredient == null)
                {
                    _logger.LogWarning("Ingredient with ID {Id} not found.", request.Id);
                    throw new KeyNotFoundException($"Ingredient with ID {request.Id} not found.");
                }

                _mapper.Map(request, ingredient);

                await _repository.UpdateAsync(ingredient);
                _logger.LogInformation("Ingredient with ID {Id} updated successfully.", request.Id);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating ingredient with ID {Id}.", request.Id);
                throw;
            }
        }
    }
}