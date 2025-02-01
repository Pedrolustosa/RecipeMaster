namespace RecipeMaster.Core.Entities;

public class Recipe
{
    public Guid Id { get; private set; }
    public IReadOnlyCollection<RecipeIngredient> Ingredients { get; private set; }
    public int Quantity { get; private set; }                   
    public decimal UnitCost { get; private set; }               
    public decimal QuantityPerProduction { get; private set; }  
    public decimal ProductionCost { get; private set; }         

    public Recipe()
    {
        Ingredients = new List<RecipeIngredient>().AsReadOnly();
    }

    public Recipe(
        IReadOnlyCollection<RecipeIngredient> ingredients,
        int quantity,
        decimal unitCost,
        decimal quantityPerProduction,
        decimal productionCost)
    {
        Id = Guid.NewGuid();
        Ingredients = ingredients;
        Quantity = quantity;
        UnitCost = unitCost;
        QuantityPerProduction = quantityPerProduction;
        ProductionCost = productionCost;
    }

    public void AddIngredient(RecipeIngredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);

        var ingredientList = Ingredients.ToList();
        ingredientList.Add(ingredient);
        Ingredients = ingredientList.AsReadOnly();
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

        Quantity = newQuantity;
    }

    public void UpdateUnitCost(decimal newUnitCost)
    {
        if (newUnitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative.", nameof(newUnitCost));

        UnitCost = newUnitCost;
    }

    public void UpdateQuantityPerProduction(decimal newQuantityPerProduction)
    {
        if (newQuantityPerProduction <= 0)
            throw new ArgumentException("Quantity per production must be greater than zero.", nameof(newQuantityPerProduction));

        QuantityPerProduction = newQuantityPerProduction;
    }

    public void UpdateProductionCost(decimal newProductionCost)
    {
        if (newProductionCost < 0)
            throw new ArgumentException("Production cost cannot be negative.", nameof(newProductionCost));

        ProductionCost = newProductionCost;
    }
}
