using System.Diagnostics;

namespace Api.Extensions.Telemetry;

public static class DiagnosticsConfig
{
    public const string ServiceName = "MyService";
    public static ActivitySource ActivitySource => new(ServiceName);
}
