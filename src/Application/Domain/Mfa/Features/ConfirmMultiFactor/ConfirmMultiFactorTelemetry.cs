using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Mfa.Features.ConfirmMultiFactor;

public class ConfirmMultiFactorTelemetry(
    ActivitySource activitySource,
    ILogger<ConfirmMultiFactorTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(ConfirmMultiFactorHandler)}.{nameof(ConfirmMultiFactorHandler.Handle)}")!;
    private bool disposed;

    public void MarkCodeValidated(string userId)
    {
        telemetryService
            .AddTag("user.id", userId)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Code validated"));

        logger.LogInformation("User {UserId} validated code", userId);
    }

    public void MarkInvalidCode(ConfirmMultiFactorCommand command)
    {
        telemetryService
            .AddTag("user.id", command.UserId)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("Invalid code"));

        logger.LogError("User {UserId} typed invalid code: {Code}", command.UserId, command.Code);
    }

    public void MarkUserFound(string userId)
    {
        telemetryService
           .AddTag("user.exists", true)
           .AddTag("user.id", userId)
           .SetStatus(ActivityStatusCode.Ok)
           .AddEvent(new ActivityEvent("User found"));

        logger.LogInformation("User with email: {UserId}, found", userId);
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
