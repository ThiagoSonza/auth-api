using Microsoft.AspNetCore.Identity;

namespace Application.Domain.Roles.Features.GetRoles;

public record GetRolesResponse
{
    public GetRolesResponse(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; }
    public string Name { get; }

    public static implicit operator GetRolesResponse(IdentityRole role)
    {
        return new GetRolesResponse(role.Id, role.Name!);
    }
}
