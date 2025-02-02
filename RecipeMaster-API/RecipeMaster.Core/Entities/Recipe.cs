using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Core.Entities
{
    public class Recipe
    {
        public Guid Id { get; private set; }

        private readonly List<RecipeIngredient> _ingredients = new List<RecipeIngredient>();
        public IReadOnlyCollection<RecipeIngredient> Ingredients => _ingredients.AsReadOnly();

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
            _ingredients.AddRange(ingredients);
            RecipeName = recipeName;
            Quantity = quantity;
            UnitCost = unitCost;
            QuantityPerProduction = quantityPerProduction;
            ProductionCost = productionCost;
        }

        public void AddIngredient(RecipeIngredient ingredient)
        {
            ArgumentNullException.ThrowIfNull(ingredient);
            _ingredients.Add(ingredient);
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
}
