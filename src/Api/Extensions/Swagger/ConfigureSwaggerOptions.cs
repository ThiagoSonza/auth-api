using System.Text;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extensions;

/// <summary>
/// Configures the Swagger generation options.
/// </summary>
/// <remarks>This allows API versioning to define a Swagger document per API version after the
/// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
/// </remarks>
/// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider = provider;

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var text = new StringBuilder("Exemplo com a API de Contagem de Acessos utilizando Minimal APIs, Swagger, Swashbuckle e API versioning.");
        var info = new OpenApiInfo
        {
            Title = "API de Autenticação e Autorização",
            Version = "v1",
            Description = "API para gerenciar autenticação e autorização de usuários com ASP.NET Identity e JWT.",
            Contact = new OpenApiContact
            {
                Name = "Thiago Sonza",
                Email = "thiagosnz11@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/thiago-sonza-10a408196")
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/license/mit/")
            }
        };
        return info;
    }
}