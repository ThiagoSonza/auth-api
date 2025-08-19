using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Mfa.Features.GetStatus;

public class GetStatusTelemetry(
    ActivitySource activitySource,
    ILogger<GetStatusTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(GetStatusHandler)}.{nameof(GetStatusHandler.Handle)}")!;
    private bool disposed;

    public void MarkStatusTwoFactor(string userId, bool status)
    {
        telemetryService
            .AddTag("user.id", userId)
            .AddTag("user.two_factor_enabled", status);

        logger.LogInformation("User {UserId} enabled two-factor authentication", userId);
    }

    public void MarkUserNotFound(string userId)
    {
        telemetryService
            .AddTag("user.exists", false)
            .AddTag("user.email", userId)
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
