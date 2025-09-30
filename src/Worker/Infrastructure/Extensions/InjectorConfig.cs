using Microsoft.AspNetCore.Identity.UI.Services;
using Worker.ManageUserContext.Password.ForgotPassword;
using Worker.ManageUserContext.Password.ResetPassword;
using Worker.ManageUserContext.User.ConfirmEmail;
using Worker.ManageUserContext.User.RegisterUser;
using Worker.Services;

namespace Worker.Infrastructure.Extensions;

public static class InjectorConfig
{
    public static IServiceCollection AddInjectorConfig(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<EmailTemplateRenderer>();

        services.AddTelemetries();

        return services;
    }

    private static IServiceCollection AddTelemetries(this IServiceCollection services)
    {
        services.AddScoped<ForgotPasswordTelemetry>();
        services.AddScoped<ResetPasswordTelemetry>();
        services.AddScoped<ResendConfirmationTelemetry>();
        services.AddScoped<RegisterUserTelemetry>();

        return services;
    }
}
