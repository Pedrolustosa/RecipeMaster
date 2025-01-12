using MediatR;

namespace RecipeMaster.Application.Commands.Ingredients;

public class UpdateIngredientCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public decimal Cost { get; set; }
}
