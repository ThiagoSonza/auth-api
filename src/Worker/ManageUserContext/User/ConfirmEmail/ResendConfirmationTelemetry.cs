using System.Diagnostics;

namespace Worker.ManageUserContext.User.ConfirmEmail;

public class ResendConfirmationTelemetry(
    ActivitySource activitySource,
    ILogger<ResendConfirmationTelemetry> logger) : IDisposable
{
    private Activity? telemetryService;
    private bool disposed;

    public void ReceivedMessage(ResendConfirmationMessage message)
    {
        telemetryService = activitySource.StartActivity($"{nameof(ResendConfirmationConsumer)}.{nameof(ResendConfirmationConsumer.HandleAsync)}")!;

        telemetryService?
            .AddTag("user.id", message.UserId)
            .AddTag("user.name", message.Name)
            .AddTag("user.email", message.Email)
            .AddTag("token", message.Token)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Received confirmation email resend message"));

        logger.LogInformation("Received confirmation email resend message for: {UserId} | {UserEmail}", message.UserId, message.Email);
    }

    public void SendingEmail(ResendConfirmationMessage message)
    {
        telemetryService?
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Sending confirmation email"));

        logger.LogInformation("Sending confirmation email to: {UserId} | {UserEmail}", message.UserId, message.Email);
    }

    public void EmailSent(ResendConfirmationMessage message)
    {
        telemetryService?
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Confirmation email sent"));

        logger.LogInformation("Confirmation email sent to: {UserId} | {UserEmail}", message.UserId, message.Email);
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
            telemetryService?.Dispose();

        disposed = true;
    }
}