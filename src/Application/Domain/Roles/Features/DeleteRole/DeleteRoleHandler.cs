using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.DeleteRole;

public class DeleteRoleHandler(
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByNameAsync(command.RoleName);
        if (role is null)
            return Result.Failure("Grupo não encontrado.");

        var result = await roleManager.DeleteAsync(role);
        if (result.Succeeded)
            return Result.Success($"Grupo '{role.Name}' excluído com sucesso.");

        var errors = result.Errors.Select(e => e.Description).ToList();
        return Result.Failure(errors);
    }
}
