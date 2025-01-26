using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Queries.Ingredients;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class GetAllIngredientsQueryHandler(IIngredientRepository repository, IMapper mapper, ILogger<GetAllIngredientsQueryHandler> logger) : IRequestHandler<GetAllIngredientsQuery, IEnumerable<IngredientDTO>>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetAllIngredientsQueryHandler> _logger = logger;

    public async Task<IEnumerable<IngredientDTO>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ingredients = await _repository.GetAllAsync();
            _logger.LogInformation("Retrieved {Count} ingredients successfully.", ingredients.Count());
            return _mapper.Map<IEnumerable<IngredientDTO>>(ingredients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving ingredients.");
            throw;
        }
    }
}