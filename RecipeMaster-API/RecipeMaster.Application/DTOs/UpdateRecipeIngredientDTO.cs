namespace RecipeMaster.Application.DTOs;

public class UpdateRecipeIngredientDTO
{
    public Guid IngredientId { get; set; }
    public decimal Quantity { get; set; }
}
