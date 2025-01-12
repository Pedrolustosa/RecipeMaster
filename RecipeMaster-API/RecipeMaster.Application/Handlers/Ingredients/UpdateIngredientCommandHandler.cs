using AutoMapper;
using MediatR;
using RecipeMaster.Application.Commands.Ingredients;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand, Unit>
{
    private readonly IIngredientRepository _repository;
    private readonly IMapper _mapper;

    public UpdateIngredientCommandHandler(IIngredientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var ingredient = await _repository.GetByIdAsync(request.Id);

        if (ingredient == null)
        {
            throw new KeyNotFoundException("Ingredient not found");
        }

        // Mapeia as mudanças do comando para a entidade existente
        _mapper.Map(request, ingredient);

        // Atualiza no repositório
        await _repository.UpdateAsync(ingredient);

        return Unit.Value;
    }
}
