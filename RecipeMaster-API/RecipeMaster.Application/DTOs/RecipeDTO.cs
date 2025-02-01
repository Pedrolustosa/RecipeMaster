namespace RecipeMaster.Application.DTOs;

public class RecipeDTO
{
    public Guid Id { get; set; }
    /// <summary>
    /// Represents the overall quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Represents the unit cost.
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// Represents the quantity produced per production cycle.
    /// </summary>
    public decimal QuantityPerProduction { get; set; }

    /// <summary>
    /// Represents the total cost per production.
    /// </summary>
    public decimal ProductionCost { get; set; }

    public List<RecipeIngredientDTO> Ingredients { get; set; }
}
