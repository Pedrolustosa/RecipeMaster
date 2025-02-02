namespace RecipeMaster.Application.DTOs;

public class RecipeDTO
{
    public Guid Id { get; set; }

    public string RecipeName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitCost { get; set; }

    public decimal QuantityPerProduction { get; set; }

    public decimal ProductionCost { get; set; }

    public List<RecipeIngredientDTO> Ingredients { get; set; }
}
