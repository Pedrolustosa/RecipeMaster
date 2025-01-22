using RecipeMaster.Application.DTOs;

public class UpdateRecipeDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int PreparationTime { get; set; }
    public int CookingTime { get; set; }
    public int Servings { get; set; }
    public string Difficulty { get; set; }
    public string Instructions { get; set; }
    public decimal TotalCost { get; set; }
    public decimal YieldPerPortion { get; set; }
    public List<UpdateRecipeIngredientDTO> Ingredients { get; set; }
}
