
namespace RecipeMaster.Core.Entities;

public class Recipe
{
    public Guid Id { get; private set; }
    public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    public string RecipeName { get; set; }
    public int Quantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal QuantityPerProduction { get; private set; }
    public decimal ProductionCost { get; private set; }

    public Recipe() { }

    public Recipe(
        IEnumerable<RecipeIngredient> ingredients,
        string recipeName,
        int quantity,
        decimal unitCost,
        decimal quantityPerProduction,
        decimal productionCost)
    {
        Id = Guid.NewGuid();
        Ingredients = new List<RecipeIngredient>(ingredients);
        RecipeName = recipeName;
        Quantity = quantity;
        UnitCost = unitCost;
        QuantityPerProduction = quantityPerProduction;
        ProductionCost = productionCost;
    }

    public void AddIngredient(RecipeIngredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);
        Ingredients.Add(ingredient);
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(newQuantity));

        Quantity = newQuantity;
    }

    public void UpdateUnitCost(decimal newUnitCost)
    {
        if (newUnitCost < 0)
            throw new ArgumentException("O custo unitário não pode ser negativo.", nameof(newUnitCost));

        UnitCost = newUnitCost;
    }

    public void UpdateQuantityPerProduction(decimal newQuantityPerProduction)
    {
        if (newQuantityPerProduction <= 0)
            throw new ArgumentException("A quantidade por produção deve ser maior que zero.", nameof(newQuantityPerProduction));

        QuantityPerProduction = newQuantityPerProduction;
    }

    public void UpdateProductionCost(decimal newProductionCost)
    {
        if (newProductionCost < 0)
            throw new ArgumentException("O custo de produção não pode ser negativo.", nameof(newProductionCost));

        ProductionCost = newProductionCost;
    }
}
