using RecipeMaster.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeMaster.Infra.IoC.Configurations;

public static class AutoMapperSetup
{
    public static IServiceCollection AddAutoMapperSetup(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<IngredientMappingProfile>();
            cfg.AddProfile<RecipeMappingProfile>();
        });
        return services;
    }
}
