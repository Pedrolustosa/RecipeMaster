using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Queries.Ingredients;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class GetIngredientByIdQueryHandler(
    IIngredientRepository repository,
    IMapper mapper,
    ILogger<GetIngredientByIdQueryHandler> logger
) : IRequestHandler<GetIngredientByIdQuery, IngredientDTO>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetIngredientByIdQueryHandler> _logger = logger;

    public async Task<IngredientDTO> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ingredient = await _repository.GetByIdAsync(request.Id);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found.", request.Id);
                throw new KeyNotFoundException($"Ingredient with ID {request.Id} not found.");
            }

            _logger.LogInformation("Retrieved ingredient with ID {Id} successfully.", request.Id);
            return _mapper.Map<IngredientDTO>(ingredient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving ingredient with ID {Id}.", request.Id);
            throw;
        }
    }
}
