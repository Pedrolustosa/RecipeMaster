namespace RecipeMaster.Application.DTOs;

public class UpdateRecipeDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<UpdateRecipeIngredientDTO> Ingredients { get; set; }
}
