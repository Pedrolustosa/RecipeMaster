using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Queries.Recipes;

public class GetRecipeByIdQuery : IRequest<RecipeDTO>
{
    public Guid Id { get; set; }
}
