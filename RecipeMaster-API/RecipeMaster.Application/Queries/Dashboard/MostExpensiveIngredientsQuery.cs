using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Queries.Dashboard;

public class MostExpensiveIngredientsQuery : IRequest<List<IngredientCostDTO>>
{
}
