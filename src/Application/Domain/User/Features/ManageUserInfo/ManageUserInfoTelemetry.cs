using System.Diagnostics;
using Domain.User;
using Microsoft.Extensions.Logging;

namespace Application.Domain.User.Features.ManageUserInfo;

public class ManageUserInfoTelemetry(
    ActivitySource activitySource,
    ILogger<ManageUserInfoTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(ManageUserInfoHandler)}.{nameof(ManageUserInfoHandler.Handle)}")!;
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

    public void MarkUserUpdated(UserDomain user)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent($"User {user.Id} updated"));

        logger.LogInformation("User updated: {Email}", user.Email);
    }

    public void MarkUserNotUpdated(UserDomain user, List<string> errors)
    {
        telemetryService
            .AddTag("user.exists", true)
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent($"User {user.Id} not updated"));

        foreach (var error in errors)
            telemetryService.AddEvent(new ActivityEvent(error));

        logger.LogWarning("User not updated: {Email}", user.Email);
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
