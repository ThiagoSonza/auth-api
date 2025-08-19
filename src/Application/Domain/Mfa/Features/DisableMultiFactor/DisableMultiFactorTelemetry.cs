using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Mfa.Features.DisableMultiFactor;

public class DisableMultiFactorTelemetry(
    ActivitySource activitySource,
    ILogger<DisableMultiFactorTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(DisableMultiFactorHandler)}.{nameof(DisableMultiFactorHandler.Handle)}")!;
    private bool disposed;

    public void MarkTwoFactorDisable(string userId)
    {
        telemetryService
            .AddTag("user.id", userId)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Two-factor authentication disabled"));

        logger.LogInformation("User {UserId} disabled two-factor authentication", userId);
    }

    public void MarkUserNotFound(string userId)
    {
        telemetryService
            .AddTag("user.exists", false)
            .AddTag("user.id", userId)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("User not found"));

        logger.LogError("User not found: {UserId}", userId);
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
