using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Queries.Ingredients;

public class GetAllIngredientsQuery : IRequest<IEnumerable<IngredientDTO>>
{
}
