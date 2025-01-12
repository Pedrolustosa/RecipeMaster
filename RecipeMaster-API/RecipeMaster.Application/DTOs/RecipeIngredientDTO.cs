namespace RecipeMaster.Application.DTOs;

public class RecipeIngredientDTO
{
    public Guid IngredientId { get; set; }
    public string IngredientName { get; set; }
    public decimal Quantity { get; set; }
}
