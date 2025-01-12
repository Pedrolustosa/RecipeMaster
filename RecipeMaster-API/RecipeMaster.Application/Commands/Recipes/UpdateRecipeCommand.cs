using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Commands.Recipes;

public class UpdateRecipeCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<IngredientDto> Ingredients { get; set; }

    public class IngredientDto
    {
        public Guid IngredientId { get; set; }
        public decimal Quantity { get; set; }
    }
}
