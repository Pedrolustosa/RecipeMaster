namespace RecipeMaster.Core.Entities;

public class RecipeIngredient
{
    public Guid RecipeId { get; private set; }
    public Guid IngredientId { get; private set; }
    public decimal Quantity { get; private set; }

    public Recipe Recipe { get; private set; }
    public Ingredient Ingredient { get; private set; }

    public RecipeIngredient() { }

    public RecipeIngredient(Guid recipeId, Guid ingredientId, decimal quantity)
    {
        RecipeId = recipeId;
        IngredientId = ingredientId;
        Quantity = quantity;
    }
}
