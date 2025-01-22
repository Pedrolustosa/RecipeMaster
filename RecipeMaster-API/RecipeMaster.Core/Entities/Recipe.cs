using RecipeMaster.Core.ValueObjects;

namespace RecipeMaster.Core.Entities;

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
    public decimal TotalCost { get; private set; }
    public decimal YieldPerPortion { get; private set; }

    public Recipe() { }

    public Recipe(string name, string description, int preparationTime, int cookingTime, int servings,
                  string instructions, DifficultyLevel difficulty, decimal totalCost, decimal yieldPerPortion)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        PreparationTime = preparationTime;
        CookingTime = cookingTime;
        Servings = servings;
        Instructions = instructions;
        Difficulty = difficulty;
        TotalCost = totalCost;
        YieldPerPortion = yieldPerPortion;
        Ingredients = new List<RecipeIngredient>();
    }

    public void AddIngredient(RecipeIngredient ingredient)
    {
        if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
        var ingredients = Ingredients.ToList();
        ingredients.Add(ingredient);
        Ingredients = ingredients.AsReadOnly();
    }

    public void UpdateTotalCost(decimal totalCost)
    {
        if (totalCost < 0) throw new ArgumentException("Total cost cannot be negative.");
        TotalCost = totalCost;
    }

    public void UpdateYieldPerPortion(decimal yieldPerPortion)
    {
        if (yieldPerPortion <= 0) throw new ArgumentException("Yield per portion must be greater than zero.");
        YieldPerPortion = yieldPerPortion;
    }
}
