using Domain.User;

namespace Application.Domain.User.Features.RegisterUser;

public record RegisterUserResponse
{
    private RegisterUserResponse(string id, string name)
    {
        Id = id;
        UserName = name;
    }

    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public static implicit operator RegisterUserResponse(UserDomain user)
    {
        return new(user.Id, user.UserName!);
    }
}
