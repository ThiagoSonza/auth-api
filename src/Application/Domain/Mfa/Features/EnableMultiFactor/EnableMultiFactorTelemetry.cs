using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Mfa.Features.EnableMultiFactor;

public class EnableMultiFactorTelemetry(
    ActivitySource activitySource,
    ILogger<EnableMultiFactorTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(EnableMultiFactorHandler)}.{nameof(EnableMultiFactorHandler.Handle)}")!;
    private bool disposed;

    public void MarkTwoFactorEnable(string? userId)
    {
        telemetryService
            .AddTag("user.id", userId)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Two-factor authentication enabled"));

        logger.LogInformation("User {UserId} enabled two-factor authentication", userId);
    }

    public void MarkUserNotFound(string userId)
    {
        telemetryService
            .AddTag("user.exists", false)
            .AddTag("user.id", userId)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent($"User {userId} not found"));

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
