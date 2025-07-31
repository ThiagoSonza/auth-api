using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.GetRoleByName;

public class GetRoleByNameCommand : IRequest<Result<GetRoleByNameResponse>>
{
    private GetRoleByNameCommand(string roleName)
    {
        RoleName = roleName;
    }

    public string RoleName { get; }

    public static GetRoleByNameCommand Create(string roleName)
    {
        return new GetRoleByNameCommand(roleName);
    }
}
