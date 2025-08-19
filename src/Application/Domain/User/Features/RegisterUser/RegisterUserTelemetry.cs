using System.Diagnostics;
using Domain.User;
using Microsoft.Extensions.Logging;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterUserTelemetry(
    ActivitySource activitySource,
    ILogger<RegisterUserTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(RegisterUserHandler)}.{nameof(RegisterUserHandler.Handle)}")!;
    private bool disposed;

    public void MarkUserRegistered(UserDomain user)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent($"User {user.Id} created"));

        logger.LogInformation("User created: {Email}", user.Email);
    }

    public void MarkUserRegistrationFailed(UserDomain user, List<string> errors)
    {
        telemetryService
            .AddTag("user.email", user.Email)
            .AddTag("user.id", user.Id)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent($"User {user.Id} not created"));

        foreach (var error in errors)
            telemetryService.AddEvent(new ActivityEvent(error));

        logger.LogWarning("User not created: {Email}", user.Email);
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