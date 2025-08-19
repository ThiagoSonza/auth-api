using System.Diagnostics;
using Domain.User;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Email.Features.ConfirmEmail;

public class ConfirmEmailTelemetry(
    ActivitySource activitySource,
    ILogger<ConfirmEmailTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(ConfirmEmailHandler)}.{nameof(ConfirmEmailHandler.Handle)}")!;
    private bool disposed;

    public void MarkEmailConfirmed(UserDomain user)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Email confirmed"));

        logger.LogInformation("Email confirmed for user: {UserId} {UserEmail}", user.Id, user.Email);
    }

    public void MarkUserFound(UserDomain user)
    {
        telemetryService
            .AddTag("user.exists", true)
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("User found"));

        logger.LogInformation("User with email: {UserId} {UserEmail}, found", user.Id, user.Email);
    }

    public void MarkUserNotFound(string email)
    {
        telemetryService
            .AddTag("user.exists", false)
            .AddTag("user.email", email)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("User not found"));

        logger.LogError("User not found: {Email}", email);
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
