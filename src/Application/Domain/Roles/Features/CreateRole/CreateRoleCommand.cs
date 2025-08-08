using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.CreateRole;

public class CreateRoleCommand : IRequest<Result<CreateRoleResponse>>
{
    private CreateRoleCommand(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public static implicit operator CreateRoleCommand(CreateRoleRequest request)
    {
        return new CreateRoleCommand(request.Name);
    }

    public static implicit operator IdentityRole(CreateRoleCommand command)
    {
        return new IdentityRole(command.Name)
        {
            Name = command.Name,
            NormalizedName = command.Name.ToUpperInvariant()
        };
    }
}
