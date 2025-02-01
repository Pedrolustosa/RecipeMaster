using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RecipeMaster.Infra.IoC.Configurations;

public class AddBearerTokenDefaultValueFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Security == null)
            operation.Security = new List<OpenApiSecurityRequirement>();

        var securityScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer abcdef12345'"
        };

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            { securityScheme, Array.Empty<string>() }
        });
    }
}
