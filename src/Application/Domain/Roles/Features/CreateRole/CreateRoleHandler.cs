using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.CreateRole;

public class CreateRoleHandler(
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<CreateRoleCommand, Result>
{
    public async Task<Result> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        IdentityRole role = command;
        var roleExist = await roleManager.CreateAsync(role);
        if (roleExist.Succeeded)
            return Result.Success("Grupo criado com sucesso");

        var errors = roleExist.Errors.Select(e => e.Description).ToList();
        return Result.Failure(errors);
    }
}
