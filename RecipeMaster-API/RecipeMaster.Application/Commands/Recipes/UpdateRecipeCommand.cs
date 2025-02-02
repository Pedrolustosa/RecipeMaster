using MediatR;

namespace RecipeMaster.Application.Commands.Recipes;

public class UpdateRecipeCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string RecipeName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal QuantityPerProduction { get; set; }
    public decimal ProductionCost { get; set; }

    public List<IngredientDto> Ingredients { get; set; }

    public class IngredientDto
    {
        public Guid IngredientId { get; set; }
        public decimal Quantity { get; set; }
    }
}
