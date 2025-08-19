using System.Diagnostics;
using Domain.User;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Email.Features.ResendConfirmationEmail;

public class ResendConfirmationTelemetry(
    ActivitySource activitySource,
    ILogger<ResendConfirmationTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(ResendConfirmationHandler)}.{nameof(ResendConfirmationHandler.Handle)}")!;
    private bool disposed;

    public void MarkEmailAlreadyConfirmed(UserDomain user)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("Email already confirmed"));

        logger.LogError("Email already confirmed for user: {UserId} {UserEmail}", user.Id, user.Email);
    }

    public void MarkEmailResent(UserDomain user)
    {
        telemetryService
            .AddTag("user.exists", true)
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("User found"))
            .AddEvent(new ActivityEvent("Email resent"));

        logger.LogInformation("Email resent for user: {UserId} {Email}", user.Id, user.Email);
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