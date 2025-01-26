using Serilog;
using RecipeMaster.Infra.Identity;
using RecipeMaster.Infra.Repositories;
using RecipeMaster.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using RecipeMaster.Core.Interfaces.Repositories;
using RecipeMaster.Application.Services.Interfaces;
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
        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<IRecipeService, RecipeService>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddIdentitySetup();
        services.AddRepositories();
        services.AddServices();
        services.AddSingleton(Log.Logger);
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateIngredientCommand).Assembly);
        });
        return services;
    }

}
