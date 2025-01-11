using AutoMapper;
using MediatR;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Ingredients;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class GetIngredientByIdQueryHandler : IRequestHandler<GetIngredientByIdQuery, IngredientDTO>
{
    private readonly IIngredientRepository _repository;
    private readonly IMapper _mapper;

    public GetIngredientByIdQueryHandler(IIngredientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IngredientDTO> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        var ingredient = await _repository.GetByIdAsync(request.Id);

        if (ingredient == null)
        {
            throw new KeyNotFoundException("Ingredient not found");
        }

        return _mapper.Map<IngredientDTO>(ingredient);
    }
}
