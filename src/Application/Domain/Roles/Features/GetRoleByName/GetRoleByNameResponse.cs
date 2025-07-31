using Microsoft.AspNetCore.Identity;

namespace Application.Domain.Roles.Features.GetRoleByName;

public record GetRoleByNameResponse
{
    public GetRoleByNameResponse(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; init; }
    public string Name { get; init; }

    public static implicit operator GetRoleByNameResponse(IdentityRole role)
    {
        return new GetRoleByNameResponse(role.Id, role.Name!);
    }
}
