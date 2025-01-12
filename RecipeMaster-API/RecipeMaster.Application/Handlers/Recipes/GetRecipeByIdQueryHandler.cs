using AutoMapper;
using MediatR;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Recipes;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes;

public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, RecipeDTO>
{
    private readonly IRecipeRepository _repository;
    private readonly IMapper _mapper;

    public GetRecipeByIdQueryHandler(IRecipeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RecipeDTO> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        var recipe = await _repository.GetByIdAsync(request.Id);

        if (recipe == null)
        {
            throw new KeyNotFoundException("Recipe not found");
        }

        return _mapper.Map<RecipeDTO>(recipe);
    }
}
