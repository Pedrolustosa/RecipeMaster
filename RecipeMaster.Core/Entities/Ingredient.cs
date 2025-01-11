using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Core.Entities;

public class Ingredient(string name, MeasurementUnit unit, IngredientCost cost)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public MeasurementUnit Unit { get; private set; } = unit;
    public IngredientCost Cost { get; private set; } = cost;

    public void UpdateCost(IngredientCost newCost)
    {
        if (newCost == null) throw new ArgumentNullException(nameof(newCost));
        Cost = newCost;
    }
}
