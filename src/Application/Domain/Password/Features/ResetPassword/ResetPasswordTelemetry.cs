using System.Diagnostics;
using Domain.User;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Password.Features.ResetPassword;

public class ResetPasswordTelemetry(
    ActivitySource activitySource,
    ILogger<ResetPasswordTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(ResetPasswordHandler)}.{nameof(ResetPasswordHandler.Handle)}")!;
    private bool disposed;

    public void MarkUserNotFound(string email)
    {
        telemetryService
            .AddTag("user.exists", false)
            .AddTag("user.email", email)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("User not found"));

        logger.LogError("User not found: {Email}", email);
    }

    public void MarkEmailNotConfirmed(UserDomain user)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("Email not confirmed confirmed"));

        logger.LogInformation("Email not confirmed for user: {UserId} {UserEmail}", user.Id, user.Email);
    }

    public void MarkPasswordChanged(UserDomain user)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Password changed successfully"));

        logger.LogInformation("Password changed successfully for user: {UserId} {UserEmail}", user.Id, user.Email);
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
