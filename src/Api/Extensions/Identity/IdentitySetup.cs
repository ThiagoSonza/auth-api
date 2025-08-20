using Api.Extensions.Identity.Providers;
using Application.Infrastructure;
using Domain.User;
using Microsoft.AspNetCore.Identity;

namespace Api.Extensions.Identity;

public static class IdentitySetup
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<UserDomain>(options =>
        {
            options.Tokens.EmailConfirmationTokenProvider = "EmailConfirmProvider";
            options.Tokens.PasswordResetTokenProvider = "PasswordResetProvider";
        })
        .AddRoles<IdentityRole>()
        .AddTokenProvider<EmailConfirmationTokenProvider<UserDomain>>("EmailConfirmProvider")
        .AddTokenProvider<PasswordResetTokenProvider<UserDomain>>("PasswordResetProvider")
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<AppDbContext>();

        services.Configure<EmailConfirmationTokenProviderOptions>(options
            => options.TokenLifespan = TimeSpan.FromHours(1));

        services.Configure<PasswordResetTokenProviderOptions>(options
            => options.TokenLifespan = TimeSpan.FromHours(1));

        return services;
    }

}