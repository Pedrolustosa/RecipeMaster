using MediatR;

namespace RecipeMaster.Application.Commands.Recipes;

public class DeleteRecipeCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
