using Microsoft.Extensions.DependencyInjection;
using RecipeMaster.Application.Mappings;

namespace RecipeMaster.Infra.IoC.Configurations;

public static class AutoMapperSetup
{
    public static IServiceCollection AddAutoMapperSetup(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}
