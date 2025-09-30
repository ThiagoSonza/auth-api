using System.Diagnostics;

namespace Worker.ManageUserContext.User.RegisterUser;

public class RegisterUserTelemetry(
    ActivitySource activitySource,
    ILogger<RegisterUserTelemetry> logger) : IDisposable
{
    private Activity? telemetryService;
    private bool disposed;

    public void ReceivedMessage(RegisterUserMessage message)
    {
        telemetryService = activitySource.StartActivity($"{nameof(RegisterUserConsumer)}.{nameof(RegisterUserConsumer.HandleAsync)}")!;

        telemetryService?
            .AddTag("user.email", message.Email)
            .AddTag("user.name", message.Name)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Received register user message"));

        logger.LogInformation("Received register user message for: {UserEmail}", message.Email);
    }

    public void SendingEmail(RegisterUserMessage message)
    {
        telemetryService?
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Sending welcome email"));

        logger.LogInformation("Sending welcome email to: {UserEmail}", message.Email);
    }

    public void EmailSent(RegisterUserMessage message)
    {
        telemetryService?
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Welcome email sent"));

        logger.LogInformation("Welcome email sent to: {UserEmail}", message.Email);
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