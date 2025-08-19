using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Domain.Roles.Features.CreateRole;

public class CreateRoleTelemetry(
    ActivitySource activitySource,
    ILogger<CreateRoleTelemetry> logger
    ) : IDisposable
{
    private readonly Activity telemetryService = activitySource.StartActivity($"{nameof(CreateRoleHandler)}.{nameof(CreateRoleHandler.Handle)}")!;
    private bool disposed;

    public void RoleCreated(IdentityRole role)
    {
        telemetryService
            .AddTag("role.id", role.Id)
            .AddTag("role.name", role.Name)
            .SetStatus(ActivityStatusCode.Ok)
            .AddEvent(new ActivityEvent($"Role {role.Name} created"));

        logger.LogInformation("Role created: {RoleName} with ID: {RoleId}", role.Name, role.Id);
    }

    public void RoleNotCreated(string roleName, List<string> errors)
    {
        telemetryService
            .AddTag("role.name", roleName)
            .AddTag("errors", string.Join(", ", errors))
            .SetStatus(ActivityStatusCode.Error);

        foreach (var error in errors)
            telemetryService.AddEvent(new ActivityEvent(error));

        logger.LogWarning("Role not created. Errors: {Errors}", string.Join(", ", errors));
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
