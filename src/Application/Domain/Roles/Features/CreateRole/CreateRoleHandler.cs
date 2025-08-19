using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.CreateRole;

public class CreateRoleHandler(
    RoleManager<IdentityRole> roleManager,
    CreateRoleTelemetry telemetry
) : IRequestHandler<CreateRoleCommand, Result<CreateRoleResponse>>
{
    public async Task<Result<CreateRoleResponse>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        IdentityRole role = command;
        var roleExist = await roleManager.CreateAsync(role);
        if (roleExist.Succeeded)
        {
            telemetry.RoleCreated(role);
            return Result.Success((CreateRoleResponse)role);
        }

        var errors = roleExist.Errors.Select(e => e.Description).ToList();
        telemetry.RoleNotCreated(role.Name!, errors);
        return Result.Failure<CreateRoleResponse>(errors);
    }
}
