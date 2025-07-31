using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infrastructure.Validation;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
            return await next(context);

        var argument = context.Arguments.OfType<T>().FirstOrDefault();
        if (argument is null)
            return Results.BadRequest("Dados inválidos.");

        var validationResult = await validator.ValidateAsync(argument);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => (PropertyName: e.PropertyName, ErrorMessage: e.ErrorMessage));
            return Results.BadRequest(new ValidatonCustomProblemDetails(HttpStatusCode.BadRequest, request: context.HttpContext.Request, errors: errors));
        }

        return await next(context);
    }
}
