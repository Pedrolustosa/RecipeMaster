using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Services.Interfaces;
using RecipeMaster.Application.Queries.Ingredients;
using RecipeMaster.Application.Commands.Ingredients;

namespace RecipeMaster.Application.Services;

public class IngredientService(IMediator mediator, IMapper mapper, ILogger<IngredientService> logger) : IIngredientService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IngredientService> _logger = logger;

    public async Task<IEnumerable<IngredientDTO>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all ingredients...");
            var ingredients = await _mediator.Send(new GetAllIngredientsQuery());
            _logger.LogInformation("Successfully fetched {Count} ingredients.", ingredients.Count());
            return ingredients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all ingredients.");
            throw;
        }
    }

    public async Task<IngredientDTO> GetByIdAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid ID {Id} provided for fetching ingredient.", id);
                throw new ArgumentException("ID cannot be empty.", nameof(id));
            }

            _logger.LogInformation("Fetching ingredient with ID {Id}...", id);
            var ingredient = await _mediator.Send(new GetIngredientByIdQuery { Id = id });

            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Ingredient with ID {id} not found.");
            }

            _logger.LogInformation("Successfully fetched ingredient with ID {Id}.", id);
            return ingredient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching ingredient with ID {Id}.", id);
            throw;
        }
    }

    public async Task<Guid> CreateAsync(IngredientDTO dto)
    {
        try
        {
            if (dto == null)
            {
                _logger.LogWarning("Null IngredientDTO provided for creation.");
                throw new ArgumentNullException(nameof(dto), "IngredientDTO cannot be null.");
            }

            _logger.LogInformation("Mapping IngredientDTO to CreateIngredientCommand...");
            var command = _mapper.Map<CreateIngredientCommand>(dto);

            _logger.LogInformation("Creating a new ingredient...");
            var id = await _mediator.Send(command);
            _logger.LogInformation("Successfully created ingredient with ID {Id}.", id);
            return id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a new ingredient.");
            throw;
        }
    }

    public async Task UpdateAsync(Guid id, UpdateIngredientDTO dto)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid ID {Id} provided for updating ingredient.", id);
                throw new ArgumentException("ID cannot be empty.", nameof(id));
            }

            if (dto == null)
            {
                _logger.LogWarning("Null UpdateIngredientDTO provided for updating ingredient with ID {Id}.", id);
                throw new ArgumentNullException(nameof(dto), "UpdateIngredientDTO cannot be null.");
            }

            _logger.LogInformation("Updating ingredient with ID {Id}...", id);
            var command = _mapper.Map<UpdateIngredientCommand>(dto);
            command.Id = id;

            await _mediator.Send(command);
            _logger.LogInformation("Successfully updated ingredient with ID {Id}.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating ingredient with ID {Id}.", id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid ID {Id} provided for deleting ingredient.", id);
                throw new ArgumentException("ID cannot be empty.", nameof(id));
            }

            _logger.LogInformation("Deleting ingredient with ID {Id}...", id);
            await _mediator.Send(new DeleteIngredientCommand { Id = id });
            _logger.LogInformation("Successfully deleted ingredient with ID {Id}.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting ingredient with ID {Id}.", id);
            throw;
        }
    }
}
