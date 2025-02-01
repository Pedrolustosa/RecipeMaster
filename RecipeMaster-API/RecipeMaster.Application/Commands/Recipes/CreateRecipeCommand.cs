using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Commands.Recipes;

public class CreateRecipeCommand : IRequest<Guid>
{
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal QuantityPerProduction { get; set; }
    public decimal ProductionCost { get; set; }

    public List<RecipeIngredientDTO> Ingredients { get; set; }
}
