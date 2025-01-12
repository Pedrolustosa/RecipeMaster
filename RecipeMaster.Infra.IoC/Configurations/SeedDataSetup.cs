using Microsoft.AspNetCore.Builder;
using RecipeMaster.Infra.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeMaster.Infra.IoC.Configurations;

public static class SeedDataSetup
{
    public static void UseSeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        SeedData.Initialize(services);
    }
}
