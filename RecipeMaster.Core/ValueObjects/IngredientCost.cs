namespace RecipeMaster.Core.ValueObjects;

public class IngredientCost
{
    public decimal Value { get; private set; }

    public IngredientCost() { }

    public IngredientCost(decimal value)
    {
        if (value < 0) throw new ArgumentException("Cost cannot be negative", nameof(value));
        Value = value;
    }

    public static IngredientCost operator +(IngredientCost a, IngredientCost b)
    {
        return new IngredientCost(a.Value + b.Value);
    }

    public static IngredientCost operator *(IngredientCost cost, decimal multiplier)
    {
        return new IngredientCost(cost.Value * multiplier);
    }
}
