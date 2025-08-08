using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.DeleteRole;

public record DeleteRoleCommand : IRequest<Result<string>>
{
    private DeleteRoleCommand(string roleId)
    {
        RoleId = roleId;
    }

    public string RoleId { get; }

    public static DeleteRoleCommand Create(string roleId)
    {
        return new DeleteRoleCommand(roleId);
    }
}
