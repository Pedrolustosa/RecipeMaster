using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Core.Entities
{
    public class Recipe
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyCollection<RecipeIngredient> Ingredients { get; private set; }
        public int PreparationTime { get; private set; }
        public int CookingTime { get; private set; }
        public int Servings { get; private set; }
        public string Instructions { get; private set; }
        public DifficultyLevel Difficulty { get; private set; }
        public decimal TotalCost => Ingredients.Sum(i => i.Quantity * i.Ingredient.Cost.Value);

        public Recipe() { }

        public Recipe(string name, string description, int preparationTime, int cookingTime, int servings,
                      string instructions, DifficultyLevel difficulty)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            PreparationTime = preparationTime;
            CookingTime = cookingTime;
            Servings = servings;
            Instructions = instructions;
            Difficulty = difficulty;
            Ingredients = new List<RecipeIngredient>();
        }

        public void AddIngredient(RecipeIngredient ingredient)
        {
            if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
            var ingredients = Ingredients.ToList();
            ingredients.Add(ingredient);
            Ingredients = ingredients.AsReadOnly();
        }
    }
}