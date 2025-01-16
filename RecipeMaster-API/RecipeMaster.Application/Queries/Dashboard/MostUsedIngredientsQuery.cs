using MediatR;
using RecipeMaster.Application.DTOs;

namespace RecipeMaster.Application.Queries.Dashboard;

public class MostUsedIngredientsQuery : IRequest<List<IngredientUsageDTO>>
{
}
