
using System.Diagnostics;

namespace Worker.ManageUserContext.Password.ForgotPassword;

public class ForgotPasswordTelemetry(
    ActivitySource activitySource,
    ILogger<ForgotPasswordTelemetry> logger) : IDisposable
{
    private Activity? telemetryService;
    private bool disposed;

    public void ReceivedMessage(ForgotPasswordMessage message)
    {
        telemetryService = activitySource.StartActivity($"{nameof(ForgotPasswordConsumer)}.{nameof(ForgotPasswordConsumer.HandleAsync)}")!;

        telemetryService
            .SetTag("user.name", message.Name)
            .SetTag("user.email", message.Email)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Received forgot password message"));

        logger.LogInformation("Received forgot password message for: {UserEmail}", message.Email);
    }

    public void SendingEmail(ForgotPasswordMessage message)
    {
        telemetryService?
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Sending forgot password email"));

        logger.LogInformation("Sending forgot password email for: {UserEmail}", message.Email);
    }

    public void EmailSent(ForgotPasswordMessage message)
    {
        telemetryService?
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Forgot password email sent"));

        logger.LogInformation("Forgot password email sent to: {UserEmail}", message.Email);
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