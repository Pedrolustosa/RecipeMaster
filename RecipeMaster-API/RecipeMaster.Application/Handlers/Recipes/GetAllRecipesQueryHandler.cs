using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Recipes;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class GetAllRecipesQueryHandler(
    IRecipeRepository repository,
    IMapper mapper,
    ILogger<GetAllRecipesQueryHandler> logger) : IRequestHandler<GetAllRecipesQuery, IEnumerable<RecipeDTO>>
{
    private readonly IRecipeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetAllRecipesQueryHandler> _logger = logger;

    public async Task<IEnumerable<RecipeDTO>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching all recipes...");
            var recipes = await _repository.GetAllAsync();
            _logger.LogInformation("Successfully fetched all recipes.");
            return _mapper.Map<IEnumerable<RecipeDTO>>(recipes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all recipes.");
            throw;
        }
    }
}
