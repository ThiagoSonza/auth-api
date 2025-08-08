using Microsoft.AspNetCore.Identity;

namespace Application.Domain.Roles.Features.CreateRole;

public class CreateRoleResponse
{
    private CreateRoleResponse(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public static implicit operator CreateRoleResponse(IdentityRole role)
    {
        return new(role.Id, role.Name!);
    }
}
