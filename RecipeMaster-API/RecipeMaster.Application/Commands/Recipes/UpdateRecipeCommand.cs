using MediatR;

namespace RecipeMaster.Application.Commands.Recipes;

public class UpdateRecipeCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PreparationTime { get; set; }
    public int CookingTime { get; set; }
    public int Servings { get; set; }
    public string Difficulty { get; set; }
    public string Instructions { get; set; }
    public List<IngredientDto> Ingredients { get; set; }

    public class IngredientDto
    {
        public Guid IngredientId { get; set; }
        public decimal Quantity { get; set; }
    }
}
