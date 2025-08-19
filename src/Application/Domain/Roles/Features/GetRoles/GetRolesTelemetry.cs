using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Roles.Features.GetRoles;

public class GetRolesTelemetry(
    ActivitySource activitySource,
    ILogger<GetRolesTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(GetRolesHandler)}.{nameof(GetRolesHandler.Handle)}")!;
    private bool disposed;

    public void MarkRolesRetrieved(IEnumerable<GetRolesResponse> response)
    {
        telemetryService
            .AddTag("roles.count", response.Count())
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent($"Retrieved {response.Count()} roles"));

        logger.LogInformation("Roles retrieved: {Count}", response.Count());
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
            telemetryService!.Dispose();

        disposed = true;
    }
}
