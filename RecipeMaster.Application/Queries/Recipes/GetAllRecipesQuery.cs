using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Queries.Recipes;

public class GetAllRecipesQuery : IRequest<IEnumerable<RecipeDTO>>
{
}
