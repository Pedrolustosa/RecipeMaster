using RecipeMaster.Infra.Identity;
using RecipeMaster.Infra.Repositories;
using RecipeMaster.Application.Services;
using RecipeMaster.Core.Interfaces.Services;
using RecipeMaster.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeMaster.Infra.IoC.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICostCalculationService, CostCalculationService>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddIdentitySetup();
        services.AddRepositories();
        services.AddServices();
        return services;
    }
}
