using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Mfa.Features.GenerateQrCode;

public class GenerateQrCodeTelemetry(
    ActivitySource activitySource,
    ILogger<GenerateQrCodeTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(GenerateQrCodeHandler)}.{nameof(GenerateQrCodeHandler.Handle)}")!;
    private bool disposed;

    public void MarkQrCodeGenerated(string userId, string image, string key)
    {
        telemetryService
            .AddTag("user.id", userId)
            .AddTag("qr.code.image", image)
            .AddTag("qr.code.key", key)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent("QR code generated"));

        logger.LogInformation("QR code generated for user: {UserId}", userId);
    }

    public void MarkUserNotFound(string userId)
    {
        telemetryService
            .AddTag("user.exists", false)
            .AddTag("user.id", userId)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent("User not found"));

        logger.LogError("User not found: {UserId}", userId);
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
