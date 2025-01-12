using AutoMapper;
using MediatR;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Ingredients;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Ingredients;

public class GetAllIngredientsQueryHandler : IRequestHandler<GetAllIngredientsQuery, IEnumerable<IngredientDTO>>
{
    private readonly IIngredientRepository _repository;
    private readonly IMapper _mapper;

    public GetAllIngredientsQueryHandler(IIngredientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<IngredientDTO>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
    {
        var ingredients = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<IngredientDTO>>(ingredients);
    }
}