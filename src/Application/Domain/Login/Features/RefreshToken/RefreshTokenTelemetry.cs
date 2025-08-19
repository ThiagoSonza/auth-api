using System.Diagnostics;
using Domain.User;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Login.Features.RefreshToken;

public class RefreshTokenTelemetry(
    ActivitySource activitySource,
    ILogger<RefreshTokenTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(RefreshTokenHandler)}.{nameof(RefreshTokenHandler.Handle)}")!;
    private bool disposed;

    public void AuthenticateSuccessful(UserDomain user)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Authentication successful"));

        logger.LogInformation("Authentication successful for user: {UserId} {UserEmail}", user.Id, user.Email);
    }

    public void AuthenticateFailed(UserDomain user, List<string> errors)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("Authentication failed"));

        foreach (var error in errors)
            telemetryService.AddEvent(new ActivityEvent(error));

        logger.LogError("Authentication failed for user: {UserId} {UserEmail}. Errors: {Errors}", user.Id, user.Email, errors);
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
