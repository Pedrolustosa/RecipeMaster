namespace RecipeMaster.Application.DTOs;

public class IngredientDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public decimal Cost { get; set; }
}
