using Microsoft.AspNetCore.Identity;
using RecipeMaster.Infra.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeMaster.Infra.Identity;

public static class IdentitySetup
{
    public static IServiceCollection AddIdentitySetup(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<RecipeMasterDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
