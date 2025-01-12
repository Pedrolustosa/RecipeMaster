using AutoMapper;
using MediatR;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Recipes;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery, IEnumerable<RecipeDTO>>
{
    private readonly IRecipeRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRecipesQueryHandler(IRecipeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecipeDTO>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
    {
        var recipes = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RecipeDTO>>(recipes);
    }
}
