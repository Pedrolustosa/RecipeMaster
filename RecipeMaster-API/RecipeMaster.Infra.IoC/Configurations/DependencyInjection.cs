using Serilog;
using RecipeMaster.Core.JWT;
using RecipeMaster.Infra.Identity;
using Microsoft.Extensions.Options;
using RecipeMaster.Infra.Repositories;
using RecipeMaster.Application.Services;
using Microsoft.Extensions.Configuration;
using RecipeMaster.Application.Interfaces;
using RecipeMaster.Infra.Identity.Services;
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
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentitySetup();
        services.AddRepositories();
        services.AddServices();
        var jwtSettingsSection = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSettingsSection);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
        services.AddSingleton(Log.Logger);
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateIngredientCommand).Assembly);
        });
        return services;
    }
}
