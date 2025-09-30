using Microsoft.AspNetCore.HttpLogging;

namespace Api.Extensions;

public static class LoggingSetup
{
    public static IServiceCollection AddHttpLogging(this IServiceCollection services)
    {
        return services.AddHttpLogging(c =>
        {
            c.LoggingFields = HttpLoggingFields.RequestHeaders | HttpLoggingFields.ResponseHeaders;
            c.RequestHeaders.Add("x-request-id");
            c.ResponseHeaders.Add("x-request-id");
            c.MediaTypeOptions.AddText("application/json");
            c.RequestBodyLogLimit = 4096;
            c.ResponseBodyLogLimit = 4096;
            c.CombineLogs = true;
        });
    }
}