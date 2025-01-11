namespace RecipeMaster.Core.Entities;

public class Recipe
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public IReadOnlyCollection<RecipeIngredient> Ingredients { get; private set; }

    public Recipe(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Ingredients = new List<RecipeIngredient>();
    }

    public void AddIngredient(RecipeIngredient ingredient)
    {
        if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
        var ingredients = Ingredients.ToList();
        ingredients.Add(ingredient);
        Ingredients = ingredients.AsReadOnly();
    }

    public void RemoveIngredient(RecipeIngredient ingredient)
    {
        if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
        var ingredients = Ingredients.ToList();
        ingredients.Remove(ingredient);
        Ingredients = ingredients.AsReadOnly();
    }
}
