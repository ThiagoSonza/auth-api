using Microsoft.AspNetCore.Http.Features;

namespace Api.Extensions.CustomProblemDetails;

public static class ProblemDetailsSetup
{
    public static void AddApiProblemDetails(this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddProblemDetails(options
            => options.CustomizeProblemDetails = (context) => context.MapExceptionToStatusCode());
    }

    public static void MapExceptionToStatusCode(this ProblemDetailsContext context)
    {
        var env = context.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();
        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    }
}
