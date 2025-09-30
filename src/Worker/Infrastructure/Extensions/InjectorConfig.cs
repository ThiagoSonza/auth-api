using Microsoft.AspNetCore.Identity.UI.Services;
using Worker.Services;

namespace Worker.Infrastructure.Extensions;

public static class InjectorConfig
{
    public static IServiceCollection AddInjectorConfig(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<EmailTemplateRenderer>();

        return services;
    }
}
