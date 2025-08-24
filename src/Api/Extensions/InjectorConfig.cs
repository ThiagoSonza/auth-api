using Api.Extensions.Identity;
using Application.Domain.Email.Features.ConfirmEmail;
using Application.Domain.Email.Features.ResendConfirmationEmail;
using Application.Domain.Login.Features.Authenticate;
using Application.Domain.Login.Features.RefreshToken;
using Application.Domain.Mfa.Features.ConfirmMultiFactor;
using Application.Domain.Mfa.Features.DisableMultiFactor;
using Application.Domain.Mfa.Features.EnableMultiFactor;
using Application.Domain.Mfa.Features.GenerateQrCode;
using Application.Domain.Mfa.Features.GetStatus;
using Application.Domain.Password.Features.ForgotPassword;
using Application.Domain.Password.Features.ResetPassword;
using Application.Domain.Roles.Features.CreateRole;
using Application.Domain.Roles.Features.DeleteRole;
using Application.Domain.Roles.Features.GetRoleByName;
using Application.Domain.Roles.Features.GetRoles;
using Application.Domain.User.Features.GetUserInfo;
using Application.Domain.User.Features.ManageUserInfo;
using Application.Domain.User.Features.RegisterUser;
using Application.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedKernel;
using Worker.Services;

namespace Api.Extensions;

public static class InjectorConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        var settings = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value.ConnectionStrings;
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(settings.DefaultConnection));

        services.AddIdentityService();

        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<EmailTemplateRenderer>();

        services.AddTelemetries();

        return services;
    }

    private static IServiceCollection AddTelemetries(this IServiceCollection services)
    {
        services.AddScoped<ConfirmEmailTelemetry>();
        services.AddScoped<ResendConfirmationTelemetry>();
        services.AddScoped<AuthenticateTelemetry>();
        services.AddScoped<RefreshTokenTelemetry>();
        services.AddScoped<ConfirmMultiFactorTelemetry>();
        services.AddScoped<DisableMultiFactorTelemetry>();
        services.AddScoped<EnableMultiFactorTelemetry>();
        services.AddScoped<GenerateQrCodeTelemetry>();
        services.AddScoped<GetStatusTelemetry>();

        services.AddScoped<ForgotPasswordTelemetry>();
        services.AddScoped<ResetPasswordTelemetry>();

        services.AddScoped<CreateRoleTelemetry>();
        services.AddScoped<DeleteRoleTelemetry>();
        services.AddScoped<GetRoleByNameTelemetry>();
        services.AddScoped<GetRolesTelemetry>();

        services.AddScoped<GetUserInfoTelemetry>();
        services.AddScoped<ManageUserInfoTelemetry>();
        services.AddScoped<RegisterUserTelemetry>();

        return services;
    }
}
