using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Recipes;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class GetRecipeByIdQueryHandler(
    IRecipeRepository repository,
    IMapper mapper,
    ILogger<GetRecipeByIdQueryHandler> logger) : IRequestHandler<GetRecipeByIdQuery, RecipeDTO>
{
    private readonly IRecipeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetRecipeByIdQueryHandler> _logger = logger;

    public async Task<RecipeDTO> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching recipe with ID {Id}...", request.Id);
            var recipe = await _repository.GetByIdAsync(request.Id);

            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} not found.", request.Id);
                throw new KeyNotFoundException("Recipe not found");
            }

            _logger.LogInformation("Successfully fetched recipe with ID {Id}.", request.Id);
            return _mapper.Map<RecipeDTO>(recipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching recipe with ID {Id}.", request.Id);
            throw;
        }
    }
}
