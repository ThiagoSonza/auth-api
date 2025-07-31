namespace Api.Extensions;

public static class SwaggerSetup
{
    public static IServiceCollection AdicionaSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "API de Autenticação e Autorização",
                Version = "v1",
                Description = "API para gerenciar autenticação e autorização de usuários com ASP.NET Identity e JWT.",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Thiago Sonza",
                    Email = "thiagosnz11@gmail.com",
                    Url = new Uri("https://www.linkedin.com/in/thiago-sonza-10a408196")
                },
                License = new Microsoft.OpenApi.Models.OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/license/mit/")
                },
            });

            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Insira 'Bearer' seguido do token JWT. Exemplo: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }

    public static WebApplication ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            opt.DocumentTitle = "API Documentation";
            opt.DefaultModelsExpandDepth(-1); // Hide default models section
        });
        return app;
    }
}