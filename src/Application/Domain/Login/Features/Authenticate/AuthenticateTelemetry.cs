using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Login.Features.Authenticate;

public class AuthenticateTelemetry(
    ActivitySource activitySource,
    ILogger<AuthenticateTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(AuthenticateHandler)}.{nameof(AuthenticateHandler.Handle)}")!;
    private bool disposed;

    public void AuthenticateSuccessful(string email)
    {
        telemetryService
            .AddTag("user.email", email)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Authentication successful"));

        logger.LogInformation("Authentication successful for user: {Email}", email);
    }

    public void AuthenticateFailed(string email, IEnumerable<string> errors)
    {
        telemetryService
            .AddTag("user.email", email)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("Authentication failed"));

        foreach (var error in errors)
            telemetryService.AddEvent(new ActivityEvent(error));

        logger.LogError("Authentication failed for user: {Email}. Errors: {Errors}", email, errors);
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
