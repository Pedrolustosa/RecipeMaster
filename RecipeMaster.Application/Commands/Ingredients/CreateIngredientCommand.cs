using MediatR;

namespace RecipeMaster.Application.Commands.Ingredients;

public class CreateIngredientCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Unit { get; set; }
    public decimal Cost { get; set; }
}
