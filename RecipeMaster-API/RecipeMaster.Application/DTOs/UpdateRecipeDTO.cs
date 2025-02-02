namespace RecipeMaster.Application.DTOs;

public class UpdateRecipeDTO
{
    public string RecipeName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal QuantityPerProduction { get; set; }
    public decimal ProductionCost { get; set; }

    public List<UpdateRecipeIngredientDTO> Ingredients { get; set; }
}
