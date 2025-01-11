using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Core.Entities;

public class Ingredient
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public MeasurementUnit Unit { get; private set; }
    public IngredientCost Cost { get; private set; }

    public Ingredient() { }

    public Ingredient(string name, MeasurementUnit unit, IngredientCost cost)
    {
        Id = Guid.NewGuid();
        Name = name;
        Unit = unit;
        Cost = cost;
    }

    public void UpdateCost(IngredientCost newCost) => Cost = newCost ?? throw new ArgumentNullException(nameof(newCost));
}
