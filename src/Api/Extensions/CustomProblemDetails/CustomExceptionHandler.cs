using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions.CustomProblemDetails;

public sealed class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    private static readonly Dictionary<Type, int> _mapping = new()
    {
        { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
        { typeof(JsonException), StatusCodes.Status400BadRequest },
        { typeof(ArgumentException), StatusCodes.Status400BadRequest },
        { typeof(ArgumentNullException), StatusCodes.Status400BadRequest },
        { typeof(NotImplementedException), StatusCodes.Status501NotImplemented },
        { typeof(HttpRequestException), StatusCodes.Status503ServiceUnavailable },
        { typeof(Exception), StatusCodes.Status500InternalServerError },
    };

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var env = httpContext.RequestServices.GetRequiredService<IHostEnvironment>();
        var statusCode = _mapping.GetValueOrDefault(exception.GetType(), httpContext.Response.StatusCode);

        var typeofexp = exception.GetType();
        var status = _mapping.GetValueOrDefault(typeofexp);

        var detail = env.IsDevelopment() || env.IsStaging()
            ? $"Erro: {exception.Message}, Stacktrace: {exception.StackTrace}"
            : exception.Message;

        var problemDetails = new ProblemDetails
        {
            Status = _mapping.GetValueOrDefault(exception.GetType(), httpContext.Response.StatusCode),
            Title = "An error occurred",
            Type = exception.GetType().Name,
            Detail = detail,
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
