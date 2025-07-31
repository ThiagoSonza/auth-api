using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.GetRoles;

public class GetRolesHandler(
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<GetRolesCommand, Result<IEnumerable<GetRolesResponse>>>
{
    public async Task<Result<IEnumerable<GetRolesResponse>>> Handle(GetRolesCommand request, CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles.ToListAsync(cancellationToken);
        IEnumerable<GetRolesResponse> response = [.. roles.Select(role => (GetRolesResponse)role)];
        return Result.Success(response);
    }
}
