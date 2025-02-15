using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Recipes;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Application.Services.Interfaces;

namespace RecipeMaster.Application.Services;

public class RecipeService(IMediator mediator, IMapper mapper, ILogger<RecipeService> logger) : IRecipeService
{
    private readonly IMapper _mapper = mapper;
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<RecipeService> _logger = logger;

    public async Task<IEnumerable<RecipeDTO>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all recipes...");
            var recipes = await _mediator.Send(new GetAllRecipesQuery());
            _logger.LogInformation("Successfully fetched all recipes.");
            return recipes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all recipes.");
            throw;
        }
    }

    public async Task<RecipeDTO> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching recipe with ID {Id}...", id);
            var recipe = await _mediator.Send(new GetRecipeByIdQuery { Id = id });

            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Recipe with ID {id} not found.");
            }

            _logger.LogInformation("Successfully fetched recipe with ID {Id}.", id);
            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching recipe with ID {Id}.", id);
            throw;
        }
    }

    public async Task<Guid> CreateAsync(RecipeDTO dto)
    {
        try
        {
            _logger.LogInformation("Mapping RecipeDTO to CreateRecipeCommand...");
            var command = _mapper.Map<CreateRecipeCommand>(dto);

            _logger.LogInformation("Creating a new recipe...");
            var id = await _mediator.Send(command);
            _logger.LogInformation("Successfully created recipe with ID {Id}.", id);
            return id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a new recipe.");
            throw;
        }
    }

    public async Task UpdateAsync(Guid id, UpdateRecipeDTO dto)
    {
        try
        {
            _logger.LogInformation("Updating recipe with ID {Id}...", id);
            var command = _mapper.Map<UpdateRecipeCommand>(dto);
            command.Id = id;
            await _mediator.Send(command);
            _logger.LogInformation("Successfully updated recipe with ID {Id}.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating recipe with ID {Id}.", id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting recipe with ID {Id}...", id);
            await _mediator.Send(new DeleteRecipeCommand { Id = id });
            _logger.LogInformation("Successfully deleted recipe with ID {Id}.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting recipe with ID {Id}.", id);
            throw;
        }
    }
}
