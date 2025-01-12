using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Commands.Recipes;

public class CreateRecipeCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<RecipeIngredientDTO> Ingredients { get; set; }
}
