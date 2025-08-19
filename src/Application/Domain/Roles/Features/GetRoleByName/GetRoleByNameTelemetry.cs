using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Roles.Features.GetRoleByName;

public class GetRoleByNameTelemetry(
    ActivitySource activitySource,
    ILogger<GetRoleByNameTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(GetRoleByNameHandler)}.{nameof(GetRoleByNameHandler.Handle)}")!;
    private bool disposed;

    public void MarkRoleFound(IdentityRole role)
    {
        telemetryService
            .AddTag("role.exists", true)
            .AddTag("role.id", role.Id)
            .AddTag("role.name", role.Name)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent($"Role {role.Name} found"));

        logger.LogInformation("Role found: {RoleId}", role.Id);
    }

    public void MarkRoleNotFound(string roleName)
    {
        telemetryService
            .AddTag("role.exists", false)
            .AddTag("role.name", roleName)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent($"Role {roleName} not found"));

        logger.LogError("Role not found: {RoleName}", roleName);
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
