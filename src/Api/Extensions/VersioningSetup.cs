using Asp.Versioning;

namespace Api.Extensions;

public static class VersioningSetup
{
    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.AssumeDefaultVersionWhenUnspecified = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
        return services;
    }
}