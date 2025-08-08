using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.DeleteRole;

public class DeleteRoleHandler(
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<DeleteRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(command.RoleId);
        if (role is null)
            return Result.Failure<string>("Grupo não encontrado.");

        var result = await roleManager.DeleteAsync(role);
        if (result.Succeeded)
            return Result.Success($"Grupo '{role.Name}' excluído com sucesso.");

        var errors = result.Errors.Select(e => e.Description).ToList();
        return Result.Failure<string>(errors);
    }
}
