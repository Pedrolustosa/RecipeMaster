using MediatR;

namespace RecipeMaster.Application.Commands.Ingredients;

public class DeleteIngredientCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
