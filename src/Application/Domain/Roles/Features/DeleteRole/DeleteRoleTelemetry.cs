using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Roles.Features.DeleteRole;

public class DeleteRoleTelemetry(
    ActivitySource activitySource,
    ILogger<DeleteRoleTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(DeleteRoleHandler)}.{nameof(DeleteRoleHandler.Handle)}")!;
    private bool disposed;

    public void MarkRoleDeleted(IdentityRole role)
    {
        telemetryService
            .AddTag("role.id", role.Id)
            .AddTag("role.name", role.Name)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent($"Role {role.Name} deleted"));

        logger.LogInformation("Role deleted: {RoleName} with ID: {RoleId}", role.Name, role.Id);
    }

    public void MarkRoleDeletionFailed(string id, List<string> errors)
    {
        telemetryService
            .AddTag("role.id", id)
            .AddTag("errors", string.Join(", ", errors))
            .SetStatus(ActivityStatusCode.Error);

        foreach (var error in errors)
            telemetryService.AddEvent(new ActivityEvent(error));

        logger.LogWarning("Role not deleted. Errors: {Errors}", string.Join(", ", errors));
    }

    public void MarkRoleNotFound(string roleId)
    {
        telemetryService
            .AddTag("role.exists", false)
            .AddTag("role.id", roleId)
            .SetStatus(ActivityStatusCode.Error)
            .AddEvent(new ActivityEvent($"Role {roleId} not found"));

        logger.LogError("Role not found: {RoleId}", roleId);
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
