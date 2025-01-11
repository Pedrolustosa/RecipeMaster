using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Queries.Ingredients;

public class GetIngredientByIdQuery : IRequest<IngredientDTO>
{
    public Guid Id { get; set; }
}
