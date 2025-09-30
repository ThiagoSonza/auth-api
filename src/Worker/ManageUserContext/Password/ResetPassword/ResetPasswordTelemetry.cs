
using System.Diagnostics;

namespace Worker.ManageUserContext.Password.ResetPassword;

public class ResetPasswordTelemetry(
    ActivitySource activitySource,
    ILogger<ResetPasswordTelemetry> logger) : IDisposable
{
    private Activity? telemetryService;
    private bool disposed;

    public void ReceivedMessage(ResetPasswordMessage message)
    {
        telemetryService = activitySource.StartActivity($"{nameof(ResetPasswordConsumer)}.{nameof(ResetPasswordConsumer.HandleAsync)}")!;

        telemetryService
            .SetTag("user.name", message.Name)
            .SetTag("user.email", message.Email)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Received reset password message"));

        logger.LogInformation("Received reset password message for: {UserEmail}", message.Email);
    }

    public void SendingEmail(ResetPasswordMessage message)
    {
        telemetryService?
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Sending reset password email"));

        logger.LogInformation("Sending reset password email for: {UserEmail}", message.Email);
    }

    public void EmailSent(ResetPasswordMessage message)
    {
        telemetryService?
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("Reset password email sent"));

        logger.LogInformation("Reset password email sent to: {UserEmail}", message.Email);
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