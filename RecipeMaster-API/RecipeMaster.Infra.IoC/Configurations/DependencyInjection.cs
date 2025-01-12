using MediatR;
using RecipeMaster.Infra.Identity;
using RecipeMaster.Infra.Repositories;
using RecipeMaster.Application.Services;
using RecipeMaster.Core.Interfaces.Services;
using RecipeMaster.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RecipeMaster.Application.Commands.Ingredients;

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

        // Registrar o MediatR corretamente
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateIngredientCommand).Assembly);
        });

        return services;
    }
}
