using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeMaster.Infra.IoC.Configurations
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the field below.\n\nExample: Bearer abcdef12345",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                options.OperationFilter<AddBearerTokenDefaultValueFilter>();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RecipeMaster API",
                    Version = "v1",
                    Description = "API documentation for RecipeMaster application",
                });
            });

            return services;
        }
    }
}
