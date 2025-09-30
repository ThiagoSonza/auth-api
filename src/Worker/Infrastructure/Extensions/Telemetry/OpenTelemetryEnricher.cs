using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Worker.Infrastructure.Extensions.Telemetry;

public class OpenTelemetryEnricher : ILogEventEnricher
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

        // Environments values
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("MachineName", Environment.MachineName));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ProcessId", Environment.ProcessId));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ThreadId", Environment.CurrentManagedThreadId));

        // Exception details
        if (logEvent.Exception is not null)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("exception.type", logEvent.Exception.GetType().FullName));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("exception.stacktrace", logEvent.Exception.StackTrace));
        }
    }
}