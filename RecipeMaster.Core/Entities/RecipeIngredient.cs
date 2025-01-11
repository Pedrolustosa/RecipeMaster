namespace RecipeMaster.Core.Entities;

public class RecipeIngredient(Guid recipeId, Guid ingredientId, decimal quantity)
{
    public Guid RecipeId { get; private set; } = recipeId;
    public Guid IngredientId { get; private set; } = ingredientId;
    public decimal Quantity { get; private set; } = quantity;
}
