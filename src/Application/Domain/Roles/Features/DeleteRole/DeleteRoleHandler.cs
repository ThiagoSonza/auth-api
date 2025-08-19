using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.DeleteRole;

public class DeleteRoleHandler(
    RoleManager<IdentityRole> roleManager,
    DeleteRoleTelemetry telemetry
) : IRequestHandler<DeleteRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(command.RoleId);
        if (role is null)
        {
            telemetry.MarkRoleNotFound(command.RoleId);
            return Result.Failure<string>("Grupo não encontrado.");
        }

        var result = await roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            telemetry.MarkRoleDeleted(role);
            return Result.Success($"Grupo '{role.Name}' excluído com sucesso.");
        }

        var errors = result.Errors.Select(e => e.Description).ToList();
        telemetry.MarkRoleDeletionFailed(role.Id, errors);
        return Result.Failure<string>(errors);
    }
}
