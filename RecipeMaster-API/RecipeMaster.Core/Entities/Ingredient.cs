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
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ingredient name cannot be null or empty.", nameof(name));
        ArgumentNullException.ThrowIfNull(cost);
        if (stockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(stockQuantity));
        if (minimumStockLevel < 0)
            throw new ArgumentException("Minimum stock level cannot be negative.", nameof(minimumStockLevel));
        if (string.IsNullOrWhiteSpace(supplierName))
            throw new ArgumentException("Supplier name cannot be null or empty.", nameof(supplierName));
        if (string.IsNullOrWhiteSpace(originCountry))
            throw new ArgumentException("Origin country cannot be null or empty.", nameof(originCountry));
        if (string.IsNullOrWhiteSpace(storageInstructions))
            throw new ArgumentException("Storage instructions cannot be null or empty.", nameof(storageInstructions));

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

    public void UpdateCost(IngredientCost newCost)
    {
        ArgumentNullException.ThrowIfNull(newCost);
        Cost = newCost;
    }

    public void IncreaseStock(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("The quantity to increase must be greater than zero.", nameof(quantity));

        StockQuantity += quantity;
    }

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

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;

    public void UpdateSupplier(string newSupplierName)
    {
        if (string.IsNullOrWhiteSpace(newSupplierName))
            throw new ArgumentException("Supplier name cannot be null or empty.", nameof(newSupplierName));
        SupplierName = newSupplierName;
    }
}
