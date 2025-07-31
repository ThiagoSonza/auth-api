using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.GetRoles;

public record GetRolesCommand : IRequest<Result<IEnumerable<GetRolesResponse>>>
{
    private GetRolesCommand()
    {
    }

    public static GetRolesCommand Create()
    {
        return new GetRolesCommand();
    }
}
