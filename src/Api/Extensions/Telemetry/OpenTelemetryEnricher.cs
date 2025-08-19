using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Api.Extensions.Telemetry;

public class OpenTelemetryEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // IDs de rastreamento (trace/spans)
        var activity = Activity.Current;
        if (activity is not null)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("trace_id", activity.TraceId.ToString()));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("span_id", activity.SpanId.ToString()));
        }

        // Contexto HTTP
        var context = httpContextAccessor.HttpContext;
        if (context is not null)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(ip))
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("client.ip", ip));

            var userId = context.User?.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : null;
            if (!string.IsNullOrEmpty(userId))
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("user.id", userId));
        }

        // Exceção
        if (logEvent.Exception is not null)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("exception.type", logEvent.Exception.GetType().FullName));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("exception.stacktrace", logEvent.Exception.StackTrace));
        }
    }
}
