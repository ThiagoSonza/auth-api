using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.GetRoleByName;

public class GetRoleByNameHandler(
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<GetRoleByNameCommand, Result<GetRoleByNameResponse>>
{
    public async Task<Result<GetRoleByNameResponse>> Handle(GetRoleByNameCommand command, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByNameAsync(command.RoleName);
        if (role is null)
            return Result.Failure<GetRoleByNameResponse>("Grupo não encontrado");

        GetRoleByNameResponse response = role;
        return Result.Success(response);
    }
}
