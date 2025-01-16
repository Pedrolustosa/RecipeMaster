using MediatR;
using RecipeMaster.Application.DTOs;
using RecipeMaster.Application.Queries.Dashboard;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Dashboard
{
    public class MostUsedIngredientsQueryHandler(IRecipeRepository repository) : IRequestHandler<MostUsedIngredientsQuery, List<IngredientUsageDTO>>
    {
        private readonly IRecipeRepository _repository = repository;

        public async Task<List<IngredientUsageDTO>> Handle(MostUsedIngredientsQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _repository.GetMostUsedIngredientsAsync();

            return ingredients
                .Select(i => new IngredientUsageDTO
                {
                    Name = i.Name,
                    RecipeCount = i.RecipeCount
                })
                .ToList();
        }
    }
}
