using FluentValidation;
using FluentValidation.AspNetCore;

namespace Api.Extensions;

public static class ValidadorSetup
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddValidatorsFromAssemblyContaining<Application.ApplicationAssembly>();

        return services;
    }
}