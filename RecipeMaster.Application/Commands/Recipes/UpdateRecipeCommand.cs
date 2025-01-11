using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Commands.Recipes;

public class UpdateRecipeCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<RecipeIngredientDTO> Ingredients { get; set; }
}
