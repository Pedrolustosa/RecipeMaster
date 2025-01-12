namespace RecipeMaster.Application.DTOs;

public class RecipeDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<RecipeIngredientDTO> Ingredients { get; set; }
}
