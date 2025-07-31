using Application.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Api.Extensions;

public static class InjectorConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentityService();

        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<EmailTemplateRenderer>();

        return services;
    }
}
