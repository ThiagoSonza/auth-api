using SharedKernel;

namespace Worker.Infrastructure.Extensions;

public static class ConfigurationSetup
{
    public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AppSettings>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}