using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.GetRoleByName;

public class GetRoleByNameHandler(
    RoleManager<IdentityRole> roleManager,
    GetRoleByNameTelemetry telemetry
) : IRequestHandler<GetRoleByNameCommand, Result<GetRoleByNameResponse>>
{
    public async Task<Result<GetRoleByNameResponse>> Handle(GetRoleByNameCommand command, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByNameAsync(command.RoleName);
        if (role is null)
        {
            telemetry.MarkRoleNotFound(command.RoleName);
            return Result.Failure<GetRoleByNameResponse>("Grupo não encontrado");
        }

        telemetry.MarkRoleFound(role);

        GetRoleByNameResponse response = role;
        return Result.Success(response);
    }
}
