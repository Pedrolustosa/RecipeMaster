using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Core.Entities;

public class Ingredient
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public MeasurementUnit Unit { get; private set; }
    public IngredientCost Cost { get; private set; }
    public decimal StockQuantity { get; private set; }
    public decimal MinimumStockLevel { get; private set; }
    public string SupplierName { get; private set; }
    public bool IsPerishable { get; private set; }
    public string OriginCountry { get; private set; }
    public string StorageInstructions { get; private set; }
    public bool IsActive { get; private set; }

    public Ingredient() { }

    public Ingredient(
        string name,
        string description,
        MeasurementUnit unit,
        IngredientCost cost,
        decimal stockQuantity,
        decimal minimumStockLevel,
        string supplierName,
        bool isPerishable,
        string originCountry,
        string storageInstructions,
        bool isActive)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Unit = unit;
        Cost = cost;
        StockQuantity = stockQuantity;
        MinimumStockLevel = minimumStockLevel;
        SupplierName = supplierName;
        IsPerishable = isPerishable;
        OriginCountry = originCountry;
        StorageInstructions = storageInstructions;
        IsActive = isActive;
    }

    public void UpdateCost(IngredientCost newCost) => Cost = newCost ?? throw new ArgumentNullException(nameof(newCost));

    public void DecreaseStock(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("The quantity to decrease must be greater than zero.", nameof(quantity));

        if (quantity > StockQuantity)
            throw new InvalidOperationException($"Not enough stock for {Name}. Current stock: {StockQuantity}, requested: {quantity}");

        StockQuantity -= quantity;

        if (StockQuantity < MinimumStockLevel)
            NotifyLowStock();
    }

    private void NotifyLowStock()
    {
        Console.WriteLine($"Warning: Stock for {Name} is below the minimum level. Current stock: {StockQuantity}, Minimum level: {MinimumStockLevel}");
    }
}
